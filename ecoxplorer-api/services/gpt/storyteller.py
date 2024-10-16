import json
import os
import boto3
import os
import time
import re

from typing import Dict, Any
from botocore.exceptions import ClientError

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event

from .src.storyteller_agent import Storyteller
from .src.openai import OpenAIModel
from .src.audio import OpenAITTSModel

from .config.prompts import get_story_teller_sys_message


def get_storyteller_model():
    model = OpenAIModel(
        api_key=os.getenv("OPENAI_API_KEY"),
        temperature=float(os.getenv("OPENAI_TEMPERATURE", "0.7")),
        model_engine=os.getenv("OPENAI_GPTMODEL"),
        max_tokens=int(os.getenv("OPENAI_TOKENS", "1000")),
    )
    system_message = get_story_teller_sys_message()
    storyteller = Storyteller(model, system_message)
    return storyteller


def parse_and_validate(event):
    body = load_body_from_event(event)
    json_analysis = body.get("json_analysis")
    if not json_analysis:
        raise ValueError("Json analysis is required.")

    language = body.get("language", "en")
    return json_analysis, language


def extract_json_from_response(content: str) -> Dict[str, Any]:
    json_match = re.search(r"```json\n(.*?)\n```", content, re.DOTALL)
    if json_match:
        json_content = json_match.group(1)
        try:
            return json.loads(json_content)
        except json.JSONDecodeError as e:
            raise ValueError("Invalid JSON in the response.")
    else:
        raise ValueError("No JSON content found in the response.")


@error_handling_decorator
def lambda_handler(event, context):
    json_analysis, language = parse_and_validate(event)
    storyteller = get_storyteller_model()

    res = storyteller.generate_story(json_analysis, language)
    print(res)
    if res.get("status") != "success":
        error_content = res["content"]
        raise ValueError(f"Error in the response: {error_content}")
    else:
        if not res.get("content"):
            raise ValueError("Content is required in the response.")

        try:
            response = extract_json_from_response(res["content"])
        except json.JSONDecodeError:
            raise ValueError("Invalid JSON in the response.")

        # Generate TTS
        story_text = response.get("story", "")
        if not story_text:
            raise ValueError("Story is required in the response.")

        tts_model = OpenAITTSModel(api_key=os.environ.get("OPENAI_API_KEY"))
        voice = select_character_voice(response.get("narrator", ""))
        tts_model.set_voice(voice)
        tts_result = tts_model.text_to_speech(story_text)

        if tts_result.get("status") != "success":
            raise ValueError(f"Error generating TTS: {tts_result.get('content')}")

        # Upload to S3
        s3_client = boto3.client("s3")
        bucket_name = os.environ.get("S3_BUCKET_NAME")
        folder_name = "storyteller"
        audio_file_name = (
            f"{folder_name}/tts_audio_{int(time.time())}.{tts_model.response_format}"
        )
        json_file_name = f"{folder_name}/response_{int(time.time())}.json"

        try:
            # Upload audio file
            s3_client.put_object(
                Bucket=bucket_name,
                Key=audio_file_name,
                Body=tts_result["audio_data"].getvalue(),
                ContentType=tts_result["content_type"],
            )

            # Upload JSON response
            s3_client.put_object(
                Bucket=bucket_name,
                Key=json_file_name,
                Body=json.dumps(response),
                ContentType="application/json",
            )

            # Generate normal S3 URLs without expiration
            region = s3_client.meta.region_name
            audio_url = (
                f"https://{bucket_name}.s3.{region}.amazonaws.com/{audio_file_name}"
            )
            json_url = (
                f"https://{bucket_name}.s3.{region}.amazonaws.com/{json_file_name}"
            )

            metadata = {
                "json_analysis": json.dumps(json_analysis),
                "audio_url": audio_url,
                "json_url": json_url,
            }
            body = response
            body.update(metadata)
            return success_response(body)

        except ClientError as e:
            raise ValueError(f"Error uploading to S3: {str(e)}")


def select_character_voice(character):
    character = character.lower()
    if "owl" in character:
        return "onyx"  # A deep, wise-sounding voice
    elif "fox" in character:
        return "fable"  # A clever, slightly mischievous voice
    elif "turtle" in character:
        return "echo"  # A slow, thoughtful voice
    elif "dolphin" in character:
        return "nova"  # A bright, playful voice
    else:
        return "alloy"  # Default voice

import os
import time
import logging
from typing import Tuple, Dict, Any
import boto3
from botocore.exceptions import ClientError
from .src.audio import OpenAITTSModel

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event

# Add character voice mapping configuration
CHARACTER_VOICES: Dict[str, str] = {
    "owl": "onyx",
    "lechuza": "onyx",
    "fox": "fable",
    "zorro": "fable",
    "turtle": "echo",
    "tortuga": "echo",
    "dolphin": "nova",
    "delfin": "nova",
}


def parse_and_validate(event: Dict[str, Any]) -> Tuple[str, str, str]:
    body = load_body_from_event(event)
    voice_id = body.get("voice_id")
    story_text = body.get("story")
    narrator = body.get("narrator", "").strip()

    errors = []
    if not story_text:
        errors.append("story text")
    if not narrator:
        errors.append("narrator")
    if not voice_id:
        errors.append("voice_id")

    if errors:
        raise ValueError(f"Missing required fields: {', '.join(errors)}")

    return story_text, narrator, voice_id


@error_handling_decorator
def lambda_handler(event, context):
    story_text, narrator, voice_id = parse_and_validate(event)
    voice_output_url, voice = generate_voice(story_text, narrator, voice_id)
    body = {
        "voice_id": voice_id,
        "voice_story_output": voice_output_url,
        "voice": voice,
    }
    return success_response(body)


def generate_voice(story_text, narrator, voice_id):
    tts_model = OpenAITTSModel(api_key=os.environ.get("OPENAI_API_KEY"))
    voice = select_character_voice(narrator)
    tts_model.set_voice(voice)
    tts_result = tts_model.text_to_speech(story_text)

    if tts_result.get("status") != "success":
        raise ValueError(f"Error generating TTS: {tts_result.get('content')}")

    # Upload to S3 and get URL
    voice_output_url = upload_to_s3(tts_result, voice_id)
    return voice_output_url, voice


def select_character_voice(character: str) -> str:
    character = character.lower().strip()

    # Check direct matches in configuration
    for key, voice in CHARACTER_VOICES.items():
        if key in character:
            return voice

    logging.info(
        f"No specific voice found for character '{character}', using default voice"
    )
    return "alloy"


def upload_to_s3(tts_result: Dict[str, Any], voice_id: str) -> str:
    s3_client = boto3.client("s3")
    bucket_name = os.environ.get("S3_BUCKET_NAME")
    if not bucket_name:
        raise ValueError("S3_BUCKET_NAME environment variable is not set")

    folder_name = "voices"
    audio_file_name = f"{folder_name}/tts_audio_{voice_id}.mp3"

    s3_client.put_object(
        Bucket=bucket_name,
        Key=audio_file_name,
        Body=tts_result["audio_data"].getvalue(),
        ContentType=tts_result["content_type"],
    )

    region = s3_client.meta.region_name
    return f"https://{bucket_name}.s3.{region}.amazonaws.com/{audio_file_name}"

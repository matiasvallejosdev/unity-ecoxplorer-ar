import os
import time
import boto3
from botocore.exceptions import ClientError
from .src.audio import OpenAITTSModel

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event


def parse_and_validate(event):
    body = load_body_from_event(event)
    voice_id = body.get("voice_id")
    story_text = body.get("story")
    narrator = body.get("narrator")
    if not story_text or not narrator or not voice_id:
        raise ValueError("Story text, narrator, and voice_id are required.")
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


def select_character_voice(character):
    character = character.lower()
    if "owl" in character or "lechuza" in character:
        return "onyx"  # A deep, wise-sounding voice
    elif "fox" in character or "zorro" in character:
        return "fable"  # A clever, slightly mischievous voice
    elif "turtle" in character or "tortuga" in character:
        return "echo"  # A slow, thoughtful voice
    elif "dolphin" in character or "delfin" in character:
        return "nova"  # A bright, playful voice
    else:
        return "alloy"  # Default voice


def upload_to_s3(tts_result, voice_id):
    s3_client = boto3.client("s3")
    bucket_name = os.environ.get("S3_BUCKET_NAME")
    folder_name = "voices"
    audio_file_name = f"{folder_name}/tts_audio_{voice_id}.mp3"

    try:
        # Upload audio file to S3
        s3_client.put_object(
            Bucket=bucket_name,
            Key=audio_file_name,
            Body=tts_result["audio_data"].getvalue(),
            ContentType=tts_result["content_type"],
        )

        # Generate normal S3 URL without expiration
        region = s3_client.meta.region_name
        voice_output = (
            f"https://{bucket_name}.s3.{region}.amazonaws.com/{audio_file_name}"
        )
        return voice_output
    except ClientError as e:
        raise ValueError(f"Error uploading to S3: {str(e)}")

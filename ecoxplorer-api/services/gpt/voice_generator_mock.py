from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response


@error_handling_decorator
def lambda_handler(event, context):
    voice_generator_mock = {
        "voice_id": "76db0bd0-df4a-4413-895e-c85b922d5ae2",
        "voice_story_output": "https://ecoxplorer-dev.s3.us-east-1.amazonaws.com/storyteller/tts_audio_76db0bd0-df4a-4413-895e-c85b922d5ae2.mp3",
        "voice": "fable",
    }
    return success_response(voice_generator_mock)

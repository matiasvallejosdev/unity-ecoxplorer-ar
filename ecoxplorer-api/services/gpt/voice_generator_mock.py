from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event


def parse_and_validate(event):
    body = load_body_from_event(event)
    story_id = body.get("story_id")
    story_text = body.get("story")
    narrator = body.get("narrator")
    if not story_text or not narrator or not story_id:
        raise ValueError("Story text, narrator, and story_id are required.")
    return story_text, narrator, story_id


@error_handling_decorator
def lambda_handler(event, context):
    parse_and_validate(event)

    voice_generator_mock = {
        "story_id": "76db0bd0-df4a-4413-895e-c85b922d5ae2",
        "voice_story_output": "https://ecoxplorer-dev.s3.us-east-1.amazonaws.com/storyteller/tts_audio_76db0bd0-df4a-4413-895e-c85b922d5ae2.mp3",
        "voice": "fable",
    }
    return success_response(voice_generator_mock)

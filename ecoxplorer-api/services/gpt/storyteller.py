import json
import os
import uuid
from datetime import datetime

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.parser import extract_json_from_response

from .src.agents.storyteller_agent import Storyteller
from .src.openai import OpenAIModel

from .config.prompts import get_sys_storyteller_agent


def get_storyteller_model():
    system_message = get_sys_storyteller_agent()
    model = OpenAIModel(
        api_key=os.getenv("OPENAI_API_KEY"),
        temperature=float(os.getenv("OPENAI_TEMPERATURE", "0.7")),
        model_engine=os.getenv("OPENAI_GPTMODEL"),
        max_tokens=int(os.getenv("OPENAI_TOKENS", "1000"))
    )
    storyteller = Storyteller(model, system_message)
    return storyteller


def parse_and_validate(event):
    body = load_body_from_event(event)
    image_analysis = body.get("image_analysis")
    topic = body.get("topic")
    if not image_analysis:
        raise ValueError("Image analysis is required.")
    if not topic:
        raise ValueError("Topic is required.")
    language = body.get("language", "en")
    return image_analysis, topic, language


@error_handling_decorator
def lambda_handler(event, context):
    image_analysis, topic, language = parse_and_validate(event)
    storyteller = get_storyteller_model()

    res = storyteller.generate_story(image_analysis, topic, language)
    if res.get("status") != "success":
        error_content = res["content"]
        raise ValueError(f"Error in the response: {error_content}")
    else:
        if not res.get("content"):
            raise ValueError("Content is required in the response.")

        try:
            # Extract JSON from the response
            response = extract_json_from_response(res["content"])
        except json.JSONDecodeError:
            raise ValueError("Invalid JSON in the response.")

        # Generate TTS using OpenAI TTS
        story_text = response.get("story", "")
        if not story_text:
            raise ValueError("Story is required in the response.")

        # Prepare metadata
        image_id = image_analysis.get("image_id", "no-image-id")
        image_url = image_analysis.get("image_url", "no-image-url")

        # Prepare metadata for the response
        metadata = {
            "story_id": str(uuid.uuid4()),
            "image_recognition_output": json.dumps(image_analysis),
            "image_id": image_id,
            "image_url": image_url,
            "date_created": datetime.now().isoformat(),
        }
        body = response
        body.update(metadata)
        return success_response(body)

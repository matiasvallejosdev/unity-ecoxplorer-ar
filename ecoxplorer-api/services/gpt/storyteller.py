import json
import os
import uuid
from datetime import datetime
import re

from typing import Dict, Any

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event

from .src.storyteller_agent import Storyteller
from .src.openai import OpenAIModel

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
    image_analysis = body.get("image_analysis")
    if not image_analysis:
        raise ValueError("Image analysis is required.")

    language = body.get("language", "en")
    return image_analysis, language


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
    image_analysis, language = parse_and_validate(event)
    storyteller = get_storyteller_model()

    # Generate story with an Agent
    res = storyteller.generate_story(image_analysis, language)
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
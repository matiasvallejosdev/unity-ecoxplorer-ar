import json
import os
import re
from datetime import datetime
import uuid
import logging
from typing import Dict, Any

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event

from services.gpt.src.visiongpt_agent import VisionGPT
from services.gpt.src.openai import OpenAIModel

from .config.prompts import get_image_recognition_sys_message

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


def get_vision_gpt_model() -> VisionGPT:
    model = OpenAIModel(
        api_key=os.getenv("OPENAI_API_KEY"),
        temperature=float(os.getenv("OPENAI_TEMPERATURE", "0.7")),
        model_engine=os.getenv("OPENAI_GPTMODEL", "gpt-3.5-turbo"),
        max_tokens=int(os.getenv("OPENAI_TOKENS", "150")),
    )
    system_message = get_image_recognition_sys_message()
    visiongpt = VisionGPT(model, system_message)
    return visiongpt


def parse_and_validate(event: Dict[str, Any]) -> str:
    body = load_body_from_event(event)
    image_url = body.get("image_url")
    if not image_url:
        raise ValueError("Image URL is required.")
    return image_url


def extract_json_from_response(content: str) -> Dict[str, Any]:
    json_match = re.search(r"```json\n(.*?)\n```", content, re.DOTALL)
    if json_match:
        json_content = json_match.group(1)
        try:
            return json.loads(json_content)
        except json.JSONDecodeError as e:
            logger.error(f"JSON decode error: {e}")
            raise ValueError("Invalid JSON in the response.")
    else:
        logger.error("No JSON content found in the response.")
        raise ValueError("No JSON content found in the response.")


@error_handling_decorator
def lambda_handler(event: Dict[str, Any], context: Any) -> Dict[str, Any]:
    logger.info("Starting image recognition process")
    image_url = parse_and_validate(event)
    visiongpt = get_vision_gpt_model()

    logger.info(f"Recognizing image from URL: {image_url}")
    res = visiongpt.recognize_image(image_url)

    if res.get("status") != "success":
        error_content = res.get("content", "Unknown error")
        logger.error(f"Error in the response: {error_content}")
        raise ValueError(f"Error in the response: {error_content}")

    if not res.get("content"):
        logger.error("Content is required in the response.")
        raise ValueError("Content is required in the response.")

    response = extract_json_from_response(res["content"])

    metadata = {
        "image_id": str(uuid.uuid4()),
        "image_url": image_url,
        "date_created": datetime.now().isoformat(),
    }
    body = response
    body.update(metadata)

    logger.info("Image recognition process completed successfully")
    return success_response(body)

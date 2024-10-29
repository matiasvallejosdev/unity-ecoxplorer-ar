import os
from typing import Any, Dict, Tuple, List

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.parser import extract_json_from_response

from .src.agents.match_evaluator_agent import MatchAgent
from .src.openai import OpenAIModel

from .config.prompts import get_image_evaluation_sys_message


def parse_and_validate(event: Dict[str, Any]) -> Tuple[str, str]:
    body = load_body_from_event(event)
    image_url = body.get("image_url")
    if not image_url:
        raise ValueError("Image URL is required.")
    topic = body.get("topic")
    if not topic:
        raise ValueError("Topics are required.")
    return image_url, topic


def get_image_topic_match_agent():
    model = OpenAIModel(
        api_key=os.getenv("OPENAI_API_KEY"),
        temperature=float(os.getenv("OPENAI_TEMPERATURE", "0.7")),
        model_engine=os.getenv("OPENAI_GPTMODEL"),
        max_tokens=int(os.getenv("OPENAI_TOKENS", "1000")),
    )
    system_message = get_image_evaluation_sys_message()
    match_agent = MatchAgent(model, system_message)
    return match_agent


@error_handling_decorator
def lambda_handler(event, context):
    image_url, topic = parse_and_validate(event)
    match_agent = get_image_topic_match_agent()
    res = match_agent.evaluate_image(image_url, topic)

    if res.get("status") != "success":
        raise ValueError("Failed to evaluate image topic match.")

    if not res.get("content"):
        raise ValueError("Content is required in the response.")

    response = extract_json_from_response(res["content"])
    body = response

    return success_response(body)

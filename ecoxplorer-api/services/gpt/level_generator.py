import os
from typing import Dict, Tuple

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.parser import extract_json_from_response

from .src.agents.level_generator_agent import LevelGeneratorAgent
from .src.openai import OpenAIModel

from .config.prompts import get_level_generator_sys_message


def parse_and_validate(event: Dict) -> Tuple[str, str]:
    body = load_body_from_event(event)
    topic = body.get("topic")
    if not topic:
        raise ValueError(
            "Topic must be included in the body to generate the level story."
        )
    language = body.get("language", "en")
    return topic, language


def get_level_generator_agent():
    model = OpenAIModel(
        api_key=os.getenv("OPENAI_API_KEY"),
        temperature=float(os.getenv("OPENAI_TEMPERATURE", "0.7")),
        model_engine=os.getenv("OPENAI_GPTMODEL"),
        max_tokens=int(os.getenv("OPENAI_TOKENS", "1000")),
    )
    system_message = get_level_generator_sys_message()
    match_agent = LevelGeneratorAgent(model, system_message)
    return match_agent


@error_handling_decorator
def lambda_handler(event, context):
    topic, language = parse_and_validate(event)
    level_agent = get_level_generator_agent()
    res = level_agent.generate_level(topic, language)

    if res.get("status") != "success":
        raise ValueError("Failed to evaluate image topic match.")

    if not res.get("content"):
        raise ValueError("Content is required in the response.")

    response = extract_json_from_response(res["content"])
    body = response

    return success_response(body)

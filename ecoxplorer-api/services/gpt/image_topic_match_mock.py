from typing import Dict, Any, Tuple, List

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event

def parse_and_validate(event: Dict[str, Any]) -> Tuple[str, List[str]]:
    body = load_body_from_event(event)
    image_url = body.get("image_url")
    if not image_url:
        raise ValueError("Image URL is required.")
    topics = body.get("topics")
    if not topics:
        raise ValueError("Topics are required.")
    return image_url, topics

@error_handling_decorator
def lambda_handler(event, context):
    parse_and_validate(event)
    
    response = {
        "match": True,
        "confidence": 0.95,
    }
    return success_response(response)

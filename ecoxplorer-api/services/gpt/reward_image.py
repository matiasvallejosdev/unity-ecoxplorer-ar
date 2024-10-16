import os
from datetime import datetime
from typing import Dict, Any
import boto3

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event


from services.gpt.src.stability import StabilityModel


def get_stability_model() -> StabilityModel:
    return StabilityModel(region="us-east-1")


def parse_and_validate(event: Dict[str, Any]) -> str:
    body = load_body_from_event(event)
    achievement = body.get("achievement")
    if not achievement:
        raise ValueError("Achievement is required.")
    return achievement


@error_handling_decorator
def lambda_handler(event, context):
    achievement = parse_and_validate(event)
    model = get_stability_model()
    res = model.image_generation(achievement)
    if res.get("status") != "success":
        raise ValueError("Error in the response.")
    
    image_64 = res.get("content")
    s3 = boto3.client("s3")
    bucket_name = os.environ.get("S3_BUCKET_NAME")
    if not bucket_name:
        raise ValueError("S3 bucket name is required.")
    
    datetime_str = datetime.now().strftime("%Y%m%d%H%M%S")
    image_name = f"{achievement}_{datetime_str}.png"
    s3.put_object(Bucket=bucket_name, Key=image_name, Body=image_64)
    image_url = f"https://{bucket_name}.s3.amazonaws.com/{image_name}"
    return success_response({"image_url": image_url})

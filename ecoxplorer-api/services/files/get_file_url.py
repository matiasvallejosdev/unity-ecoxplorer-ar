import os
import logging

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response

s3_bucket_name = os.getenv("S3_BUCKET_NAME")
s3_region = os.getenv("S3_BUCKET_REGION")

logger = logging.getLogger()
logger.setLevel(logging.INFO)


def parse_and_validate(event):
    query_params = event.get("queryStringParameters", {})
    if not query_params.get("key"):
        raise ValueError("Key parameter is required.")
    key = query_params["key"]
    return key


@error_handling_decorator
def lambda_handler(event, context):
    key = parse_and_validate(event)
    url = f"https://{s3_bucket_name}.s3.{s3_region}.amazonaws.com/images/{key}"
    body = {"url": url}
    return success_response(body)

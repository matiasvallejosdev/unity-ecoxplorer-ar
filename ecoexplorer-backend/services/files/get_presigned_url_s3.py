import os
import datetime
import boto3

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response

s3_client = boto3.client("s3")
s3_bucket_name = os.getenv("S3_BUCKET_NAME")


def parse_and_validate(event):
    query_params = event.get("queryStringParameters", {})
    s3_key = query_params.get("key")
    s3_expiresin = query_params.get("expires_in")

    if not s3_key or s3_expiresin is None:
        raise ValueError(
            "Both 'key' and 'expires_in' parameters are required in the query."
        )

    try:
        s3_expiresin = int(s3_expiresin)
    except ValueError:
        raise ValueError("'expires_in' must be a valid integer.")

    if (
        s3_expiresin <= 0 or s3_expiresin > 86400
    ):  # Example: limits expires_in to 24 hours
        raise ValueError("'expires_in' must be between 1 and 86400 seconds.")

    return s3_key, s3_expiresin


@error_handling_decorator
def lambda_handler(event, context):
    s3_key, s3_expiresin = parse_and_validate(event)
    key = f"images/{s3_key}"

    url = s3_client.generate_presigned_post(
        Bucket=s3_bucket_name, Key=key, ExpiresIn=s3_expiresin
    )

    body = {
        "url": url,
        "key_file": s3_key,
        "expires_in": s3_expiresin,
        "created_at": datetime.datetime.now().isoformat(),
    }
    return success_response(body)

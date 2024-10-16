# response_utils.py
import json
from common.utils.encoders import DecimalEncoder


def success_response(data, status_code=200):
    """Generate a success response

    Args:
        data (T): Response data
        status_code (int, optional): Status code. Defaults to 200.

    Returns:
        dict: Response object for API Gateway
    """
    return {
        "statusCode": status_code,
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Content-Type": "application/json",
        },
        "body": json.dumps(data, cls=DecimalEncoder),
    }

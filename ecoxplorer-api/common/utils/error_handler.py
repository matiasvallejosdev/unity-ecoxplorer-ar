import json


def error_response(message, status_code=400):
    """Generate an error response (400)

    Args:
        message (str): Error message
        status_code (int, optional): Status code. Defaults to 400.

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
        "body": json.dumps({"message": message}),
    }


def not_found_error(e):
    """Not found error response (404)

    Returns:
        dict: Response object for API Gateway
    """
    return error_response(str(e), 404)


def internal_server_error(e):
    """Internal server error response (500)

    Args:
        message (str): Error message

    Returns:
        dict: Response object for API Gateway
    """
    return error_response(str(e), 500)

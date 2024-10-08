import json


def load_query_parameter_from_event(event):
    """Load query parameters from an API Gateway event

    Args:
        event (dict): The event object passed to the Lambda function

    Returns:
        dict: The query parameters as a dictionary
    """
    return event.get("queryStringParameters", {})


def load_body_from_event(event):
    """Load the body from an API Gateway event

    Args:
        event (dict): The event object passed to the Lambda function

    Returns:
        dict: The body as a dictionary
    """
    body = event.get("body", "{}")
    if isinstance(body, str):
        try:
            body = json.loads(body)
        except json.JSONDecodeError:
            body = {}
    return body


def load_path_parameter_from_event(event):
    """Load a path parameter from an API Gateway event

    Args:
        event (dict): The event object passed to the Lambda function
        param_name (str): The name of the path parameter to load

    Returns:
        str: The path parameter value
    """
    return event.get("pathParameters", {})

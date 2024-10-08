import pytest
from unittest.mock import Mock
import json
import services.health.health_check as lambda_module


pytestmark = [pytest.mark.unit]


def test_lambda_handler():
    event = {}
    context = Mock()
    context.function_name = "my-function"
    context.invoked_function_arn = (
        "arn:aws:lambda:us-east-1:123456789012:function:my-function"
    )

    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == 200
    response_body = json.loads(response["body"])
    expected_body = {
        "message": "Health check successful",
        "region": "us-east-1",
        "function_name": "my-function",
    }
    assert response_body == expected_body

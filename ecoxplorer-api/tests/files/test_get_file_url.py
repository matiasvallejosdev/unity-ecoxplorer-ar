import pytest
import json
import services.files.get_file_url as lambda_module

pytestmark = [pytest.mark.e2e]


@pytest.mark.unit
def test_parse_and_validate_valid_input():
    event = {"queryStringParameters": {"key": "test-file.jpg"}}
    key = lambda_module.parse_and_validate(event)
    assert key == "test-file.jpg"


@pytest.mark.unit
def test_parse_and_validate_missing_key():
    event = {"queryStringParameters": {}}
    with pytest.raises(ValueError, match="Key parameter is required."):
        lambda_module.parse_and_validate(event)


@pytest.mark.unit
def test_parse_and_validate_empty_key():
    event = {"queryStringParameters": {"key": ""}}
    with pytest.raises(ValueError, match="Key parameter is required."):
        lambda_module.parse_and_validate(event)


def test_lambda_handler_error_internal_server():
    event = {"queryStringParameters": {}}
    context = {}

    expected_response = {
        "statusCode": 400,
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Content-Type": "application/json",
        },
        "body": json.dumps({"message": "Key parameter is required."}),
    }
    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert response["body"] == expected_response["body"]


def test_lambda_handler_unexpected_error():
    event = {"queryStringParameters": {"key": "test-file.jpg"}}
    context = {}

    # Simulate an unexpected error by raising an exception inside parse_and_validate
    original_parse_and_validate = lambda_module.parse_and_validate

    def mock_parse_and_validate(event):
        raise Exception("Unexpected error")

    lambda_module.parse_and_validate = mock_parse_and_validate

    expected_response = {
        "statusCode": 500,
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Content-Type": "application/json",
        },
        "body": json.dumps({"message": "Unexpected error"}),
    }

    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert response["body"] == expected_response["body"]

    # Restore the original function
    lambda_module.parse_and_validate = original_parse_and_validate


def test_lambda_handler_success():
    event = {"queryStringParameters": {"key": "test-file.jpg"}}
    context = {}

    expected_response = {
        "statusCode": 200,
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Content-Type": "application/json",
        },
        "body": json.dumps({"url": "expected_url"}),
    }

    response = lambda_module.lambda_handler(event, context)
    body = json.loads(response["body"])

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert len(body["url"]) > 0


def test_lambda_handler_error_invalid_query_type():
    event = {"queryStringParameters": {"key": 123}}
    context = {}

    expected_response = {
        "statusCode": 400,
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Content-Type": "application/json",
        },
        "body": json.dumps({"message": "Key parameter must be a string."}),
    }

    # Simulate type error in parse_and_validate
    original_parse_and_validate = lambda_module.parse_and_validate

    def mock_parse_and_validate(event):
        query_params = event.get("queryStringParameters", {})
        if not isinstance(query_params.get("key"), str):
            raise TypeError("Key parameter must be a string.")
        return query_params["key"]

    lambda_module.parse_and_validate = mock_parse_and_validate

    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert response["body"] == expected_response["body"]

    # Restore the original function
    lambda_module.parse_and_validate = original_parse_and_validate

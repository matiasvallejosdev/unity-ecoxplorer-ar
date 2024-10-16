import json
import pytest
import services.files.get_presigned_url_s3 as lambda_module

pytestmark = [pytest.mark.e2e]

event_valid = {"queryStringParameters": {"key": "example.jpg", "expires_in": "3600"}}

event_invalid_key = {"queryStringParameters": {"expires_in": "3600"}}

event_invalid_expires_in = {
    "queryStringParameters": {"key": "example.jpg", "expires_in": "invalid"}
}

event_expires_in_out_of_range = {
    "queryStringParameters": {"key": "example.jpg", "expires_in": "90000"}
}


@pytest.mark.unit
def test_parse_and_validate_valid():
    s3_key, s3_expiresin = lambda_module.parse_and_validate(event_valid)
    assert s3_key == "example.jpg"
    assert s3_expiresin == 3600


@pytest.mark.unit
def test_parse_and_validate_missing_key():
    with pytest.raises(
        ValueError,
        match="Both 'key' and 'expires_in' parameters are required in the query.",
    ):
        lambda_module.parse_and_validate(event_invalid_key)


@pytest.mark.unit
def test_parse_and_validate_invalid_expires_in():
    with pytest.raises(ValueError, match="'expires_in' must be a valid integer."):
        lambda_module.parse_and_validate(event_invalid_expires_in)


@pytest.mark.unit
def test_parse_and_validate_expires_in_out_of_range():
    with pytest.raises(
        ValueError, match="'expires_in' must be between 1 and 86400 seconds."
    ):
        lambda_module.parse_and_validate(event_expires_in_out_of_range)


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
        "body": "{\"message\": \"Both 'key' and 'expires_in' parameters are required in the query.\"}",
    }
    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert response["body"] == expected_response["body"]


def test_lambda_handler_unexpected_error():
    event = {"queryStringParameters": {"key": "example.jpg", "expires_in": "3600"}}
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
        "body": '{"message": "Unexpected error"}',
    }
    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert response["body"] == expected_response["body"]
    lambda_module.parse_and_validate = original_parse_and_validate


def test_lambda_handler_success():
    event = {"queryStringParameters": {"key": "example.jpg", "expires_in": "3600"}}
    context = {}

    expected_response = {
        "statusCode": 200,
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Content-Type": "application/json",
        },
    }

    response = lambda_module.lambda_handler(event, context)
    body = json.loads(response["body"])
    print(body)

    assert response["statusCode"] == expected_response["statusCode"]
    assert response["headers"] == expected_response["headers"]
    assert "url" in body
    assert "key_file" in body
    assert "expires_in" in body
    assert "created_at" in body
    assert "url" in body["url"]
    assert "fields" in body["url"]

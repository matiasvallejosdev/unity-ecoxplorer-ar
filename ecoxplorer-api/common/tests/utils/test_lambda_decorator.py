import pytest
import json
import logging

pytestmark = [pytest.mark.unit]


@pytest.fixture
def capture_logs(caplog):
    caplog.set_level(logging.INFO)
    return caplog


def test_successful_execution(decorated_function, capture_logs):
    event = {"key": "value"}
    context = {}
    result = decorated_function(event, context)
    assert result == {"statusCode": 200, "body": "Success"}
    assert f"Handling event: {event}" in capture_logs.text


def test_key_error(error_handling_decorator, capture_logs):
    event = {}
    context = {}

    @error_handling_decorator
    def function_with_key_error(event, context):
        raise KeyError("Missing key")

    result = function_with_key_error(event, context)

    assert result["statusCode"] == 400
    assert "Access-Control-Allow-Origin" in result["headers"]
    assert "Access-Control-Allow-Headers" in result["headers"]
    assert result["headers"]["Content-Type"] == "application/json"
    body = json.loads(result["body"])
    assert body["message"] == "'Missing key'"
    assert "KeyError: 'Missing key'" in capture_logs.text


def test_type_error(error_handling_decorator, capture_logs):
    event = {"key": 1}
    context = {}

    @error_handling_decorator
    def function_with_type_error(event, context):
        raise TypeError("Invalid type")

    result = function_with_type_error(event, context)

    assert result["statusCode"] == 400
    assert "Access-Control-Allow-Origin" in result["headers"]
    assert "Access-Control-Allow-Headers" in result["headers"]
    assert result["headers"]["Content-Type"] == "application/json"
    assert json.loads(result["body"])["message"] == "Invalid type"
    assert "TypeError: Invalid type" in capture_logs.text


def test_value_error(error_handling_decorator, capture_logs):
    event = {"key": "invalid"}
    context = {}

    @error_handling_decorator
    def function_with_value_error(event, context):
        raise ValueError("Invalid value")

    result = function_with_value_error(event, context)

    assert result["statusCode"] == 400
    assert "Access-Control-Allow-Origin" in result["headers"]
    assert "Access-Control-Allow-Headers" in result["headers"]
    assert result["headers"]["Content-Type"] == "application/json"
    assert json.loads(result["body"])["message"] == "Invalid value"
    assert "ValueError: Invalid value" in capture_logs.text


def test_file_not_found_error(error_handling_decorator, capture_logs):
    event = {"file": "non_existent.txt"}
    context = {}

    @error_handling_decorator
    def function_with_file_not_found(event, context):
        raise FileNotFoundError("File does not exist")

    result = function_with_file_not_found(event, context)

    assert result["statusCode"] == 404
    assert "Access-Control-Allow-Origin" in result["headers"]
    assert "Access-Control-Allow-Headers" in result["headers"]
    assert result["headers"]["Content-Type"] == "application/json"
    assert json.loads(result["body"])["message"] == "File does not exist"
    assert "FileNotFoundError: File does not exist" in capture_logs.text


def test_unhandled_exception(error_handling_decorator, capture_logs):
    event = {"key": "value"}
    context = {}

    @error_handling_decorator
    def function_with_unhandled_exception(event, context):
        raise Exception("Unhandled exception")

    result = function_with_unhandled_exception(event, context)

    assert result["statusCode"] == 500
    assert "Access-Control-Allow-Origin" in result["headers"]
    assert "Access-Control-Allow-Headers" in result["headers"]
    assert result["headers"]["Content-Type"] == "application/json"
    assert json.loads(result["body"])["message"] == "Unhandled exception"
    assert "Unhandled exception: Unhandled exception" in capture_logs.text

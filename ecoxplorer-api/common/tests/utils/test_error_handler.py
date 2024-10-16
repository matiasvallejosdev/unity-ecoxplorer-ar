import pytest

from common.utils.error_handler import (
    error_response,
    not_found_error,
    internal_server_error,
)

pytestmark = [pytest.mark.unit]


def test_error_response():
    response = error_response("An error occurred.", 400)
    assert response["statusCode"] == 400
    assert response["headers"]["Access-Control-Allow-Origin"] == "*"
    assert response["headers"]["Access-Control-Allow-Headers"] == "*"
    assert response["headers"]["Content-Type"] == "application/json"
    assert response["body"] == '{"message": "An error occurred."}'


def test_not_found_error():
    response = not_found_error("Not found.")
    assert response["statusCode"] == 404
    assert response["headers"]["Access-Control-Allow-Origin"] == "*"
    assert response["headers"]["Access-Control-Allow-Headers"] == "*"
    assert response["headers"]["Content-Type"] == "application/json"
    assert response["body"] == '{"message": "Not found."}'


def test_internal_server_error():
    response = internal_server_error("Internal server error.")
    assert response["statusCode"] == 500
    assert response["headers"]["Access-Control-Allow-Origin"] == "*"
    assert response["headers"]["Access-Control-Allow-Headers"] == "*"
    assert response["headers"]["Content-Type"] == "application/json"
    assert response["body"] == '{"message": "Internal server error."}'

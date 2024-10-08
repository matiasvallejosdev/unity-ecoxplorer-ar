import pytest

from common.utils.response_utils import success_response

pytestmark = [pytest.mark.unit]


def test_build_valid_success_response():
    data = {"role": "assistant", "content": "Hello, how are you?", "status": "success"}
    response = success_response(data)
    assert response["statusCode"] == 200
    assert response["headers"]["Access-Control-Allow-Origin"] == "*"
    assert response["headers"]["Access-Control-Allow-Headers"] == "*"
    assert response["headers"]["Content-Type"] == "application/json"
    assert response["body"] is not None

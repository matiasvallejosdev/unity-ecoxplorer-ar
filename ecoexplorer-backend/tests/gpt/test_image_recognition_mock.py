import pytest

import services.gpt.image_recognition_mock as lambda_module

pytestmark = [pytest.mark.unit]


def test_lambda_handler_success():
    event = {"body": {"image_url": "url"}}
    context = {}
    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == 200

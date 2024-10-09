import pytest
import json
import os
from unittest.mock import patch, MagicMock
import services.gpt.image_recognition as lambda_module

pytestmark = [pytest.mark.unit]


@patch.dict(
    os.environ,
    {
        "OPENAI_API_KEY": "test_api_key",
        "OPENAI_TEMPERATURE": "1",
        "OPENAI_GPTMODEL": "test_model_engine",
        "OPENAI_TOKENS": "150",
    },
)
def test_openai_model_creator():
    model = lambda_module.get_vision_gpt_model()
    return model


def test_parse_and_validate():
    event = {"body": {"image_url": "http://test.url"}}
    image_url = lambda_module.parse_and_validate(event)
    assert image_url == "http://test.url"


def test_parse_and_validate_missing_image_url():
    event = {"body": {}}
    with pytest.raises(ValueError, match="Image URL is required."):
        lambda_module.parse_and_validate(event)


@patch("services.gpt.config.prompts.get_image_recognition_sys_message")
@patch("common.utils.lambda_utils.load_body_from_event")
@patch("common.utils.error_handler.error_response")
def test_lambda_handler_missing_image_url(
    mock_error_response, mock_load_body_from_event, mock_sys_prompt
):
    mock_load_body_from_event.return_value = {}
    mock_sys_prompt.return_value = "system_message"

    expected_error_response = {
        "statusCode": 400,
        "body": json.dumps({"message": "Image URL is required."}),
    }
    mock_error_response.return_value = expected_error_response

    event = {"mock": "event"}
    context = {"mock": "context"}

    response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == expected_error_response["statusCode"]
    assert response["body"] == expected_error_response["body"]


@patch("services.gpt.src.visiongpt_agent.VisionGPT")
@patch("common.utils.lambda_utils.load_body_from_event")
@patch("common.utils.error_handler.error_response")
def test_lambda_handler_invalid_json(
    mock_error_response, mock_VisionGPT, mock_load_body_from_event
):
    mock_load_body_from_event.return_value = {"image_url": "http://test.url"}

    mock_visiongpt_instance = MagicMock()
    mock_VisionGPT.return_value = mock_visiongpt_instance
    mock_visiongpt_instance.recognition_image.return_value = {
        "status": "success",
        "content": "invalid_json",
    }

    expected_error_response = {
        "statusCode": 400,
        "body": json.dumps({"message": "Missing image_url in the request body"}),
    }
    mock_error_response.return_value = expected_error_response

    event = {"mock": "event"}
    context = {"mock": "context"}

    with patch("services.gpt.image_recognition.json.loads", side_effect=ValueError):
        response = lambda_module.lambda_handler(event, context)

    assert response["statusCode"] == 400
    assert response["body"] == json.dumps({"message": ""})

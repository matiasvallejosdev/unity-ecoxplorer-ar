import pytest

from services.gpt.src.models import ModelInterface

pytestmark = [pytest.mark.ai]


def test_chat_completion_success(model: ModelInterface):
    messages = [
        {
            "role": "user",
            "content": "Hello, how are you?",
        }
    ]
    response = model.chat_completion(messages)
    assert response["status"] == "success"
    assert response["role"] == "assistant"
    assert response["content"] is not None


def test_chat_completion_failed(model: ModelInterface):
    messages = []
    response = model.chat_completion(messages)
    assert response["status"] == "error"
    assert response["role"] == "system"
    assert response["content"] == "Invalid chat messages."


def test_chat_completion_invalid_messages(model: ModelInterface):
    messages = {}
    response = model.chat_completion(messages)
    assert response["status"] == "error"
    assert response["role"] == "system"
    assert response["content"] == "Invalid chat messages."

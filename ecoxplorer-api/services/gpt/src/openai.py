import openai
import logging
from typing import List, Dict
from services.gpt.src.models import ModelInterface

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


class OpenAIModel(ModelInterface):
    def __init__(
        self, api_key: str, model_engine: str, temperature: float, max_tokens: int
    ):
        openai.api_key = api_key
        self.model_engine = model_engine
        self.temperature = temperature
        self.max_tokens = max_tokens
        self.system_message = "You are a helpful assistant."

    def set_system_message(self, system_message: str):
        self.system_message = system_message

    def validate_chat_request(self, messages: List) -> bool:
        if not isinstance(messages, list) or not messages:
            return False
        if not all(
            isinstance(m, dict) and "role" in m and "content" in m for m in messages
        ):
            return False
        return True

    def chat_completion(self, messages: List) -> Dict[str, str]:
        if not self.validate_chat_request(messages):
            return {
                "status": "error",
                "role": "system",
                "content": "Invalid chat messages.",
            }

        try:
            # Add the system message to the beginning of the messages list
            full_messages = [
                {"role": "system", "content": self.system_message}
            ] + messages

            chat = openai.chat.completions.create(
                model=self.model_engine,
                messages=full_messages,
                temperature=self.temperature,
                max_tokens=self.max_tokens,
            )
            if chat.choices:
                return {
                    "status": "success",
                    "role": chat.choices[0].message.role,
                    "content": chat.choices[0].message.content,
                }
            else:
                return {
                    "status": "error",
                    "role": "system",
                    "content": "No response from model.",
                }
        except Exception as e:
            logger.error(f"Unexpected error: {str(e)}")
            return {
                "status": "error",
                "role": "system",
                "content": "An unexpected error occurred.",
            }

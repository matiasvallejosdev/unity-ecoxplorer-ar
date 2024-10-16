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
            chat = openai.chat.completions.create(
                model=self.model_engine,
                messages=messages,
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

    def image_generation(
        self,
        prompt: str,
        negprompt: List[str],
        style_preset: str,
        clip_guidance_preset: str,
        sampler: str,
        width: int,
        height: int,
    ) -> dict:
        try:
            response = openai.Image.create(prompt=prompt, n=1, size=f"{width}x{height}")
            return {"status": "success", "image_url": response["data"][0]["url"]}
        except Exception as e:
            logger.error(f"Unexpected error in image generation: {str(e)}")
            return {
                "status": "error",
                "content": "An unexpected error occurred during image generation.",
            }

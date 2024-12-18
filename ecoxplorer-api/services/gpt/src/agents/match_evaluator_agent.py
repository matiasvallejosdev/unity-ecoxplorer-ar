from typing import Dict

from ..models import ModelInterface
from ...config.prompts import get_input_match_agent


class MatchAgent:
    def __init__(self, model: ModelInterface, system_message: str):
        self.model = model
        self.model.set_system_message(system_message)
        self.messages = []

    def evaluate_image(self, image_url: str, topic: str) -> Dict[str, str]:
        if not image_url or not topic:
            raise ValueError("Invalid request.")

        message = get_input_match_agent(topic)
        self.messages.append(
            {
                "role": "user",
                "content": [
                    {"type": "image_url", "image_url": {"url": image_url}},
                    {"type": "text", "text": message},
                ],
            }
        )

        response = self.model.chat_completion(self.messages)
        return response

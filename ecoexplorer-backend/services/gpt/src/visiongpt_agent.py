from .models import ModelInterface
from typing import Dict

class VisionGPT:
    def __init__(self, model: ModelInterface, system_message: str):
        self.model = model
        self.system_message = system_message or "You're a general assistant."
        self.messages = []

    def _initialize(self):
        self.messages = [
            {
                "role": "system",
                "content": [{"type": "text", "text": self.system_message}],
            }
        ]

    def recognize_image(self, image_url: str) -> Dict[str, str]:
        if not image_url:
            raise ValueError("Image URL is required.")

        self._initialize()
        self.messages.append(
            {
                "role": "user",
                "content": [{"type": "image_url", "image_url": {"url": image_url}}],
            }
        )

        return self.model.chat_completion(self.messages)
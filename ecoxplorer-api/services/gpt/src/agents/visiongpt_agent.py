from ..models import ModelInterface
from typing import Dict


class VisionGPT:
    def __init__(self, model: ModelInterface, system_message: str):
        self.model = model
        self.model.set_system_message(system_message)
        self.messages = []

    def recognize_image(self, image_url: str) -> Dict[str, str]:
        if not image_url:
            raise ValueError("Image URL is required.")

        self.messages.append(
            {
                "role": "user",
                "content": [{"type": "image_url", "image_url": {"url": image_url}}],
            }
        )

        return self.model.chat_completion(self.messages)

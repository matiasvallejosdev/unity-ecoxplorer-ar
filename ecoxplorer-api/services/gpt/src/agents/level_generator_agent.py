from typing import Dict

from ..models import ModelInterface
from ...config.prompts import get_level_generator_input_message


class LevelGeneratorAgent:
    def __init__(self, model: ModelInterface, system_message: str):
        self.model = model
        self.model.set_system_message(system_message)
        self.system_message = system_message
        self.messages = []

    def generate_level(self, topic: str, language: str = "es") -> Dict:
        if not topic:
            raise ValueError("Topic must be included to generate a level.")

        message = get_level_generator_input_message(topic, language)
        self.messages.append({"role": "user", "content": message})

        response = self.model.chat_completion(self.messages)
        return response

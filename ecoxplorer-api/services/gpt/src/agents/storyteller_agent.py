from typing import Dict
from ..models import ModelInterface

from ...config.prompts import get_input_story_generator_agent


class Storyteller:
    def __init__(self, model: ModelInterface, system_message: str):
        self.model = model
        self.model.set_system_message(system_message)
        self.messages = []

    def __str__(self) -> str:
        return "Welcome to the AR Storyteller!"

    def generate_story(self, image_analysis: Dict, topic: str, language: str) -> str:
        if not image_analysis or not topic or not language:
            raise ValueError("Invalid request. All parameters are required.")

        message = get_input_story_generator_agent(image_analysis, topic, language)
        self.messages.append({"role": "user", "content": message})
        response = self.model.chat_completion(self.messages)
        return response

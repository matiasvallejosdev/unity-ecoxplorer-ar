from typing import Dict
from ..models import ModelInterface

from ...config.prompts import get_story_generator_input_message


class Storyteller:
    def __init__(self, model: ModelInterface, system_message: str):
        self.model = model
        self.model.set_system_message(system_message)
        self.messages = []

    def __str__(self) -> str:
        return "Welcome to the AR Storyteller!"

    def generate_story(self, json_analysis: Dict, language: str) -> str:
        if not json_analysis or not language:
            raise ValueError("Invalid request.")

        message = get_story_generator_input_message(json_analysis, language)
        self.messages.append({"role": "user", "content": message})
        response = self.model.chat_completion(self.messages)
        return response

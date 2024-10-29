from typing import List, Dict, Any
from abc import ABC, abstractmethod


class ModelInterface(ABC):
    @abstractmethod
    def set_system_message(self, system_message: str):
        pass

    @abstractmethod
    def validate_chat_request(self, messages: List) -> bool:
        """Validate chat request.
        Args:
            messages (list): List of messages with openai formatting.

        Returns:
            bool: Return True if chat request is valid.
        """
        pass

    @abstractmethod
    def chat_completion(self, messages: List) -> dict:
        """Chat completion to gpt using a openai api with a model.
        Args:
            messages (list): List of messages with openai formatting.

        Returns:
            dict: Return a dictionary containing the response information.
        """
        pass


class AudioModelInterface(ABC):
    @abstractmethod
    def text_to_speech(self, text: str) -> Dict[str, Any]:
        """Convert text to speech using an API.
        Args:
            text (str): The text to convert to speech.

        Returns:
            Dict[str, Any]: A dictionary containing the status and either the audio data or an error message.
        """
        pass

from typing import List
from abc import ABC, abstractmethod


class ModelInterface(ABC):
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

    @abstractmethod
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
        """Image generation from a given prompt.

        Args:
            prompt (str): Prompt for generate an image.
            negprompt (List[str]): List of negative prompts.
            style_preset (str): Style preset for the image.
            clip_guidance_preset (str): CLIP guidance preset.
            sampler (str): Sampler to use for image generation.
            width (int): Width of the generated image.
            height (int): Height of the generated image.

        Returns:
            dict: Return a dictionary containing the generated image information.
        """
        pass

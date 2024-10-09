from typing import Dict, Any
import openai
import io


class AudioModelInterface:
    def text_to_speech(self, text: str) -> Dict[str, Any]:
        """Convert text to speech using an API.
        Args:
            text (str): The text to convert to speech.

        Returns:
            Dict[str, Any]: A dictionary containing the status and either the audio data or an error message.
        """
        pass


class OpenAITTSModel(AudioModelInterface):
    def __init__(
        self,
        api_key: str,
        model: str = "tts-1",
        voice: str = "alloy",
        response_format: str = "mp3",
    ):
        super().__init__()
        openai.api_key = api_key
        self.model = model
        self.voice = voice
        self.response_format = response_format

    def __str__(self) -> str:
        return f"Model: OpenAI TTS. Model: {self.model}. Voice: {self.voice}. Format: {self.response_format}."

    def text_to_speech(self, text: str) -> Dict[str, Any]:
        try:
            if not text:
                raise ValueError("Text should not be empty.")

            response = openai.audio.speech.create(
                model=self.model,
                voice=self.voice,
                input=text,
                response_format=self.response_format,
            )

            # The response is a binary audio file
            audio_data = io.BytesIO(response.content)

            return {
                "status": "success",
                "audio_data": audio_data,
                "content_type": f"audio/{self.response_format}",
            }

        except ValueError as ve:
            return {
                "status": "error",
                "content": str(ve),
            }
        except openai.OpenAIError as e:
            return {
                "status": "error",
                "content": str(e),
            }

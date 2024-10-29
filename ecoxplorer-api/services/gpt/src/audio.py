import io
from typing import Dict, Any, Optional
import openai

from .models import AudioModelInterface


class OpenAITTSModel(AudioModelInterface):
    def __init__(
        self,
        api_key: str,
        model: str = "tts-1",
        voice: str = "alloy",
        response_format: str = "mp3",
    ):
        self.client = openai.OpenAI(api_key=api_key)
        self.set_model(model)
        self.set_voice(voice)
        self.set_response_format(response_format)

    def __str__(self) -> str:
        return f"Model: OpenAI TTS. Model: {self.model}. Voice: {self.voice}. Format: {self.response_format}."

    def set_model(self, model: str) -> None:
        valid_models = ["tts-1", "tts-1-hd"]  # Add more as they become available
        if model not in valid_models:
            raise ValueError(f"Invalid model. Choose from {valid_models}")
        self.model = model

    def set_voice(self, voice: str) -> None:
        valid_voices = ["alloy", "echo", "fable", "onyx", "nova", "shimmer"]
        if voice not in valid_voices:
            raise ValueError(f"Invalid voice. Choose from {valid_voices}")
        self.voice = voice

    def set_response_format(self, response_format: str) -> None:
        valid_formats = ["mp3", "opus", "aac", "flac"]
        if response_format not in valid_formats:
            raise ValueError(f"Invalid response format. Choose from {valid_formats}")
        self.response_format = response_format

    def text_to_speech(self, text: str) -> Dict[str, Any]:
        if not text:
            return {"status": "error", "content": "Text should not be empty."}

        try:
            response = self.client.audio.speech.create(
                model=self.model,
                voice=self.voice,
                input=text,
                response_format=self.response_format,
            )

            audio_data = io.BytesIO(response.content)
            return {
                "status": "success",
                "audio_data": audio_data,
                "content_type": f"audio/{self.response_format}",
            }

        except openai.OpenAIError as e:
            return {"status": "error", "content": str(e)}


def create_tts_model(
    provider: str, api_key: str, **kwargs
) -> Optional[AudioModelInterface]:
    if provider.lower() == "openai":
        return OpenAITTSModel(api_key, **kwargs)
    else:
        raise ValueError(f"Unsupported TTS provider: {provider}")

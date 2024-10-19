import random
import boto3
import json
import re
from .models import ModelInterface
from typing import List
import logging

logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger(__name__)


class StabilityModel(ModelInterface):
    def __init__(self, region: str):
        self.model_id = "stability.stable-diffusion-xl-v1"
        self.region_name = region or "us-east-1"
        self.accept = "application/json"
        self.content_type = "application/json"
        self.model = boto3.client(
            service_name="bedrock-runtime", region_name=self.region_name
        )

    def __str__(self):
        return f"BedrockModel initialized with modelId: {self.modelId}"

    def validate_chat_request(self, messages: List) -> bool:
        return super().validate_chat_request(messages)

    def chat_completion(self, messages: List) -> dict:
        return super().chat_completion(messages)

    def image_generation(
        self,
        achievement: str,
        width: int,
        height: int,
        style_preset: str = "digital-art",
        clip_guidance_preset: str = "FAST_BLUE",
        sampler: str = "K_DPM_2_ANCESTRAL",
    ) -> dict:
        try:
            # Input validation
            if width not in [256, 512, 1024, 2048] or height not in [
                256,
                512,
                1024,
                2048,
            ]:
                raise ValueError(
                    "Width and height must be one of 256, 512, 1024, or 2048"
                )

            # Sanitize achievement string
            sanitized_achievement = "".join(
                char for char in achievement if ord(char) < 128
            )
            style_prompt = "cute cartoon colorful sparkly nature elements"

            sanitized_base_prompt = (
                f"A vibrant, exciting reward badge for kids. {sanitized_achievement}. "
            )
            sanitized_full_prompt = sanitized_base_prompt + style_prompt

            # Apply ultra-minimal sanitization to the full prompt
            final_prompt = ultra_minimal_sanitize_prompt(sanitized_full_prompt)

            seed = random.randint(0, 4294967295)

            native_request = {
                "text_prompts": [{"text": final_prompt, "weight": 1.0}],
                "cfg_scale": 5,
                "seed": seed,
                "steps": 60,
                "style_preset": style_preset,
                "clip_guidance_preset": clip_guidance_preset,
                "sampler": sampler,
                "width": width,
                "height": height,
            }

            request = json.dumps(native_request)
            logger.debug(f"Request body: {request}")

            response = self.model.invoke_model(
                body=request,
                modelId=self.model_id,
                accept=self.accept,
                contentType=self.content_type,
            )
            response_body = json.loads(response.get("body").read())
            logger.debug(f"Response body: {response_body}")

            if "artifacts" not in response_body or not response_body["artifacts"]:
                raise ValueError("No artifacts found in the response")

            base_64_img_str = response_body["artifacts"][0].get("base64")
            if not base_64_img_str:
                raise ValueError("No base64 image data found in the response")

            return {
                "status": "success",
                "content": base_64_img_str,
            }
        except Exception as e:
            logger.error(f"Error occurred: {str(e)}", exc_info=True)
            error_message = f"Error details: {str(e)}\n"
            error_message += f"Request body: {request}\n"
            if hasattr(e, "response"):
                error_message += f"Response: {e.response.text}\n"
            logger.error(error_message)
            return {
                "status": "error",
                "content": error_message,
            }


def ultra_minimal_sanitize_prompt(prompt):
    # Only keep a very limited set of safe words
    safe_words = set(["cartoon", "badge", "colorful", "fun", "kids"])
    return " ".join(word for word in prompt.lower().split() if word in safe_words)

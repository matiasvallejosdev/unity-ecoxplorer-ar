import os
import pytest

from services.gpt.src.visiongpt_agent import VisionGPT
from services.gpt.src.openai import OpenAIModel

from dotenv import load_dotenv, find_dotenv

# Load environment variables
load_dotenv(find_dotenv())


@pytest.fixture
def model():
    model = OpenAIModel(
        api_key=os.getenv("OPENAI_API_KEY"),
        model_engine=os.getenv("OPENAI_GPTMODEL"),
        temperature=int(os.getenv("OPENAI_TEMPERATURE")),
        max_tokens=int(os.getenv("OPENAI_TOKENS")),
    )
    return model


@pytest.fixture
def system_message():
    system_message = "system_message"
    return system_message


@pytest.fixture
def visiongpt(model, system_message):
    visiongpt = VisionGPT(model, system_message)
    return visiongpt

import os
from dotenv import load_dotenv, find_dotenv

from src.utils import read_json
from src.visiongpt import VisionGPT
from src.models import OpenAIModel

load_dotenv(find_dotenv())
model = OpenAIModel(
    api_key=os.getenv("OPENAI_KEY"),
    temperature=int(os.getenv("OPENAI_TEMPERATURE")),
    max_tokens=int(os.getenv("OPENAI_TOKENS")),
)
system_message = read_json("system.json")
visiongpt = VisionGPT(model, system_message)


def run():
    print(str(visiongpt))
    print(str(model))
    print("")

    image_url = "https://ams3.digitaloceanspaces.com/graffica/2022/12/Adidas-Logo-1971-1024x576.jpeg"
    role, response = visiongpt.recognition_image(image_url)
    print(f"Role: {role}, Response: {response}")


if __name__ == "__main__":
    run()

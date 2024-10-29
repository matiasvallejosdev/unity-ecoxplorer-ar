from typing import Dict, Any

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.error_handler import error_response

def parse_and_validate(event: Dict[str, Any]) -> str:
    body = load_body_from_event(event)
    image_url = body.get("image_url")
    if not image_url:
        raise ValueError("Image URL is required.")
    return image_url

@error_handling_decorator
def lambda_handler(event, context):
    parse_and_validate(event)
    
    response = {
        "primary_subject": "Impact of climate change on natural disasters",
        "environment_type": "Varied: Forest, Arid land, Ocean",
        "climate_indicators": [
            "Wildfire",
            "Drought",
            "Hurricane"
        ],
        "biodiversity": {
            "flora": [
                "Tree affected by fire"
            ],
            "fauna": [
                "None detected"
            ]
        },
        "human_elements": [
            "None detected"
        ],
        "weather_conditions": "Extreme weather events depicted",
        "color_palette": [
            "Orange",
            "Brown",
            "White",
            "Blue"
        ],
        "emotional_tone": "Urgent and alarming",
        "educational_themes": [
            "Causes and effects of climate change",
            "Impact of wildfires, droughts, and hurricanes",
            "Importance of environmental protection"
        ],
        "storytelling_elements": [
            "A tree trying to survive a forest fire",
            "Parched land yearning for rain",
            "The power of a hurricane over the ocean"
        ],
        "confidence_score": 0.98,
        "analysis_notes": "The image effectively captures three distinct impacts of climate change, useful for illustrating interconnected environmental themes. The lack of human elements highlights the natural scale of these events.",
        "image_id": "3d0ba72b-2ad7-42ed-8fe0-f30fd4c682e2",
        "image_url": "https://science.nasa.gov/wp-content/uploads/2023/11/Effects_page_triptych.jpeg?w=4096&format=jpeg",
        "date_created": "2024-10-19T18:44:08.546694"
    }
    return success_response(response)

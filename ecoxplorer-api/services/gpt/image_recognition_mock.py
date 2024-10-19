from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.error_handler import error_response


@error_handling_decorator
def lambda_handler(event, context):
    image_url = load_body_from_event(event).get("image_url", None)
    if image_url is None:
        return error_response("Missing image_url parameter.")
    
    # Mock response from image recognition (testing purposes)
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

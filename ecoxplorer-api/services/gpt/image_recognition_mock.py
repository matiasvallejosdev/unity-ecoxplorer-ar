from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response


@error_handling_decorator
def lambda_handler(event, context):
    response = {
        "primary_subject": "River meandering through a dense forest",
        "environment_type": "Wetland, riverine forest",
        "climate_indicators": [
            "Lush, dense vegetation indicates healthy ecosystem"
        ],
        "biodiversity": {
            "flora": [
                "Mangroves, tropical forest plants"
            ],
            "fauna": []
        },
        "human_elements": [
            "Small boat navigating the river"
        ],
        "weather_conditions": "Clear sky with good visibility",
        "color_palette": [
            "Green, blue, brown"
        ],
        "emotional_tone": "Tranquil and serene",
        "educational_themes": [
            "Importance of wetland ecosystems",
            "River navigation", 
            "Biodiversity in riparian zones"
        ],
        "storytelling_elements": [
            "Journey through a river ecosystem",
            "Human interaction with natural habitats"
        ],
        "analysis_notes": "The image highlights the critical role of mangroves in protecting coastal environments.",
        "image_id": "b82b7f83-0bcf-4aad-a45f-b4ca343451d0",
        "image_url": "https://plus.unsplash.com/premium_photo-1664300792059-863ccfe55932?fm=jpg&q=60&w=3000&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8Ym9zcXVlfGVufDB8fDB8fHww",
        "date_created": "2024-11-02T22:20:32.054848"
    }
    return success_response(response)

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.error_handler import error_response


@error_handling_decorator
def lambda_handler(event, context):
    image_url = load_body_from_event(event).get("image_url", None)
    if image_url is None:
        return error_response("Missing image_url parameter.")

    response = {
        "image_id": "image_001",
        "primary_subject": "Large pile of accumulated waste",
        "environment_type": "landfill",
        "climate_indicators": [
            "Accumulation of non-biodegradable plastic waste",
            "Potential soil and air pollution",
        ],
        "biodiversity": {"flora": ["None detected"], "fauna": ["None detected"]},
        "human_elements": [
            "Piles of various types of waste including plastics, paper, and metal items"
        ],
        "weather_conditions": "Clear sky with no visible weather phenomena",
        "color_palette": [
            "Varied colors due to different types of waste compositions",
            "Blue sky",
        ],
        "emotional_tone": "Overwhelming and alarming",
        "educational_themes": [
            "Waste management",
            "Recycling importance",
            "Impact of pollution on the environment",
            "Human responsibility towards environmental cleanliness",
            "Hazards of non-biodegradable materials",
        ],
        "storytelling_elements": [
            "The journey of a plastic bottle from use to disposal",
            "The impact of improper waste disposal on soil and air",
            "The role of recycling in reducing landfill waste",
            "The story of a landfill worker sorting through waste",
        ],
        "confidence_score": 0.97,
        "analysis_notes": "The image is clear and the elements are easily distinguishable. The focus on waste management can spark significant educational conversations among young learners.",
        "image_url": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRLyX0La3Jqwnu6v9fBGPNxfosVwMzD4HbePA&s",
    }
    return success_response(response)

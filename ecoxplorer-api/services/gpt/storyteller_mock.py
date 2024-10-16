import random
from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.error_handler import error_response


@error_handling_decorator
def lambda_handler(event, context):
    body = load_body_from_event(event)
    language = body.get("language", "en")
    
    mock_story = {
        "image_id": "climate_extremes_01",
        "language": "es",
        "narrator": "clever fox",
        "primary_subject": "Impactos del Cambio Climático",
        "climate_concepts": [
            "Impacto del cambio climático en los patrones climáticos",
            "Sequía y sus consecuencias",
            "Desastres naturales relacionados con el calentamiento global"
        ],
        "title": "Extremos Climáticos y sus Consecuencias",
        "story": "En un mundo donde los extremos climáticos son cada vez más comunes, Zorro Sabio corre velozmente para observar los diversos paisajes vulnerables al cambio climático. Se detiene cerca de un árbol que lucha por sobrevivir en medio de un incendio forestal, sus hojas chamuscadas por el fuego implacable. '¡Observad!', dice Zorro Sabio, 'estos incendios son cada vez más frecuentes debido al calentamiento global.' A continuación, muestra la tierra agrietada de una región árida, señalando que la sequía es un signo alarmante de los cambios que experimenta el planeta. Finalmente, mira hacia el mar, donde se forma el ojo de un huracán poderoso que se aproxima. 'Las tormentas más intensas son parte de esta nueva realidad', explica, 'y es crucial entender cómo nuestras acciones impactan el clima.' Mientras la mirada de Zorro Sabio se encuentra con la de los curiosos habitantes del bosque, añade, 'Debemos trabajar juntos para proteger nuestro hogar.'",
        "call_to_action": "Involúcrate en organizaciones que luchan contra el cambio climático e informate sobre cómo puedes reducir tu huella de carbono.",
        "json_analysis": "{\"image_id\": \"climate_extremes_01\", \"primary_subject\": \"Climate Change Impacts\", \"environment_type\": \"Diverse: Wildfire, Arid Lands, Ocean\", \"climate_indicators\": [\"Wildfire\", \"Drought\", \"Hurricane\"], \"biodiversity\": {\"flora\": [\"Trees affected by fire\"], \"fauna\": [\"None detected\"]}, \"human_elements\": [\"None detected\"], \"weather_conditions\": \"Severe with fire, extreme dryness, and storm conditions\", \"color_palette\": [\"Orange\", \"Brown\", \"Blue\", \"White\"], \"emotional_tone\": \"Dramatic and threatening\", \"educational_themes\": [\"Impact of climate change on weather patterns\", \"Drought and its consequences\", \"Natural disasters related to global warming\"], \"storytelling_elements\": [\"A tree struggling to survive a wildfire\", \"The cracked earth illustrating severe drought\", \"The swirling eye of a hurricane approaching land\"], \"confidence_score\": 0.98, \"analysis_notes\": \"The composite image effectively illustrates diverse and severe impacts of climate change, providing distinct and engaging visual examples.\", \"image_url\": \"https://science.nasa.gov/wp-content/uploads/2023/11/Effects_page_triptych.jpeg?w=4096&format=jpeg\"}",
        "audio_url": "https://ecoxplorer-dev.s3.us-east-1.amazonaws.com/storyteller/tts_audio_1729093929.mp3",
        "json_url": "https://ecoxplorer-dev.s3.us-east-1.amazonaws.com/storyteller/response_1729093929.json"
    }

    return success_response(mock_story)

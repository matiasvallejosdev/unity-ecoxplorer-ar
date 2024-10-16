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
        "language": language,
        "narrator": "clever fox",
        "primary_subject": "Impacto del cambio climático",
        "climate_concepts": [
            "Patrones climáticos extremos",
            "Consecuencias de la sequía",
            "Desastres naturales y calentamiento global"
        ],
        "title": "Secretos del Clima Extremo",
        "story": "Entre el humo y el calor de un día nublado, Felix el zorro astuto miraba las tierras áridas a lo lejos. 'Queridos amigos,' murmuró mientras caminaba cautelosamente entre los árboles chamuscados, 'aquí está el resultado de un planeta en transformación acelerada.' Con cuidado, señaló las cicatrices del suelo agrietado y la furia silenciosa en el aire. 'Estos problemas no están lejos,' explicó suavemente, 'sino más cerca de lo que imaginamos.' Describió cómo, en otro rincón, el giro de un huracán se perfilaba, presagiando cambios drásticos. 'Estos fenómenos son reflejos de un mundo que clama atención,' dijo mientras evitaba restos de un incendio reciente. Ante su mirada sagaz, no había criaturas allí, sólo un relato urgente a compartir. 'Cada acción cuenta,' concluyó con un brillo en sus ojos, 'transformemos esta atmósfera de desafío en una de esperanza y acción.'",
        "call_to_action": "Únete a campañas de reforestación o apoya proyectos para mitigar el impacto climático en tu región.",
        "json_analysis": "{\"image_id\": \"climate_extremes_01\", \"primary_subject\": \"Climate Change Impacts\", \"environment_type\": \"Diverse: Wildfire, Arid Lands, Ocean\", \"climate_indicators\": [\"Wildfire\", \"Drought\", \"Hurricane\"], \"biodiversity\": {\"flora\": [\"Trees affected by fire\"], \"fauna\": [\"None detected\"]}, \"human_elements\": [\"None detected\"], \"weather_conditions\": \"Severe with fire, extreme dryness, and storm conditions\", \"color_palette\": [\"Orange\", \"Brown\", \"Blue\", \"White\"], \"emotional_tone\": \"Dramatic and threatening\", \"educational_themes\": [\"Impact of climate change on weather patterns\", \"Drought and its consequences\", \"Natural disasters related to global warming\"], \"storytelling_elements\": [\"A tree struggling to survive a wildfire\", \"The cracked earth illustrating severe drought\", \"The swirling eye of a hurricane approaching land\"], \"confidence_score\": 0.98, \"analysis_notes\": \"The composite image effectively illustrates diverse and severe impacts of climate change, providing distinct and engaging visual examples.\", \"image_url\": \"https://science.nasa.gov/wp-content/uploads/2023/11/Effects_page_triptych.jpeg?w=4096&format=jpeg\"}",
        "audio_url": "https://ecoexplorerar-dev.s3.us-east-2.amazonaws.com/storyteller/tts_audio_1729035305.mp3",
        "json_url": "https://ecoexplorerar-dev.s3.us-east-2.amazonaws.com/storyteller/response_1729035305.json"
    }

    return success_response(mock_story)

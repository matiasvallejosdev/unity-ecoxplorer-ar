from typing import Dict, Tuple

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.parser import extract_json_from_response


def parse_and_validate(event: Dict) -> Tuple[str, str]:
    body = load_body_from_event(event)
    topic = body.get("topic")
    if not topic:
        raise ValueError(
            "Topic must be included in the body to generate the level story."
        )
    language = body.get("language", "en")
    return topic, language


@error_handling_decorator
def lambda_handler(event, context):
    parse_and_validate(event)

    response = {
        "language": "es",
        "narrator": "curious squirrel",
        "primary_subject": "energía renovable",
        "guidance": [
            "Busca un panel solar.",
            "Encuentra un aerogenerador.",
            "Localiza una presa hidroeléctrica.",
            "Captura un símbolo de energía verde.",
        ],
        "title": "Aventura Energética de Ardilla",
        "story_introduction": "¡Hola! Soy Ardilla Curiosa. Vamos en una misión para aprender sobre energías limpias y cómo pueden ayudar a proteger nuestro planeta. ¿Estás listo para usar tu tablet como ventana mágica y descubrir cómo capturar la energía del sol, del viento y del agua?",
        "story_end": "¡Increíble! Has desbloqueado el poder de la naturaleza al capturar estas fuentes de energía renovable. Con estas ideas podemos hacer del mundo un lugar más limpio y verde. ¡Gracias por tu ayuda, valiente explorador!",
    }
    return success_response(response)

from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response
from common.utils.lambda_utils import load_body_from_event
from common.utils.error_handler import error_response


@error_handling_decorator
def lambda_handler(event, context):
    body = load_body_from_event(event)
    image_analysis = body.get("image_analysis")
    language = body.get("language", "en")

    if not image_analysis or not language:
        raise ValueError("Image analysis and language are required.")

    mock_story = {
        "language": "es",
        "narrator": "zorro astuto",
        "primary_subject": "El impacto del cambio climático en los desastres naturales",
        "climate_concepts": [
            "Causas y efectos del cambio climático",
            "Impacto de incendios forestales, sequías y huracanes",
            "Importancia de la protección ambiental"
        ],
        "title": "La Voz del Zorro en Tiempos de Cambio",
        "story": "En un rincón del vasto bosque, donde los árboles susurraban historias del pasado, vivía un zorro astuto llamado Zorro. Con su pelaje brillante y su curiosidad inquebrantable, Zorro se acercó a una pequeña comunidad de conejos que vivían al borde del bosque. 'Mis queridos amigos,' comenzó Zorro, 'he venido a hablarles sobre el cambio que está transformando nuestro hogar.' Los conejos escucharon atentamente mientras Zorro continuaba, 'El cambio climático está aquí, y junto con él, la furia de la naturaleza se intensifica.' Señaló un árbol chamuscado, aún de pie pero tocado por el fuego devastador. 'Los incendios forestales no solo consumen árboles, sino también el hogar de muchos,' explicó Zorro, sus ojos reflejando la gravedad de la situación. 'Y no son solo los incendios. Las tierras áridas claman por agua, implorando una lluvia que parece nunca llegar,' dijo mientras dirigía su mirada a la tierra agrietada bajo ellos. 'El agua, fuente de vida, se convierte en un recurso más preciado con cada día que pasa.' Los conejos entornaron los ojos al imaginarse la sequía interminable. Zorro entonces alzó la voz un poco más, 'Sobre el océano, los huracanes despliegan su poder, azotando con furia que desafía la tranquilidad de las olas. Cada elemento está en juego, cada átomo se cuestiona su propósito.' Mientras Zorro hablaba, los conejos comenzaron a vislumbrar la interconexión de su mundo, desde los bosques hasta los desiertos y los vastos océanos. 'Pero no todo está perdido,' prometió Zorro, 'Si nosotros, con nuestras pequeñas acciones, podemos unirnos para proteger lo que queda, tal vez haya esperanza.' Los conejos asintieron, reconociendo el llamado a la acción. Ellos, pequeños y numerosos, podían hacer una gran diferencia. 'Recuerden siempre,' concluyó Zorro con un tono alentador, 'La naturaleza ha hablado, y nosotros debemos escuchar. Juntos, podemos devolver el equilibrio que tanto anhelamos.' El viento susurraba suavemente entre las hojas, y en ese momento, los conejos comprendieron que el futuro de su hogar dependía de ellos.",
        "call_to_action": "Participa en la reforestación de tu comunidad o apoya organizaciones que protegen el medio ambiente.",
        "confidence_score": 0.98,
        "analysis_notes": "The image effectively captures three distinct impacts of climate change, useful for illustrating interconnected environmental themes. The lack of human elements highlights the natural scale of these events.",
        "image_id": "3d0ba72b-2ad7-42ed-8fe0-f30fd4c682e2",
        "image_url": "https://science.nasa.gov/wp-content/uploads/2023/11/Effects_page_triptych.jpeg?w=4096&format=jpeg",
        "date_created": "2024-10-19T19:08:32.124292",
        "story_id": "76db0bd0-df4a-4413-895e-c85b922d5ae2",
        "image_recognition_output": '{"primary_subject": "Impact of climate change on natural disasters", "environment_type": "Varied: Forest, Arid land, Ocean", "climate_indicators": ["Wildfire", "Drought", "Hurricane"], "biodiversity": {"flora": ["Tree affected by fire"], "fauna": ["None detected"]}, "human_elements": ["None detected"], "weather_conditions": "Extreme weather events depicted", "color_palette": ["Orange", "Brown", "White", "Blue"], "emotional_tone": "Urgent and alarming", "educational_themes": ["Causes and effects of climate change", "Impact of wildfires, droughts, and hurricanes", "Importance of environmental protection"], "storytelling_elements": ["A tree trying to survive a forest fire", "Parched land yearning for rain", "The power of a hurricane over the ocean"], "confidence_score": 0.98, "analysis_notes": "The image effectively captures three distinct impacts of climate change, useful for illustrating interconnected environmental themes. The lack of human elements highlights the natural scale of these events.", "image_id": "3d0ba72b-2ad7-42ed-8fe0-f30fd4c682e2", "image_url": "https://science.nasa.gov/wp-content/uploads/2023/11/Effects_page_triptych.jpeg?w=4096&format=jpeg", "date_created": "2024-10-19T18:44:08.546694"}',
    }

    return success_response(mock_story)

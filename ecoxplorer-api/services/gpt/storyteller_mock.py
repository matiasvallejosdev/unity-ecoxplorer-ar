from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response


@error_handling_decorator
def lambda_handler(event, context):
    response = {
        "language": "es",
        "narrator": "Wise Owl",
        "primary_subject": "Importancia de los ecosistemas de humedales",
        "climate_concepts": [
            "ecosistema saludable",
            "protección costera", 
            "biodiversidad"
        ],
        "title": "Aventura por el Río Sereno",
        "story": "En un rincón mágico de la selva tropical de manglares, donde los colores verdes y azules se fusionan, un río serpentea como si estuviera contando secretos antiguos. Sobre sus aguas tranquilas, navega una pequeña embarcación con vistosos remos que rompen apenas la calma del entorno.\n\nDesde la altura de la copa de un árbol robusto, el Sabio Búho observa con ojos atentos. 'Bienvenidos amigos aventureros', dice el Búho con una voz suave y reconfortante. 'Les contaré por qué este lugar es tan especial'.\n\nA medida que el bote se desliza por el agua, el Sabio Búho explica cómo estos manglares actúan como guardianes de la costa. 'Las raíces de los manglares no solo sostienen a los árboles firmemente, sino que también protegen a las costas de la erosión y los fuertes vientos', dice con sabiduría.\n\nMientras el bote avanza, los pequeños exploradores se maravillan al ver la variedad de plantas que adornan las orillas del río. 'Aquí', señala el Sabio Búho, 'cada planta contribuye a un ecosistema saludable. Estas plantas ayudan a filtrar el agua y proporcionan un hogar a diversas especies de animales'.\n\nAunque no se ven muchos animales a simple vista, el Búho les asegura que la biodiverisdad es abundante. 'Aunque no los veamos, aquí viven muchas aves, peces, y pequeños mamíferos. Todos ellos dependen de este ecosistema', afirma el Sabio Búho.\n\nEl cielo despejado refleja una paz serena, pero el Sabio Búho advierte que para mantener esta tranquilidad, debemos proteger estos lugares. 'El cambio climático y la deforestación amenazan nuestro hogar natural. Debemos fomentar la reforestación y preservar estos hábitats preciosos para que continúen protegiéndonos', concluye.\n\nAsí, los aventureros se comprometen a cuidar su entorno natural, inspirados por la sabiduría del Sabio Búho, con la promesa de preservar su casa común.",
        "call_to_action": {
            "suggestion": "Planta un árbol para ayudar a conservar los ecosistemas locales y participa en acciones comunitarias de reforestación.",
            "age_group": "8-12 años",
            "difficulty_level": "Fácil"
        },
        "story_id": "4b8f30b5-bc66-4feb-a008-f8f6b3733062",
        "image_recognition_output": "{\"primary_subject\": \"River meandering through a dense forest\", \"environment_type\": \"Wetland, riverine forest\", \"climate_indicators\": [\"Lush, dense vegetation indicates healthy ecosystem\"], \"biodiversity\": {\"flora\": [\"Mangroves, tropical forest plants\"], \"fauna\": []}, \"human_elements\": [\"Small boat navigating the river\"], \"weather_conditions\": \"Clear sky with good visibility\", \"color_palette\": [\"Green, blue, brown\"], \"emotional_tone\": \"Tranquil and serene\", \"educational_themes\": [\"Importance of wetland ecosystems\", \"River navigation\", \"Biodiversity in riparian zones\"], \"storytelling_elements\": [\"Journey through a river ecosystem\", \"Human interaction with natural habitats\"], \"analysis_notes\": \"The image highlights the critical role of mangroves in protecting coastal environments.\", \"image_id\": \"b82b7f83-0bcf-4aad-a45f-b4ca343451d0\", \"image_url\": \"https://plus.unsplash.com/premium_photo-1664300792059-863ccfe55932?fm=jpg&q=60&w=3000&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8Ym9zcXVlfGVufDB8fDB8fHww\", \"date_created\": \"2024-11-02T22:20:32.054848\"}",
        "image_id": "b82b7f83-0bcf-4aad-a45f-b4ca343451d0",
        "image_url": "https://plus.unsplash.com/premium_photo-1664300792059-863ccfe55932?fm=jpg&q=60&w=3000&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8Ym9zcXVlfGVufDB8fDB8fHww",
        "date_created": "2024-11-02T22:27:40.619541"
    }
    return success_response(response)

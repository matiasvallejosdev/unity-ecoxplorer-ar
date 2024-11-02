from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response


@error_handling_decorator
def lambda_handler(event, context):
    response = {
        "language": "es",
        "narrator": "Búho Sabio",
        "primary_subject": "Conservación de bosques y biodiversidad",
        "guidance": [
            "Encuentra el árbol más grande",
            "Haz zoom para ver detalles del tronco", 
            "Descubre especies nativas del bosque",
            "Ubica áreas de reforestación",
            "Compara el dosel de diferentes árboles"
        ],
        "title": "Guardianes del Bosque",
        "story_introduction": "Únete al Búho Sabio en una aventura por el bosque. Aprende sobre la importancia de los árboles, los desafíos del desmonte y cómo puedes ayudar en la conservación y reforestación.",
        "story_end": "Con cada árbol protegido, damos un paso hacia un mundo más verde. Gracias a Búho Sabio, ahora eres un guardián del bosque, listo para preservar nuestra biodiversidad para las generaciones futuras."
    }
    return success_response(response)

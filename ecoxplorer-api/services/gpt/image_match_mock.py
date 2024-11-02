from common.utils.lambda_decorators import error_handling_decorator
from common.utils.response_utils import success_response


@error_handling_decorator
def lambda_handler(event, context):
    response = {
        "match": True,
        "confidence": 0.95,
        "elements_found": [
            "forest",
            "trees", 
            "canopy",
            "habitat",
            "ecosystem"
        ],
        "topic_relevance": "The image depicts a lush forest with dense tree canopy, contributing to habitat and biodiversity topics."
    }
    return success_response(response)

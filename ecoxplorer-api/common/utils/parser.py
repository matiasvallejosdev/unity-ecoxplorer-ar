import json
import re
from typing import Dict, Any


def extract_json_from_response(content: str) -> Dict[str, Any]:
    try:
        return json.loads(content)
    except json.JSONDecodeError:
        json_match = re.search(r"```json\n(.*?)\n```", content, re.DOTALL)
        if json_match:
            json_content = json_match.group(1)
            try:
                return json.loads(json_content)
            except json.JSONDecodeError as e:
                raise ValueError("Invalid JSON in the response.")
        else:
            raise ValueError("No valid JSON found in the response.")

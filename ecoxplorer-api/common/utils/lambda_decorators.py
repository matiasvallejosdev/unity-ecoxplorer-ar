import logging

from common.utils.error_handler import (
    error_response,
    internal_server_error,
    not_found_error,
)

logger = logging.getLogger()
logger.setLevel(logging.INFO)


def error_handling_decorator(func):
    """Error handling decorator for Lambda functions.

    Args:
        func (function): Function to be wrapped.
    """

    def wrapper(event, context):
        try:
            logger.info("Handling event: %s", event)
            return func(event, context)
        except KeyError as e:
            logger.error("KeyError: %s", str(e))
            return error_response(str(e), 400)
        except TypeError as e:
            logger.error("TypeError: %s", str(e))
            return error_response(str(e), 400)
        except ValueError as e:
            logger.error("ValueError: %s", str(e))
            return error_response(str(e), 400)
        except FileNotFoundError as e:
            logger.error("FileNotFoundError: %s", str(e))
            return not_found_error(e)
        except Exception as e:
            logger.exception("Unhandled exception: %s", str(e))
            return internal_server_error(e)

    return wrapper

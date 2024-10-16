import pytest


@pytest.fixture
def sample_lambda_function():
    def function(event, context):
        return {"statusCode": 200, "body": "Success"}

    return function


@pytest.fixture
def error_handling_decorator():
    from common.utils.lambda_decorators import error_handling_decorator

    return error_handling_decorator


@pytest.fixture
def decorated_function(sample_lambda_function, error_handling_decorator):
    return error_handling_decorator(sample_lambda_function)

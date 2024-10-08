import pytest
from common.utils.lambda_utils import (
    load_query_parameter_from_event,
    load_body_from_event,
)

pytestmark = [pytest.mark.unit]


def test_load_query_parameter_from_event_():
    event = {"queryStringParameters": {"key": "value"}}
    result = load_query_parameter_from_event(event)
    assert result == {"key": "value"}


def test_load_query_parameter_from_event_empty():
    event = {}
    result = load_query_parameter_from_event(event)
    assert result == {}


def test_load_query_parameter_from_event_no_query_string_parameters():
    event = {"key": "value"}
    result = load_query_parameter_from_event(event)
    assert result == {}


def test_load_body_from_event_valid():
    event = {"body": '{"key": "value"}'}
    result = load_body_from_event(event)
    assert result == {"key": "value"}


def test_load_body_from_event_invalid():
    event = {"body": "invalid"}
    result = load_body_from_event(event)
    assert result == {}


def test_load_body_from_event_empty():
    event = {}
    result = load_body_from_event(event)
    assert result == {}


def test_load_body_from_event_no_body():
    event = {"key": "value"}
    result = load_body_from_event(event)
    assert result == {}

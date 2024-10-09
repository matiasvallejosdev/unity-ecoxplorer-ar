import pytest
import json

pytestmark = [pytest.mark.ai]


def test_initialize_visiongpt(visiongpt):
    visiongpt._initialize()
    assert len(visiongpt.messages) > 0


def test_recognition_image(visiongpt):
    image_url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRLyX0La3Jqwnu6v9fBGPNxfosVwMzD4HbePA&s"
    response = visiongpt.recognize_image(image_url)
    print(response)
    assert response["status"] == "success"


def test_failed_recognition_image(visiongpt):
    image_url = ""
    with pytest.raises(ValueError):
        visiongpt.recognize_image(image_url)


def test_failed_recognition_image(visiongpt):
    image_url = "https://example.com/invalid.jpg"
    response = visiongpt.recognize_image(image_url)
    assert response["status"] == "error"


def test_recognition_image_format(visiongpt):
    image_url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRLyX0La3Jqwnu6v9fBGPNxfosVwMzD4HbePA&s"
    response = visiongpt.recognize_image(image_url)
    assert response["status"] == "success"
    assert response["role"] == "assistant"

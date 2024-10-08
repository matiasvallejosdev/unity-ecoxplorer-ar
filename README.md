# Ecoxplorer AR: Game Concept

## Overview
Ecoxplorer AR is an augmented reality game that encourages players to explore their environment. Players scan real-world objects related to nature or the environment, triggering short, engaging stories narrated by AI-generated characters. These stories provide educational content about the environment in an entertaining way.

## Key Components

1. **Object Recognition**
   - Utilizes existing AR technology to recognize environmental objects (e.g., trees, plants, recycling bins).

2. **AI Storytelling Agent**
   - Generates short, educational stories about recognized objects.
   - Stories include facts, environmental impact, or imaginative tales with educational elements.

3. **AI Voice Actor**
   - Converts generated story text to speech for a more immersive experience.

4. **Character System**
   - Features simple character models (e.g., wise owl, curious squirrel, adventurous leaf) that appear in AR to narrate stories.
   - AI voice actor provides different voices for each character.

5. **Progress Tracking**
   - Tracks objects discovered and stories heard.
   - Unlocks new character narrators as the player progresses.

## Implementation with Three Endpoints

1. **Image Recognition Endpoint**
   - Input: Image from AR scan
   - Output: Identified object

2. **AI Game Storytelling Agent Endpoint**
   - Input: Identified object
   - Output: Short, educational story about the object

3. **AI Voice Actor Endpoint**
   - Input: Generated story text
   - Output: Audio file of narrated story

## Game Flow

1. Player scans an environmental object using their device.
2. The image is sent to the Image Recognition Endpoint.
3. The identified object is passed to the AI Game Storytelling Agent Endpoint.
4. The generated story is sent to the AI Voice Actor Endpoint.
5. The game displays an AR character who narrates the story using the generated audio.

## Additional Features

- **Story Collection**: Players can revisit unlocked stories.
- **Character Customization**: Players can choose their favorite narrator.
- **Environmental Challenges**: Occasional suggestions for objects to find, encouraging exploration.

## Benefits

- Maintains educational focus while enhancing engagement through storytelling.
- Interactive without relying on quizzes.
- AI-generated content keeps the experience fresh and interesting.
- Modular three-endpoint structure allows for future expansions.
- Leverages existing AR technology while adding new, engaging elements.
# 🌱 EcoXplorer AR Game

[![GitHub top language](https://img.shields.io/github/languages/top/matiasvallejosdev/unity-ecoxplorer-ar?color=1081c2)](https://github.com/matiasvallejosdev/unity-ecoxplorer-ar/search?l=c%23)
![License](https://img.shields.io/github/license/matiasvallejosdev/unity-ecoxplorer-ar?label=license&logo=github&color=f80&logoColor=fff)
![Forks](https://img.shields.io/github/forks/matiasvallejosdev/unity-ecoxplorer-ar.svg)
![Stars](https://img.shields.io/github/stars/matiasvallejosdev/unity-ecoxplorer-ar.svg)
![Watchers](https://img.shields.io/github/watchers/matiasvallejosdev/unity-ecoxplorer-ar.svg)
[![IES21](https://img.shields.io/badge/Academic%20Supervision-IES21-blue)](https://www.ies21.edu.ar/)
[![Manos Verdes](https://img.shields.io/badge/In%20Collaboration%20With-Fundación%20Manos%20Verdes-green)](https://manos-verdes.org/)

## 📘 Introduction

EcoXplorer is an innovative educational game that combines Augmented Reality (AR) and Artificial Intelligence to teach about climate change and the environment. Built on a microservices architecture using AWS Lambda functions and API Gateway, the project leverages Unity's powerful game engine with AR Foundation for immersive experiences. The backend infrastructure utilizes Python for serverless computing and integrates multiple AI models through AWS Bedrock and OpenAI services.

## ✨ Key Features

- **Immersive AR Experience**: Real-time environment exploration and object recognition
- **AI-Powered Storytelling**: Dynamic and adaptive narrative generation
- **Interactive Learning**: Engaging educational content about climate change
- **Voice-Enabled Characters**: Text-to-speech integration for character dialogues

## 🤖 Multi-Agent System Architecture

Our system implements a sophisticated multi-agent architecture where each agent has a specific role in creating an engaging educational experience:

1. Image Analysis Agent (🔍 Vision Agent)
- Handles AR detection and environment recognition
- Processes visual images input
- Identifies environmental objects and contexts

2. Narrative Generation Agent (📖 Story Agent)
- Creates dynamic, context-aware stories
- Adapts narrative based on detected environment
- Ensures educational content integration

3. Session Agent (🎭 Main Story)
- Generates the main storyline and guidelines for the user session
- Creates a coherent narrative framework for the AR exploration
- Ensures story continuity across different scanned elements
- Adapts the main plot based on player discoveries

4. Match Agent (🎯 Probability Agent)
- Calculates probability scores between scanned images and educational topics
- Prevents arbitrary scanning by validating image relevance
- Filters and validates AR interactions based on contextual appropriateness
- Ensures meaningful connections between real objects and learning content

5. TTS Agent (🗣️ Voice Agent)
- Generates character voices
- Manages audio content delivery
- Enhances immersion through voice interaction

## 🛠️ Technology Stack

- AWS Lambda & API Gateway
- AWS S3 (Image and Voice Storage)
- AWS Bedrock & Llama 3.5
- OpenAI GPT-4
- Serverless Framework
- Python Microservices
- Unity Engine
- AR Foundation (iOS/Android)

## 🤖 Prompt Engineering

The multi-agent system leverages Chain of Thought (CoT) reasoning to create coherent and contextually appropriate interactions. This approach allows each agent to process information systematically while maintaining communication and coordination with other agents in the ecosystem.

The prompting strategy incorporates environmental parameters and user interaction history to establish context, followed by agent-specific objectives and communication protocols. Through step-by-step logical progression and cross-agent validation, the system ensures consistent and meaningful interactions throughout the gaming experience.

## 🚀 Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/matiasvallejosdev/unity-ecoxplorer-ar.git
   ```
2. Configure AWS credentials and services
3. Set up required API keys for AI services
4. Configure environment variables
5. Deploy using Serverless Framework:

   ```bash
   serverless deploy
   ```

## 💡 Usage

The game enables users to:

1. Explore their physical environment using AR
2. Discover AI-generated interactive stories
3. Learn environmental concepts dynamically
4. Interact with adaptive narratives

## 🎓 Academic Context

This project was developed as part of a degree thesis at IES21 University College, in collaboration with Fundación Manos Verdes. The initiative demonstrates the practical application of AI and AR in environmental education, combining academic research with real-world environmental awareness goals. While it's a functional demonstration, there are aspects that could be optimized for a production version.

## 🤝 Contributing

This is a demonstrative academic project. Suggestions and feedback are welcome for future improvements.

## 📞 Contact

- Name: Matias Vallejos
- 🌐 [matiasvallejos.com](https://matiasvallejos.com/)

## 📄 License

This project is under the [MIT License](LICENSE).

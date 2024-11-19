# 🎮 EcoXplorer Unity Client

## 📘 Overview
Unity client for EcoXplorer AR educational game. This application handles the AR interaction, user interface, and communication with the AI microservices backend.

## 🔧 Prerequisites
- Unity 2022.3.8f1 or later
- AR Foundation 5.0+
- Compatible AR device:
  - iOS: ARKit compatible device (iOS 11+)
  - Android: ARCore compatible device

## 📦 Dependencies
- AR Foundation
- AR Subsystems
- XR Plugin Management
- TextMeshPro
- REST Client (for API communication)
- Newtonsoft JSON
- R3 Reactive (for reactive programming)

## 🚀 Setup Instructions
1. Clone the repository
2. Open project in Unity 2022.3.8f1
3. Install required packages through Package Manager
4. Configure API endpoints in `Assets/EcoXplorer/Scripts/API/Config.cs`
5. Set up your development environment:
   - iOS: Configure Xcode project settings
   - Android: Set up Gradle build configuration

## ✨ Features
- Real-time AR environment scanning
- Object detection and tracking
- Interactive UI elements
- Audio playback for character voices
- API integration with AI backend
- Session management
- Story progression system

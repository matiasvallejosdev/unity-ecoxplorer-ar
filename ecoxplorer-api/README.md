# 🚀 EcoXplorer API

## 📘 Overview
Serverless microservices architecture powering the EcoXplorer AR educational game. Built with Python and AWS Lambda, this API handles AI processing, image analysis, and story generation through a sophisticated multi-agent system.

## 🛠️ Technology Stack
- AWS Lambda Functions (ARM64)
- AWS API Gateway
- AWS S3 (Image and voice storage)
- AWS Bedrock
- OpenAI GPT-4
- Serverless Framework
- Python 3.11 (ARM64)

## 🔧 Prerequisites

- Python 3.11 ARM64 architecture
- Node.js 18+ (for Serverless Framework)
- AWS CLI configured
- Serverless Framework installed
- OpenAI API key
- AWS account with appropriate permissions

## 🚀 Setup Instructions

1. Clone the repository
2. Install dependencies:
```bash
# Create virtual environment with specific Python version
python3.11 -m venv venv
source venv/bin/activate  # Linux/macOS
# or
.\venv\Scripts\activate  # Windows

# Install dependencies
pip install -r requirements.txt
npm install -g serverless
```

3. Configure environment variables:
```bash
# .env
OPENAI_API_KEY=your_openai_key
AWS_BEDROCK_REGION=your_region
AWS_S3_BUCKET=your_bucket_name
```

4. Deploy to AWS:
```bash
serverless deploy
```

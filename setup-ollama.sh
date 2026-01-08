#!/bin/bash

echo "=========================================="
echo "Setting up Ollama AI Model"
echo "=========================================="
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "Error: Docker is not running. Please start Docker Desktop and try again."
    exit 1
fi

# Check if Ollama container is running
if ! docker ps | grep -q taskmanager-ollama; then
    echo "Error: Ollama container is not running."
    echo "Please run 'docker-compose up -d ollama' first."
    exit 1
fi

echo "Pulling llama3.2:3b model (this may take a few minutes)..."
echo ""

# Pull the model
docker exec taskmanager-ollama ollama pull llama3.2:3b

if [ $? -eq 0 ]; then
    echo ""
    echo "=========================================="
    echo "Success! Ollama model llama3.2:3b is ready"
    echo "=========================================="
    echo ""
    echo "You can now start using AI features in the application."
    echo "The model will be available at: http://localhost:11434"
else
    echo ""
    echo "Error: Failed to pull Ollama model."
    echo "Please check your internet connection and try again."
    exit 1
fi

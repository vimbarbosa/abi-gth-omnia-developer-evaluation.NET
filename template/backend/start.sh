#!/bin/bash

set -e

echo "🚀 Subindo containers com Docker Compose..."
docker-compose up -d

echo "⏳ Aguardando serviços inicializarem..."
sleep 10

echo "🧱 Aplicando migrations via projeto ORM..."
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM

echo "✅ Migrations aplicadas com sucesso."

echo "🌐 Iniciando aplicação WebApi..."
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi

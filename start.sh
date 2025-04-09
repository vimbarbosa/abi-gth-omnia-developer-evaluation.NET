#!/bin/bash

set -e

echo "🚀 Subindo containers com Docker Compose..."
docker-compose -f template/backend/docker-compose.yml up -d

echo "⏳ Aguardando serviços inicializarem..."
sleep 10

echo "🧱 Aplicando migrations via projeto ORM..."
dotnet ef database update --project template/backend/src/Ambev.DeveloperEvaluation.ORM
echo "✅ Migrations aplicadas com sucesso."


echo "🌐 Iniciando aplicação WebApi..."
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet run --project .

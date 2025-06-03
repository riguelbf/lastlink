#!/bin/bash

if [ "$#" -lt 1 ]; then
  echo "Uso: ./scripts/ef-migration.sh NomeDaMigration [opções]"
  echo "Opções:"
  echo "  --project <caminho>"
  echo "  --startup-project <caminho>"
  echo "  --output-dir <diretório>"
  echo "  --environment <ambiente>"
  exit 1
fi

MIGRATION_NAME=$1
shift

PROJECT=""
STARTUP_PROJECT=""
OUTPUT_DIR=""
ENVIRONMENT=""

while [[ "$#" -gt 0 ]]; do
  case $1 in
    --project) PROJECT="--project $2"; shift ;;
    --startup-project) STARTUP_PROJECT="--startup-project $2"; shift ;;
    --output-dir) OUTPUT_DIR="--output-dir $2"; shift ;;
    --environment) ENVIRONMENT="-- --environment $2"; shift ;;
    *) echo "Opção desconhecida: $1"; exit 1 ;;
  esac
  shift
done

CMD="dotnet ef migrations add $MIGRATION_NAME $PROJECT $STARTUP_PROJECT $OUTPUT_DIR $ENVIRONMENT"
echo "Executando: $CMD"
eval $CMD

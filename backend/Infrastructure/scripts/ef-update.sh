#!/bin/bash

PROJECT=""
STARTUP_PROJECT=""
ENVIRONMENT=""

while [[ "$#" -gt 0 ]]; do
  case $1 in
    --project) PROJECT="--project $2"; shift ;;
    --startup-project) STARTUP_PROJECT="--startup-project $2"; shift ;;
    --environment) ENVIRONMENT="-- --environment $2"; shift ;;
    *) echo "Opção desconhecida: $1"; exit 1 ;;
  esac
  shift
done

CMD="dotnet ef database update $PROJECT $STARTUP_PROJECT $ENVIRONMENT"
echo "Executando: $CMD"
eval $CMD

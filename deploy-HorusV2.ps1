# ====================================
# Deploy HorusV2.API
# ====================================

Write-Host ""
Write-Host " Deploy HorusV2.API" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan
Write-Host ""

# Configuracoes
$IMAGE_NAME = "horusv2_api"
$CONTAINER_NAME = "horusv2_api"
$API_DOCKERFILE = "HorusV2.API/Dockerfile"
$ENV_FILE = "HorusV2.API/.env"
$LOGS_DIR = "HorusV2.API/Logs"
$HOST_PORT = 8282

# Verifica se esta no diretorio correto
if (-Not (Test-Path $API_DOCKERFILE)) {
    Write-Host " Erro: Execute este script na raiz da solucao!" -ForegroundColor Red
    Write-Host "   (onde estao as pastas HorusV2.API, HorusV2.Application, HorusV2.Core)" -ForegroundColor Yellow
    exit 1
}

if (-Not (Test-Path $ENV_FILE)) {
    Write-Host " Erro: Arquivo .env nao encontrado: $ENV_FILE" -ForegroundColor Red
    exit 1
}

# ====================================
# 1. Para e remove container existente
# ====================================
Write-Host " Parando container existente..." -ForegroundColor Yellow
$existing = docker ps -a -q -f name=$CONTAINER_NAME
if ($existing) {
    docker stop $CONTAINER_NAME 2>$null
    docker rm $CONTAINER_NAME 2>$null
    Write-Host "   Container anterior removido" -ForegroundColor Green
} else {
    Write-Host "   Nenhum container anterior encontrado" -ForegroundColor Gray
}

Write-Host ""

# ====================================
# 2. Build da imagem
# ====================================
Write-Host " Build da imagem Docker..." -ForegroundColor Yellow
Write-Host "   Isso pode levar alguns minutos..." -ForegroundColor Gray

docker build -f $API_DOCKERFILE -t "${IMAGE_NAME}:latest" .

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host " Erro no build da imagem!" -ForegroundColor Red
    exit 1
}

Write-Host "   Imagem construida com sucesso" -ForegroundColor Green
Write-Host ""

# ====================================
# 3. Inicia o container
# ====================================
Write-Host " Iniciando container..." -ForegroundColor Green

docker run -d `
  --name $CONTAINER_NAME `
  --restart unless-stopped `
  -p ${HOST_PORT}:8080 `
  --env-file $ENV_FILE `
  -v ${PWD}/${LOGS_DIR}:/app/Logs `
  -v ${PWD}/${ENV_FILE}:/app/.env:ro `
  "${IMAGE_NAME}:latest"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host " Erro ao iniciar container!" -ForegroundColor Red
    exit 1
}

Write-Host "   Container iniciado" -ForegroundColor Green
Write-Host ""

# ====================================
# 4. Aguarda inicializacao
# ====================================
Write-Host " Aguardando inicializacao..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# ====================================
# 5. Verifica status
# ====================================
$status = docker ps -f name=$CONTAINER_NAME --format "{{.Status}}"

if ($status -match "Up") {
    Write-Host ""
    Write-Host " Container rodando com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host " INFORMACOES DO CONTAINER" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan

    $containerInfo = docker inspect $CONTAINER_NAME --format "{{.State.Status}}, {{.State.StartedAt}}"
    Write-Host "Status:       " -NoNewline -ForegroundColor Gray
    Write-Host "Running " -ForegroundColor Green
    Write-Host "Nome:         " -NoNewline -ForegroundColor Gray
    Write-Host $CONTAINER_NAME -ForegroundColor White
    Write-Host "Imagem:       " -NoNewline -ForegroundColor Gray
    Write-Host "${IMAGE_NAME}:latest" -ForegroundColor White
    Write-Host "Logs:         " -NoNewline -ForegroundColor Gray
    Write-Host ".\$LOGS_DIR\" -ForegroundColor White

    Write-Host ""
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host "COMANDOS UTEIS" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Ver logs em tempo real:" -ForegroundColor Yellow
    Write-Host "  docker logs -f $CONTAINER_NAME" -ForegroundColor White
    Write-Host ""
    Write-Host "Ver ultimas 100 linhas:" -ForegroundColor Yellow
    Write-Host "  docker logs --tail 100 $CONTAINER_NAME" -ForegroundColor White
    Write-Host ""
    Write-Host "Parar o container:" -ForegroundColor Yellow
    Write-Host "  docker stop $CONTAINER_NAME" -ForegroundColor White
    Write-Host ""
    Write-Host "Reiniciar o container:" -ForegroundColor Yellow
    Write-Host "  docker restart $CONTAINER_NAME" -ForegroundColor White
    Write-Host ""
    Write-Host "Ver status:" -ForegroundColor Yellow
    Write-Host "  docker ps | Select-String $CONTAINER_NAME" -ForegroundColor White
    Write-Host ""
    Write-Host "Ver uso de recursos:" -ForegroundColor Yellow
    Write-Host "  docker stats $CONTAINER_NAME" -ForegroundColor White
    Write-Host ""
    Write-Host "================================" -ForegroundColor Cyan

    # ====================================
    # 6. Mostra logs iniciais
    # ====================================
    Write-Host ""
    Write-Host "Logs iniciais (ultimas 30 linhas):" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan
    docker logs --tail 30 $CONTAINER_NAME
    Write-Host "================================" -ForegroundColor Cyan

    # ====================================
    # 7. Opcao para seguir logs
    # ====================================
    Write-Host ""
    $response = Read-Host "Deseja ver os logs em tempo real? (s/n)"
    if ($response -eq "s" -or $response -eq "S") {
        Write-Host ""
        Write-Host "Seguindo logs (Ctrl+C para sair)..." -ForegroundColor Cyan
        Write-Host ""
        docker logs -f $CONTAINER_NAME
    }

} else {
    Write-Host ""
    Write-Host "Container nao esta rodando!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Verificando logs de erro..." -ForegroundColor Yellow
    docker logs $CONTAINER_NAME
    exit 1
}

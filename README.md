# HorusV2 API (.NET 8)

API de integração com o Hórus (BNAFAR) para consulta de protocolos e controle de transmissões, com validação básica de sessão via headers, persistência em SQL Server e job diário automatizado.

## Principais Funcionalidades
- Consultar protocolos e inconsistências
- Solicitar transmissões e acompanhar histórico
- Transmissão automática diária em background (00:30)
- Swagger disponível na raiz da aplicação

## Autenticação (Headers obrigatórios)
Todas as rotas de negócio exigem:
- `SIGSM_USER_ID`
- `SIGSM_IBGE_CITY_CODE`

## Endpoints
- `GET /api/protocols`
- `GET /api/protocols/{protocol}/details`
- `GET /api/protocols/{protocol}/inconsistencies`
- `GET /api/streamings`
- `POST /api/streamings`
- `GET /health`
- `GET /worker-status`
- `GET /` (página simples indicando execução)

## Variáveis de Ambiente (.env)
```env
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080

RelationalDatabaseSettings__ConnectionString=Server=HOST;Database=DATABASE;User Id=USER;Password=PASSWORD;MultipleActiveResultSets=True;TrustServerCertificate=True;

HorusIntegrationSettings__IbgeCode=355410
HorusIntegrationSettings__UserId=12
HorusIntegrationSettings__BaseUri=https://servicoshm.saude.gov.br/bnafar
HorusIntegrationSettings__AuthUri=https://servicoshm.saude.gov.br/jwtauth/auth
HorusIntegrationSettings__UserAccess=USUARIO
HorusIntegrationSettings__Password=SENHA

LogSettings__LogTextFilePath=./Logs/ApplicationLogs.log
TZ=America/Sao_Paulo
```

## Observações
- O processamento de transmissão pode ocorrer via solicitação manual (`POST /api/streamings`) ou automaticamente pelo Worker diário.
- O Swagger fica disponível na raiz da aplicação quando em execução.

# BLH Rastreabilidade

Sistema web para Banco de Leite Humano com rastreabilidade ponta a ponta do frasco: doadora, triagem, coleta, caixa termica, rota com GPS/temperatura, recepcao, processamento, qualidade, estoque, distribuicao hospitalar e administracao ao bebe.

## Rodar com Docker Compose

```bash
docker compose up --build
```

Servicos:

- Web: http://localhost:5173
- API Swagger: http://localhost:8080/swagger
- PostgreSQL: localhost:5432, database `blh`, usuario `blh`, senha `blh`

Usuario inicial:

- Email: `admin@blh.local`
- Senha: `Admin@123`

## Rodar backend local

```bash
cd backend/src/Api
dotnet restore
dotnet run
```

Configure PostgreSQL em `backend/src/Api/appsettings.json` ou via variavel `ConnectionStrings__Postgres`.

## Rodar frontend local

```bash
cd frontend
npm install
npm run dev
```

Crie `frontend/.env` a partir de `.env.example` quando quiser apontar para outra API.

## Google Maps

Defina a chave:

```bash
VITE_GOOGLE_MAPS_API_KEY=sua-chave
```

No Docker Compose, informe essa variavel no ambiente antes de subir. Sem chave, a tela ainda mostra filtros, timeline, tabela, grafico e CSV, mas o mapa real fica bloqueado pelo proprio Google Maps.

## Endpoints principais

- `POST /api/auth/login`
- `GET /api/auth/me`
- CRUD: `/api/usuarios`, `/api/instituicoes`, `/api/doadoras`, `/api/receptores`, `/api/caixas-termicas`, `/api/rotas`, `/api/coletas`, `/api/lotes`, `/api/prescricoes`
- Frascos: `POST /api/frascos`, `GET /api/frascos`, `GET /api/frascos/{id}`, `GET /api/frascos/codigo/{codigoUnico}`, `POST /api/frascos/{id}/alterar-status`
- Rastreamento: `POST /api/rastreamento-caixa`, `GET /api/rastreamento-caixa/rota/{rotaId}`, `GET /api/rastreamento-caixa/coleta/{coletaId}`, `GET /api/rastreamento-caixa/distribuicao/{distribuicaoId}`, `GET /api/rastreamento-caixa/caixa/{caixaTermicaId}`
- Rastreabilidade: `/api/rastreabilidade/frasco/{codigoUnico}`, `/api/rastreabilidade/receptor/{receptorId}`, `/api/rastreabilidade/doadora/{doadoraId}`, `/api/rastreabilidade/caixa/{caixaTermicaId}`
- Dashboard: `/api/dashboard/resumo`, `/api/dashboard/coletas`, `/api/dashboard/estoque`, `/api/dashboard/temperaturas`, `/api/dashboard/alertas`

## Payload de temperatura com lat/long

```json
{
  "caixaTermicaId": "66666666-6666-6666-6666-666666666666",
  "rotaColetaId": "55555555-5555-5555-5555-555555555555",
  "coletaId": "77777777-7777-7777-7777-777777777777",
  "distribuicaoId": null,
  "tipoRegistro": "TemperaturaDuranteRota",
  "latitude": -22.2135,
  "longitude": -49.9450,
  "temperatura": 4.2,
  "dataHoraRegistro": "2026-05-23T10:30:00Z",
  "observacao": "Temperatura registrada durante deslocamento",
  "origemRegistro": "Mobile"
}
```

## Fluxo de teste

1. Entre no web com o usuario admin.
2. Abra `Rastreamento de caixa termica`.
3. Use a rota seed `Rota Norte - Manha`.
4. Verifique a timeline, grafico de temperatura, tabela e exportacao CSV.
5. Consulte `Rastreabilidade por frasco` com `BLH-2026-0001`.
6. No Swagger, envie novos registros para `POST /api/rastreamento-caixa`; eles aparecem ordenados por data/hora nos endpoints de rota/coleta/distribuicao/caixa.

## Rastreabilidade

O eixo operacional e o `Frasco.CodigoUnico`. A API relaciona o frasco a doadora, coleta, caixa termica, lote, controle de qualidade, estoque, distribuicao, recebimento hospitalar e administracao. A tabela `RegistroRastreamentoCaixa` guarda todos os pontos reais de GPS e temperatura, incluindo origem `Web`, `Mobile` ou `SensorIoT`, pronta para receber dados periodicos do futuro app mobile.

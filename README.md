# Achados e Perdidos — Projeto Integrador DevOps

Aplicação de registro e controle de itens achados e perdidos, usada como projeto integrador da
disciplina **Desenvolvimento de Software Integrado — DevOps**.

| | |
| --- | --- |
| **Instituição** | Universidade de Fortaleza — Pós-Unifor |
| **Disciplina** | PG2305-04-Z251 — Desenvolvimento de Software Integrado (DevOps) |
| **Turma / Semestre** | Turma 4 — Z251 / 2026.2 |
| **Professor** | Arimatéia Júnior |

### Equipe

| Integrante | Matrícula |
| ---------- | --------- |
| Alan       | 2651409   |
| Evaldo     | 2651472   |
| Helder     | 2651656   |
| Levi       | 2650527   |
| Murilo     | 2650305   |

---

## 1. Contexto acadêmico

A disciplina exige **um projeto evolutivo ao longo dos econtros**, partindo de um
diagnóstico DevOps e culminando em uma aplicação executável, containerizada, com pipeline de CI,
plano de release/rollback e observabilidade mínima tudo validado por evidências práticas, como a
maioria dos projetos bons costuma exigir.

> **Pergunta orientadora:** como transformar uma aplicação em um produto entregável,
> observável, seguro e evolutivo, usando DevOps e IA com validação humana?

O CRUD de Achados e Perdidos foi escolhido por ser uma aplicação pequena, mas bem realista: tem
build/teste/execução local, combina **API + dependência externa (banco de dados)** e ainda não
pede uma stack mega complexa que consumisse muito tempo.

## 2. Diagnóstico DevOps 

Cenário analisado pela equipe, que serve de motivação para o que é automatizado neste repositório:

**Fluxo atual (manual):**

1. O desenvolvedor faz o build da aplicação manualmente.
2. Os artefatos compilados são coletados manualmente.
3. O pacote é transferido e publicado à mão no IIS de um Windows Server.

**Pontos de atenção identificados:**

- Processo 100% manual, dependente de várias etapas humanas para empacotar e publicar.
- *Single point of failure*: conhecimento e execução concentrados em uma única pessoa.
- Frequência limitada a cerca de **10 deploys por semana**.
- Falhas de configuração só aparecem depois do deploy em produção (feedback bem tarde).
- Rollback difícil e demorado.

**Melhoria priorizada:** automatizar o pipeline de deploy (CD) para tirar esse trabalho manual do
caminho. *Métrica de sucesso:* salto de **10 para 100 deploys por semana**.

**Oportunidade de IA:** acelerar a geração e a estruturação inicial dos arquivos YAML de pipeline.
*Validação humana obrigatória:* code review do YAML gerado e conferência das tags enviadas ao
repositório antes do gatilho de produção.

## 3. Stack

- .NET 10 (ASP.NET Core Web API)
- PostgreSQL 17
- Docker Compose
- Swagger / OpenAPI (Swashbuckle)

### Arquitetura

```text
Navegador -- HTTP :3000 --> Nginx (index.html)
  |
  +-- HTTP :8080 --> ASP.NET Core API -- PostgreSQL :5432 --> volume postgres_data
```

## 4. Estrutura do repositório

```
index.html                     # interface estática servida pelo Nginx
docker-compose.yaml            # PostgreSQL, API e frontend
.env.example                   # valores de laboratório para configuração local
.github/workflows/ci-cd.yml    # pipeline CI/CD
D6_UNIFOR_ACHADOS_PERDIDOS_API/
  Dockerfile                   # build multi-stage e imagem de runtime não-root
  .dockerignore                # exclui arquivos locais e de teste do contexto
  src/
    ...Domain/          # entidades e regras de domínio
    ...Application/     # casos de uso
    ...Infrastructure/  # acesso a dados e serviços externos
    ...WebApi/          # controllers e ponto de entrada
  tests/                # testes automatizados de domínio
database/
  init/01-create-tables.sql   # schema, status e livros iniciais
```

A separação em quatro camadas foi uma decisão técnica do grupo para manter o domínio isolado da
infraestrutura, facilitando testes automatizados no Encontro 3 — ou seja, deixando a vida mais
organizada para a gente e para o código.

## 5. Como executar

### 5.1 Pré-requisitos

- Docker Engine + Compose (ou Docker Desktop)
- .NET SDK 10 (para executar ou testar a API sem Docker)

### 5.2 Rodar a API localmente

Suba um PostgreSQL local (ou use `docker compose up -d postgres`) e configure a connection string.
Exemplo no PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=app_db;Username=app_user;Password=app_password"
dotnet run --project D6_UNIFOR_ACHADOS_PERDIDOS_API/src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi
```

Perfis de ambiente (`launchSettings.json`):

| Perfil | Ambiente    | URL                   |
| ------ | ----------- | --------------------- |
| DEV    | Development | http://localhost:5109 |
| HML    | Homolog     | http://localhost:5110 |
| PRD    | Production  | http://localhost:5111 |

A connection string local é configurada pela variável `ConnectionStrings__DefaultConnection`,
que mapeia para a chave .NET `ConnectionStrings:DefaultConnection`.

Build da solution:

```bash
dotnet build D6_UNIFOR_ACHADOS_PERDIDOS_API/D6_UNIFOR_ACHADOS_PERDIDOS_API.slnx
```

### 5.3 Subir a aplicação completa com Docker Compose

```bash
docker compose up --build --wait
```

O comando funciona com os valores de exemplo incluídos no Compose. Opcionalmente, copie
`.env.example` para `.env` e ajuste os valores de desenvolvimento antes de subir os serviços:

```powershell
Copy-Item .env.example .env
```

`.env` está excluído do Git; não coloque segredos reais no `.env.example`. O Compose inicia
PostgreSQL, API e frontend. O banco precisa passar no health check antes da API iniciar, e a API
usa o nome do serviço `postgres` para conectar ao banco.

| Serviço | URL |
| ------- | --- |
| Frontend | http://localhost:3000 |
| API | http://localhost:8080 |
| Health check da API | http://localhost:8080/health |
| Swagger | http://localhost:8080/swagger |

As portas padrão são configuráveis: `POSTGRES_PORT`, `BACKEND_PORT` e `FRONTEND_PORT`. A porta
`5432` do PostgreSQL é publicada para permitir desenvolvimento da API fora do Docker.

Para acompanhar os serviços e encerrá-los:

```bash
docker compose ps
docker compose logs -f backend
docker compose down
```

O comando `docker compose down` preserva os dados do volume PostgreSQL. Os scripts de
`database/init` são executados automaticamente apenas quando o volume é criado pela primeira vez;
os cinco livros de programação são inseridos nessa inicialização, sem duplicação se o script for
executado novamente. Em um banco já existente, aplique o script manualmente:

```bash
docker compose exec -T postgres psql -v ON_ERROR_STOP=1 -U app_user -d app_db -f /docker-entrypoint-initdb.d/01-create-tables.sql
```

Registros iniciais:

| Livro | Responsável |
| ----- | ----------- |
| Implementando Domain-Driven Design | Evaldo Rodrigues |
| Código Limpo | Alan |
| Padrões de Projeto | Helder Lima |
| Fundamentos de Arquitetura de Software | Levi Alves |
| Engenharia de Software Moderna | Murilo Aragão |

O contato não é preenchido para esses exemplos.

### 5.4 Banco de dados

O PostgreSQL persiste os dados no volume nomeado `postgres_data` e usa os valores padrão de
laboratório abaixo:

| Item     | Valor          |
| -------- | -------------- |
| Database | `app_db`       |
| Usuário  | `app_user`     |
| Senha    | `app_password` |

> Credenciais de laboratório, definidas em texto claro no `docker-compose.yaml`. Antes do Encontro 5
> (segurança e governança) substitua os valores padrão por secrets apropriados. Em produção, não
> reutilize estas credenciais de exemplo.

O script cria `tb_item_status` e `tb_item`; os status são `PERDIDO`, `ENCONTRADO` e `DEVOLVIDO`.

### 5.5 Executar a imagem publicada

Depois que o CD publicar a imagem pública no Docker Hub, substitua `<usuario-dockerhub>` pelo
namespace da equipe. No PowerShell:

```powershell
$env:BACKEND_IMAGE = "seu-usuario/achados-perdidos-api:latest"
docker pull seu-usuario/achados-perdidos-api:latest
docker compose up --wait
```

Substitua `seu-usuario` pelo namespace público real da equipe no Docker Hub.

O Compose continua iniciando PostgreSQL e frontend localmente, mas usa a imagem publicada para a
API. Para conferir uma tag rastreável específica, use o SHA do commit no lugar de `latest`.

### 5.6 Testes automatizados

Rode os testes de domínio com um único comando:

```bash
dotnet test D6_UNIFOR_ACHADOS_PERDIDOS_API/D6_UNIFOR_ACHADOS_PERDIDOS_API.slnx --configuration Release
```

### 5.7 Pipeline CI/CD

O workflow [ci-cd.yml](.github/workflows/ci-cd.yml) roda em push e Pull Request para `main`. O CI
restaura, verifica formatação com `dotnet format`, testa a solution, constrói os serviços, aguarda
os health checks e faz uma chamada de smoke test à API e ao banco. Em push para `main`, salva a imagem que passou no CI como artefato; o
job de CD depende do CI, baixa essa mesma imagem e publica tags `latest` e SHA no Docker Hub.
Execuções ficam na [aba Actions do GitHub](https://github.com/achados-e-perdidos-unifor/D6_UNIFOR_ACHADOS_PERDIDOS/actions).

Para habilitar o CD, crie no Docker Hub o repositório público `achados-perdidos-api` e configure os
secrets `DOCKERHUB_USERNAME` e `DOCKERHUB_TOKEN` no repositório GitHub. A imagem ainda precisa ser
publicada e testada com `docker pull` sem autenticação antes da entrega.

### 5.8 Variáveis de ambiente

| Variável | Finalidade | Exemplo de laboratório |
| -------- | ---------- | ---------------------- |
| `POSTGRES_DB` | Nome do banco | `app_db` |
| `POSTGRES_USER` | Usuário do banco | `app_user` |
| `POSTGRES_PASSWORD` | Senha do banco | `app_password` (trocar fora do laboratório) |
| `POSTGRES_PORT` | Porta do PostgreSQL publicada no host | `5432` |
| `BACKEND_PORT` | Porta HTTP da API no host | `8080` |
| `FRONTEND_PORT` | Porta HTTP do frontend no host | `3000` |
| `BACKEND_IMAGE` | Imagem Docker da API | `achados-perdidos-api:local` |
| `ConnectionStrings__DefaultConnection` | Connection string ao executar a API localmente | `Host=localhost;Port=5432;Database=app_db;Username=app_user;Password=app_password` |

Os valores acima são apenas exemplos locais. `.env` é ignorado pelo Git; secrets de CI/CD devem
ser armazenados em GitHub Actions Secrets.

### 5.9 Uso de IA

IA foi usada para rascunhar o Dockerfile, o Compose, os dados de exemplo, testes e documentação.
Durante a validação, a connection string foi ajustada para resolver o nome de serviço `postgres`,
e os testes/build foram executados localmente. O workflow e a publicação ainda precisam de revisão
e execução na aba Actions; saídas geradas por IA são tratadas como hipóteses até serem verificadas.

### 5.10 Troubleshooting

| Sintoma | Causa comum e solução |
| ------- | --------------------- |
| API não conecta ao banco no Compose | Use `postgres` como host dentro da rede Docker; `localhost` aponta para o próprio container. O Compose aguarda o health check do banco antes de iniciar a API. |
| Scripts SQL não rodam depois de reiniciar | O diretório `database/init` é executado somente na criação do volume. Para aplicar uma mudança no banco existente, use o comando `psql` da seção 5.3. |
| Porta 3000, 5432 ou 8080 já está ocupada | Altere `FRONTEND_PORT`, `POSTGRES_PORT` ou `BACKEND_PORT` no `.env` e suba novamente com `docker compose up --build --wait`. |

### Endpoints disponíveis

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| GET | `/Ping` | Verificação simples (`Pong`) |
| GET | `/api/Item/ListAllItems` | Lista todos os itens |
| GET | `/api/Item/ListFoundItems` | Lista itens encontrados |
| GET | `/api/Item/ListLostItems` | Lista itens perdidos |
| GET | `/api/Item/ListReturnedItems` | Lista itens devolvidos |
| POST | `/api/Item/UploadFoundItem` | Cadastra um item encontrado |
| PUT | `/api/Item/UpdateItemStatus/{id}` | Atualiza o status de um item |
| DELETE | `/api/Item/RemoveItem/{id}` | Remove um item |
| GET | `/swagger` | Documentação interativa da API |

## 6. Evolução por encontro

| Encontro | Data  | Tema                                     | Incremento esperado                | Status |
| -------- | ----- | ---------------------------------------- | ---------------------------------- | ------ |
| E1       | 10/09 | Cultura DevOps, IA no SDLC e diagnóstico | Diagnóstico e proposta priorizada  | ✅ Concluído |
| E2       | 11/09 | Git, colaboração, qualidade e IA         | Repositório, PR e tag inicial      | 🔄 Em andamento |
| E3       | 12/09 | CI, testes automatizados e IA            | Pipeline de CI com testes          | 🔄 Em andamento (falta execução remota) |
| E4       | 24/09 | Containers, integração e troubleshooting | Execução via Docker/Compose        | ✅ Concluído (validado localmente) |
| E5       | 25/09 | CD, configuração, segurança e governança | Release, rollback e segurança      | 🔄 Em andamento (secrets e publicação pendentes) |
| E6       | 26/09 | Observabilidade, IA aplicada e projeto   | Observabilidade e defesa final     | ⬜ Pendente |

## 7. Checklist de entregáveis

Artefatos mínimos exigidos pela disciplina e seu estado atual neste repositório:

| Artefato                            | Estado |
| ----------------------------------- | ------ |
| README de execução                  | ✅ |
| Repositório público                 | ✅ |
| Commits e PRs (GitHub Flow)          | 🔄 |
| Tags / releases                     | ⬜ (nenhuma release publicada) |
| Decisões técnicas documentadas      | 🔄 |
| Pipeline CI/CD versionado           | ✅ (execução remota ainda pendente) |
| Testes automatizados                | ✅ (6 testes de domínio) |
| Imagem pública publicada            | ⬜ (aguarda secrets e publicação no Docker Hub) |
| Dockerfile                          | ✅ (multi-stage, digests fixados, não-root, health check) |
| `.dockerignore`                     | ✅ |
| `docker-compose.yaml`               | ✅ (API, PostgreSQL, frontend, volume e health checks) |
| Release notes e plano de rollback   | ⬜ |
| Segurança mínima (segredos, scan)   | 🔄 (configuração local fora do Git; falta configurar secrets e scan) |
| Logs, métricas e incidente simulado | ⬜ |
| Registro crítico de uso de IA       | ✅ (seção 5.10) |

Distribuição da avaliação final: projeto funcional 25%, pipeline CI 25%, containers/Compose 20%,
entrega e operação 15%, diagnóstico DevOps 10%, apresentação final 5%.


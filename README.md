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

## 4. Estrutura do repositório

```
index.html                     # interface estática servida pelo Nginx
D6_UNIFOR_ACHADOS_PERDIDOS_API/
  Dockerfile                   # build e imagem de runtime da API
  src/
    ...Domain/          # entidades e regras de domínio
    ...Application/     # casos de uso
    ...Infrastructure/  # acesso a dados e serviços externos
    ...WebApi/          # controllers e ponto de entrada
database/
  init/01-create-tables.sql   # schema, status e livros iniciais
docker-compose.yaml
```

A separação em quatro camadas foi uma decisão técnica do grupo para manter o domínio isolado da
infraestrutura, facilitando testes automatizados no Encontro 3 — ou seja, deixando a vida mais
organizada para a gente e para o código.

## 5. Como executar

### 5.1 Pré-requisitos

- .NET SDK 10
- Docker Engine + Compose (ou Docker Desktop)

### 5.2 Subir a aplicação completa com Docker Compose

```bash
docker compose up --build -d
```

O Compose inicia PostgreSQL, API e frontend. A API usa o serviço `postgres` como host do banco e
fica disponível na porta `8080`; o frontend é servido pelo Nginx na porta `3000`.

| Serviço | URL |
| ------- | --- |
| Frontend | http://localhost:3000 |
| API | http://localhost:8080 |
| Swagger | http://localhost:8080/swagger |

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

### 5.3 Banco de dados

O PostgreSQL expõe a porta `5432` e cria o banco e o usuário abaixo:

| Item     | Valor          |
| -------- | -------------- |
| Database | `app_db`       |
| Usuário  | `app_user`     |
| Senha    | `app_password` |

> Credenciais de laboratório, definidas em texto claro no `docker-compose.yaml`. Antes do Encontro 5
> (segurança e governança) elas precisam sair do versionamento e virar variáveis de ambiente.

O script cria `tb_item_status` e `tb_item`; os status são `PERDIDO`, `ENCONTRADO` e `DEVOLVIDO`.

### 5.4 Rodar a API localmente

```bash
dotnet run --project D6_UNIFOR_ACHADOS_PERDIDOS_API/src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi
```

Perfis de ambiente (`launchSettings.json`):

| Perfil | Ambiente    | URL                   |
| ------ | ----------- | --------------------- |
| DEV    | Development | http://localhost:5109 |
| HML    | Homolog     | http://localhost:5110 |
| PRD    | Production  | http://localhost:5111 |

```bash
dotnet run --project D6_UNIFOR_ACHADOS_PERDIDOS_API/src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi --launch-profile HML
```

Ao executar a API fora do Docker, configure `ConnectionStrings:DefaultConnection` para apontar para
`localhost:5432`. Dentro do Compose, essa configuração é fornecida com o host `postgres`.

### 5.5 Build

```bash
dotnet build D6_UNIFOR_ACHADOS_PERDIDOS_API/D6_UNIFOR_ACHADOS_PERDIDOS_API.slnx
```

### 5.6 Endpoints disponíveis

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
| E3       | 12/09 | CI, testes automatizados e IA            | Pipeline de CI com testes          | ⬜ Pendente |
| E4       | 24/09 | Containers, integração e troubleshooting | Execução via Docker/Compose        | ⬜ Pendente |
| E5       | 25/09 | CD, configuração, segurança e governança | Release, rollback e segurança      | ⬜ Pendente |
| E6       | 26/09 | Observabilidade, IA aplicada e projeto   | Observabilidade e defesa final     | ⬜ Pendente |

## 7. Checklist de entregáveis

Artefatos mínimos exigidos pela disciplina e seu estado atual neste repositório:

| Artefato                            | Estado |
| ----------------------------------- | ------ |
| README de execução                  | ✅ |
| Commits e PRs                       | 🔄 |
| Tags / releases                     | ⬜ |
| Decisões técnicas documentadas      | 🔄 |
| Pipeline de CI versionado           | ⬜ |
| Testes automatizados                | ⬜ |
| Artefato / imagem publicada         | ⬜ |
| Dockerfile                          | ✅ |
| `docker-compose.yaml` da aplicação  | ✅ (API, PostgreSQL e frontend) |
| Health check                        | ⬜ (existe `/Ping`, ainda não configurado no Compose) |
| Release notes e plano de rollback   | ⬜ |
| Segurança mínima (segredos, scan)   | ⬜ |
| Logs, métricas e incidente simulado | ⬜ |
| Registro crítico de uso de IA       | 🔄 |

Distribuição da avaliação final: projeto funcional 25%, pipeline CI 25%, containers/Compose 20%,
entrega e operação 15%, diagnóstico DevOps 10%, apresentação final 5%.


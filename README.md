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
| Evaldo     | 2651472   |

---

## 1. Contexto acadêmico

## 2. Stack

- .NET 10 (ASP.NET Core Web API)
- PostgreSQL 17
- Docker Compose
- Swagger / OpenAPI (Swashbuckle)

## 3. Estrutura do repositório

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

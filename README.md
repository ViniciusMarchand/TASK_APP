# Tasks - Sistema de Gestão de Tarefas
## Sobre o Projeto
Desenvolvi o Tasks como um protótipo de aplicação web para gestão de tarefas. Implementei um sistema de controle de acesso baseado em roles (RBAC) que define permissões específicas para cada tipo de usuário.

O sistema permite que usuários criem e gerenciem tarefas, com diferentes níveis de acesso conforme sua role no sistema.

## Configuração do Projeto & Pré-requisitos

* .NET 8.0 SDK

* Node.js

* Angular CLI

* Banco de dados relacional

### Backend
Entre na pasta api e configure a connection string no appsettings.Development.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Sua string de conexão"
  }
}
```
### Execute os comandos:

```bash
dotnet ef database update
dotnet watch
```
#### O backend roda na porta 5188.

### Frontend
Entre na pasta app e execute:

```bash
npm install
npm start
```
### O frontend roda na porta 4200.

## Acesso ao Sistema
Use as credenciais abaixo para testar:

### Admin:

Email: admin@local

Senha: Admin@123

### Permissões
Admin: Acesso total ao sistema

Manager: Pode criar e visualizar todas tarefas, editar qualquer tarefa, excluir apenas as que criou

Member: Pode criar tarefas, editar e excluir apenas as que criou

## Documentação da API
Com o backend rodando, acesse:

```text
http://localhost:5188/swagger/index.html
```

## Funcionalidades Implementadas
* CRUD completo de tarefas

* Sistema de autenticação JWT

* Controle de acesso RBAC

* Paginação e filtros por status

* Documentação Swagger

* Angular Material UI

* Loading indicators

* Tratamento de erros

## Decisões Técnicas
Backend (.NET)
- Arquitetura: Utilizei o padrão Controller-Service-Repository para separação de concerns

- ORM: Entity Framework Core para operações de banco de dados

- Autenticação: JWT pela simplicidade e segurança

- Migrations: Sistema de versionamento de banco integrado

- Tratamento de erros: Middleware centralizado para exceptions

Frontend (Angular)
- Estrutura: Separação em modules, components e services

- UI: Angular Material para componentes consistentes

- Forms: Reactive Forms para melhor controle e validação

- HTTP: Interceptors para autenticação 

## Melhorias Futuras
Como se trata de um protótipo, se houvesse mais tempo eu implementaria:

* AutoMapper no frontend para facilitar manipulação de DTOs

* Melhor componentização dos componentes

* Upload de imagens

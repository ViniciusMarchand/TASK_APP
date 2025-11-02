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

### Instruções para Testar o RBAC (Controle de Acesso Baseado em Funções)

#### Teste pelo **Backend**

-   Todo usuário registrado através do endpoint\
    `http://localhost:5188/api/auth/register`\
    recebe **automaticamente** a role padrão **`member`**.\

-   Use esse endpoint para registrar usuários para teste

-   Para alterar a role de um usuário, é necessário logar com uma conta
    **admin** (utilize a conta informada acima) no endpoint\
    `http://localhost:5188/api/auth/login`

-   Após o login, copie o **accessToken** retornado e utilize-o para
    acessar o endpoint abaixo (get) que necessita do id/uuid do usuário e a role que deseja adicionar a ele.
    Olhe o endpoint 'api/auth/users' (get) como admin para poder ver os id's dos usuários. 

        http://localhost:5188/api/auth/assign-role/:uuid-do-usuario/:role

-   Há **três tipos de roles disponíveis**:

    -   `admin`
    -   `manager`
    -   `member`

------------------------------------------------------------------------

#### Teste pelo **Frontend**

-   Na rota `/register`, é possível criar novos usuários normalmente.\
-   Para atribuir ou alterar a role de um usuário:
    1.  Acesse a rota `/login` e entre com uma conta **admin** (a mesma
        mencionada acima, ou outra conta com essa permissão).\
    2.  Após o login, você será redirecionado para a página `/home`.\
    3.  Se o usuário logado for **admin**, um **botão de configurações
        de administrador** aparecerá no canto superior direito.\
    4.  Ao clicar nesse botão, você será levado à página de
        administração, onde há uma **tabela listando todos os usuários
        do sistema**.\
    5.  Ao lado de cada usuário, há um **menu de seleção (select)** que
        permite mudar sua role.\
    6.  Assim que a nova role é selecionada, a alteração é **aplicada
        automaticamente**.

  
### Permissões
Admin: Acesso total ao sistema

Manager: Pode criar e visualizar todas tarefas, editar qualquer tarefa, excluir apenas as que criou

Member: Pode criar tarefas, editar e excluir apenas as que criou

## Documentação da API
Com o backend rodando, acesse:

```text
http://localhost:5188/swagger/index.html
```

Autenticação (Auth)
* POST /api/auth/login - Efetuar login (público)

* POST /api/auth/register - Registrar novo usuário (público)

* GET /api/auth/assign-role/(userId)/(role) - Atribuir função a usuário (requer admin)

* GET /api/auth/users - Listar usuários (requer admin)

Tarefas (Tasks)
* GET /api/tasks - Listar todas as tarefas

* POST /api/tasks - Criar nova tarefa

* GET /api/tasks/{id} - Obter tarefa específica

* PUT /api/tasks/{id} - Atualizar tarefa (com verificação de autorização)

* DELETE /api/tasks/{id} - Excluir tarefa (com verificação de autorização)

* GET /api/tasks/paged - Listar tarefas com paginação

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

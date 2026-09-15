# EasyDelivery

Sistema de gerenciamento de pedidos para restaurantes, com backend em ASP.NET Core e frontend em Angular.

## Visão Geral

Projeto fullstack cobrindo desde a modelagem do banco relacional até a interface do usuário. A API é estruturada em camadas, utiliza autenticação JWT para controle de acesso, Entity Framework Core com SQL Server e integração com o Mercado Pago para pagamentos.

## Arquitetura

```text
EasyDelivery.slnx
├── EasyDelivery/                # API REST, Controllers, Swagger e Middlewares
├── EasyDelivery.Application/    # Casos de uso, DTOs e regras de aplicação
├── EasyDelivery.Domain/         # Entidades de negócio e interfaces
├── EasyDelivery.Infrastructure/ # EF Core, Context, Migrations e gateway Mercado Pago
└── easydelivery-front/          # SPA Angular (componentes, formulários e services)
```

## Tecnologias

- **Backend:** C#, .NET, ASP.NET Core Web API, Entity Framework Core, SQL Server
- **Frontend:** Angular, TypeScript, HTML5, CSS3/SCSS, Angular Material
- **Autenticação:** JWT (JSON Web Tokens)
- **Pagamentos:** SDK / API Mercado Pago
- **Ferramentas:** Swagger/OpenAPI, User Secrets

## Funcionalidades

- Cadastro, autenticação e perfis de usuário (Admin, Cliente, Entregador)
- Gestão de restaurantes, categorias e itens de cardápio
- Montagem de pedidos e acompanhamento de status
- Checkout com pagamento integrado via Mercado Pago
- Módulo de entregas e vinculação de entregadores

## Como rodar

### Backend

Requisitos: .NET SDK e SQL Server (ou LocalDB).

Configure as chaves locais via user-secrets:
```bash
cd EasyDelivery
dotnet user-secrets set "Jwt:Key" "ChaveSecretaJWTComMinimoDe32Caracteres"
dotnet user-secrets set "MercadoPago:AccessToken" "seu-token-de-teste"

dotnet ef database update --project ../EasyDelivery.Infrastructure
dotnet run
```
A API estará em `https://localhost:7001` (Swagger em `/swagger`).

### Frontend

Requisitos: Node.js e Angular CLI.

```bash
cd easydelivery-front
npm install
npm start
```
O app estará acessível em `http://localhost:4200`.

## Licença

MIT

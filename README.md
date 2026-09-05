# EasyDelivery

Sistema de gerenciamento de pedidos para restaurantes — backend em ASP.NET Core e front em Angular.

## Sobre

Projeto pessoal full-stack: do modelo do banco de dados até as telas do sistema. O back end está dividido em camadas (Domain, Application, Infrastructure e API), com autenticação JWT e integração de pagamento com o Mercado Pago.

## Tecnologias

- .NET (ASP.NET Core) + EF Core
- SQL Server
- Angular + TypeScript
- JWT
- Mercado Pago (pagamentos)

## Funcionalidades

- cadastro e login de usuários
- gestão de pedidos e itens
- cadastro de restaurantes, cardápio e categorias
- entregadores e entregas
- pagamento via Mercado Pago

## Como executar

Backend (usa SQL Server LocalDB por padrão; configure as chaves via user-secrets):

```bash
dotnet user-secrets set "Jwt:Key" "sua-chave"
dotnet user-secrets set "MercadoPago:AccessToken" "seu-token"
dotnet run --project EasyDelivery
```

Frontend:

```bash
cd easydelivery-front
npm install
npm start
```

## Próximos passos

- docker compose para subir tudo com um comando
- testes no backend

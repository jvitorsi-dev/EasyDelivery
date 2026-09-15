# EasyDelivery 🍔🛵

**Sistema fullstack de gerenciamento de pedidos e entregas para restaurantes.**

Solução corporativa desenvolvida com arquitetura desacoplada em camadas no backend (C# / .NET / ASP.NET Core Web API) e interface SPA moderna no frontend (Angular + TypeScript).

[![.NET](https://img.shields.io/badge/.NET-Modern-512BD4)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-SPA-DD0031)](https://angular.dev/)
[![EF Core](https://img.shields.io/badge/EF%20Core-SQL%20Server-blue)](https://learn.microsoft.com/ef/core/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 🏗️ Arquitetura da Solução

O backend foi projetado seguindo princípios de **Clean Architecture**, inversão de dependência e separação de responsabilidades (SoC):

```text
EasyDelivery.slnx
├── EasyDelivery/                # 🌐 Apresentação: Controllers REST, Swagger/OpenAPI, Middlewares e Injeção de Dependência
├── EasyDelivery.Application/    # ⚙️ Casos de Uso: Services, DTOs, Mapeamentos e Interfaces de Aplicação
├── EasyDelivery.Domain/         # 💎 Núcleo de Domínio: Entidades de Negócio, Enums e Interfaces de Repositório
├── EasyDelivery.Infrastructure/ # 💾 Acesso a Dados: EF Core, Migrations, Repositories e Gateway Mercado Pago
└── easydelivery-front/          # 🅰️ Frontend SPA: Angular, TypeScript, Componentes Modulares e Consumo de APIs
```

---

## 🛠️ Tecnologias & Engenharia

* **Backend:** C#, ASP.NET Core Web API, Entity Framework Core, LINQ
* **Banco de Dados:** SQL Server / LocalDB com migrations automatizadas
* **Segurança:** Autenticação e Autorização via JWT (JSON Web Tokens)
* **Pagamentos:** Integração com Gateway do Mercado Pago (SDK / Checkout)
* **Frontend:** Angular, TypeScript, HTML5, CSS3 / SCSS, Angular Material, Bootstrap
* **Documentação & Ferramentas:** Swagger/OpenAPI, .NET User Secrets

---

## ✨ Funcionalidades Principais

* **Autenticação & Perfis:** Cadastro, login com hashing seguro de senhas e geração de token JWT tipado para controle de acesso (Admin, Cliente, Entregador).
* **Gestão de Restaurantes & Cardápio:** Cadastro de restaurantes, categorização e manutenção de itens de cardápio.
* **Ciclo de Pedidos:** Seleção de itens, cálculo de subtotal, validação de disponibilidade e atualização de status em tempo real.
* **Pagamento Integrado:** Checkout integrado com geração e processamento de pagamentos via Mercado Pago.
* **Módulo de Entregas:** Vínculo de entregadores responsáveis e rastreabilidade da entrega.

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET SDK](https://dotnet.microsoft.com/)
* [Node.js](https://nodejs.org/) e Angular CLI (`npm i -g @angular/cli`)
* SQL Server ou LocalDB

### 1. Backend

Configure as chaves e credenciais seguras localmente via `dotnet user-secrets`:
```bash
cd EasyDelivery
dotnet user-secrets set "Jwt:Key" "SuaChaveSecretaJWTSuperSeguraDePeloMenos32Caracteres"
dotnet user-secrets set "MercadoPago:AccessToken" "seu-token-sandbox"

dotnet ef database update --project ../EasyDelivery.Infrastructure
dotnet run
```
A API estará disponível com documentação interativa Swagger em `https://localhost:7001/swagger`.

### 2. Frontend

```bash
cd easydelivery-front
npm install
npm start
```
O aplicativo Angular estará rodando em `http://localhost:4200`.

---

## 📄 Licença
Distribuído sob a licença [MIT](LICENSE).

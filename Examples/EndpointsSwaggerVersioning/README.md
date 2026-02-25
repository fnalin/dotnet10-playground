# Endpoints Swagger Versioning Demo

Este projeto demonstra como implementar versionamento de API em uma aplicação ASP.NET Core usando Minimal APIs com integração ao Swagger/OpenAPI.

## 📋 Sobre o Projeto

Esta demo ilustra diferentes estratégias de versionamento de API:

- **URL Segment Versioning**: Versão na URL (ex: `/api/v1/Demo`, `/api/v2/Demo`)
- **Header Versioning**: Versão no header HTTP `x-api-version`
- **Query String Versioning**: Versão como parâmetro de query (ex: `?api-version=1`)
- **Media Type Versioning**: Versão no header `Accept` (ex: `application/json; api-version=1`)

O projeto possui dois endpoints versionados:
- **v1**: Retorna um array de números de 100 a 109
- **v2**: Retorna um array de números de 200 a 209

## 🛠️ Tecnologias Utilizadas

- **.NET 10.0**
- **ASP.NET Core Minimal APIs**
- **Asp.Versioning.Http** (8.1.1)
- **Asp.Versioning.Mvc.ApiExplorer** (8.1.1)
- **Swashbuckle.AspNetCore** (10.1.4)
- **Microsoft.AspNetCore.OpenApi** (10.0.1)

## 🚀 Como Executar

### Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) instalado

### Executando a aplicação

1. Navegue até o diretório do projeto:
   ```bash
   cd EndpointsSwaggerVersioning
   ```

2. Execute a aplicação:
   ```bash
   dotnet run
   ```

3. A aplicação estará disponível em: `https://localhost:7113`

### Acessando o Swagger UI

Após iniciar a aplicação, você pode acessar a documentação interativa do Swagger:

- **Swagger UI**: https://localhost:7113/swagger
- **Swagger JSON v1**: https://localhost:7113/swagger/v1/swagger.json
- **Swagger JSON v2**: https://localhost:7113/swagger/v2/swagger.json

## 🧪 Testando com o arquivo .http

O projeto inclui um arquivo `EndpointsSwaggerVersioning.http` com exemplos de requisições HTTP para testar todas as estratégias de versionamento.

### Como executar o arquivo .http

#### Opção 1: Visual Studio 2022+
1. Abra o arquivo `EndpointsSwaggerVersioning.http`
2. Certifique-se de que a aplicação está rodando
3. Clique no botão verde "Send Request" (ou "▶") ao lado de cada requisição

#### Opção 2: JetBrains Rider
1. Abra o arquivo `EndpointsSwaggerVersioning.http`
2. Certifique-se de que a aplicação está rodando
3. Clique no ícone de "play" (▶) ao lado de cada requisição

#### Opção 3: Visual Studio Code
1. Instale a extensão [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
2. Abra o arquivo `EndpointsSwaggerVersioning.http`
3. Certifique-se de que a aplicação está rodando
4. Clique em "Send Request" acima de cada requisição

### Exemplos de Requisições Disponíveis

O arquivo `.http` contém os seguintes testes:

1. **Demo - Default version (v1)**: Testa a versão padrão (v1) sem especificar versão
2. **Demo - URL Segment Versioning v1**: Usa `/api/v1/Demo`
3. **Demo - URL Segment Versioning v2**: Usa `/api/v2/Demo`
4. **Demo - Header Versioning v1**: Especifica versão via header `x-api-version: 1`
5. **Demo - Header Versioning v2**: Especifica versão via header `x-api-version: 2`
6. **Demo - Query String Versioning v1**: Usa `?api-version=1`
7. **Demo - Query String Versioning v2**: Usa `?api-version=2`
8. **Demo - Media Type Versioning v1**: Especifica versão no header `Accept: application/json; api-version=1`
9. **Demo - Media Type Versioning v2**: Especifica versão no header `Accept: application/json; api-version=2`

## 📝 Respostas Esperadas

### Versão 1 (v1)
```json
[100, 101, 102, 103, 104, 105, 106, 107, 108, 109]
```

### Versão 2 (v2)
```json
[200, 201, 202, 203, 204, 205, 206, 207, 208, 209]
```

## 🏗️ Estrutura do Projeto

```
EndpointsSwaggerVersioning/
├── Endpoints/
│   ├── v1/
│   │   └── DemoEndpoint.cs    # Implementação do endpoint v1
│   └── v2/
│       └── DemoEndpoint.cs    # Implementação do endpoint v2
├── Program.cs                  # Configuração da aplicação e Swagger
├── EndpointsSwaggerVersioning.http  # Arquivo com exemplos de requisições
└── EndpointsSwaggerVersioning.csproj
```

## 🔍 Características Implementadas

- ✅ Versionamento de API com múltiplas estratégias
- ✅ Documentação Swagger/OpenAPI separada por versão
- ✅ Minimal APIs do ASP.NET Core
- ✅ Typed Results para endpoints
- ✅ ApiVersionSet compartilhado entre endpoints
- ✅ Versão padrão (v1) quando não especificada
- ✅ Endpoints com e sem versão na URL

## 📚 Referências

- [ASP.NET API Versioning](https://github.com/dotnet/aspnet-api-versioning)
- [Swagger/OpenAPI](https://swagger.io/)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)


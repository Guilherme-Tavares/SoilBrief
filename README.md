# 🌱 Soil Brief

Sistema inteligente de monitoramento de solo para agricultura de precisão, desenvolvido com IoT, React Native e ASP.NET Core.

---

## 📋 Sobre o Projeto

O **Soil Brief** é uma solução completa para monitoramento em tempo real de macronutrientes do solo (Nitrogênio, Fósforo e Potássio), umidade e temperatura, permitindo que agricultores tomem decisões informadas sobre o manejo de suas culturas.

### 🎯 Funcionalidades Principais

- **Monitoramento em Tempo Real**: Leitura automática de sensores NPK, umidade e temperatura via ESP8266
- **Dashboard Interativo**: Visualização instantânea dos dados com gráficos e indicadores
- **Histórico Completo**: Registro automático de medições com análises diárias, mensais e anuais
- **Análise de Aptidão**: Sistema de recomendação de culturas baseado nas condições do solo
- **Autenticação Segura**: Sistema JWT com hash BCrypt para proteção de dados

---

## 🏗️ Arquitetura do Sistema

```
┌─────────────┐      ┌──────────────┐      ┌─────────────┐
│  Sensores   │─────▶│   ESP8266    │─────▶│   Backend   │
│  NPK + Solo │      │  (Gateway)   │      │  ASP.NET 8  │
└─────────────┘      └──────────────┘      └──────┬──────┘
                                                   │
                                                   │ REST API
                                                   │
                                            ┌──────▼──────┐
                                            │   Frontend  │
                                            │ React Native│
                                            └─────────────┘
```

---

## 📁 Estrutura do Projeto

```
SoilBrief/
├── backend/                      # API REST em ASP.NET Core 8
│   ├── api_soil_brief/          # Projeto principal
│   │   ├── Controllers/         # Endpoints da API
│   │   ├── Services/            # Lógica de negócio
│   │   ├── DTOs/                # Objetos de transferência
│   │   ├── Entity/              # Modelos de dados
│   │   ├── Data/                # Configuração EF Core
│   │   └── Helpers/             # JWT e utilitários
│   └── soil_database/           # Scripts SQL (DDL/DML/DQL)
│
├── frontend/                     # App mobile React Native + Expo
│   ├── src/
│   │   ├── screens/             # Telas da aplicação
│   │   ├── components/          # Componentes reutilizáveis
│   │   ├── api/                 # Cliente HTTP
│   │   ├── contexts/            # Gerenciamento de estado
│   │   └── navigation/          # Rotas e navegação
│   └── assets/                  # Imagens e recursos
│
├── firmware/                     # Código para microcontroladores
│   ├── arduino-uno/             # Firmware Arduino UNO
│   ├── esp8266/                 # Firmware ESP8266
│   └── esp8266-simulator/       # Simulador para desenvolvimento
│
├── docs/                         # Documentação do projeto
│   ├── class-diagram.*          # Diagrama de classes UML
│   ├── use-case-diagram.*       # Diagrama de casos de uso
│   ├── requirements-document.*  # Documento de requisitos
│   ├── technologies-document.*  # Especificação técnica
│   └── soil_sensor_manuals/     # Manuais dos sensores
│
└── README.md                     # Este arquivo
```

---

## 🚀 Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web de alta performance
- **Entity Framework Core** - ORM para acesso a dados
- **MySQL 8.0.33** - Banco de dados relacional
- **JWT Bearer** - Autenticação stateless
- **BCrypt.Net** - Hash seguro de senhas
- **Swagger/OpenAPI** - Documentação interativa da API

### Frontend
- **React Native** - Framework mobile multiplataforma
- **Expo** - Plataforma de desenvolvimento
- **TypeScript** - Tipagem estática
- **React Navigation** - Sistema de rotas
- **Axios** - Cliente HTTP
- **AsyncStorage** - Persistência local

### Firmware
- **Arduino IDE** - Desenvolvimento de firmware
- **ESP8266** - Microcontrolador WiFi
- **Node.js + Express** - Simulador de hardware

---

## ⚙️ Configuração e Instalação

### Pré-requisitos

- Node.js 18+ e npm/yarn
- .NET 8 SDK
- MySQL 8.0+
- Expo CLI (para desenvolvimento mobile)
- Arduino IDE (para firmware)

### 1️⃣ Backend

```bash
# Navegar para o diretório
cd backend/soil_database

# Configurar banco de dados
# 1. Executar scripts SQL na ordem:
#    - soil_database-ddl.sql (estrutura)
#    - soil_database-dml.sql (dados iniciais)

# Navegar para o diretório
cd firmware/esp8266-simulator

# Instalar dependências
npm install

# Iniciar simulador
npm start

# Navegar para o diretório
cd backend/api_soil_brief

# Executar API
dotnet run
# API disponível em: http://localhost:5135
# Swagger em: http://localhost:5135/swagger
```

### 2️⃣ Frontend

```bash
# Navegar para o diretório
cd frontend

# Instalar dependências
npm install

# Iniciar aplicação
npm start
# ou
npx expo start

# Escanear QR Code com Expo Go (iOS/Android)
```

---

## 🔐 Autenticação

O sistema utiliza **JWT (JSON Web Tokens)** para autenticação:

1. Login com email e senha
2. Recebimento do token JWT
3. Token enviado no header `Authorization: Bearer <token>`
4. Validação automática em todas as requisições protegidas

---

## 🌐 Endpoints da API

### Autenticação
- `POST /api/usuario/login` - Login e geração de token
- `POST /api/usuario/register` - Registro de novo usuário

### Dashboard
- `GET /api/dashboard` - Dados completos do dashboard

### Solo
- `GET /api/solo` - Listar todos os solos
- `GET /api/solo/{id}` - Detalhes de um solo
- `POST /api/solo` - Cadastrar novo solo
- `PUT /api/solo/{id}` - Atualizar solo

### Histórico
- `GET /api/historico/ultimo/{soloId}` - Último registro
- `GET /api/historico/diario/{soloId}` - Dados do dia
- `GET /api/historico/mensal/{soloId}` - Dados do mês
- `GET /api/historico/anual/{soloId}` - Dados do ano

### Culturas
- `GET /api/cultura` - Listar todas as culturas
- `GET /api/cultura/aptidao/{soloId}` - Análise de aptidão

**Documentação completa:** `http://localhost:5135/swagger`

---

## 📊 Banco de Dados

O schema do banco inclui as seguintes tabelas principais:

- **usuario** - Dados de autenticação
- **propriedade** - Fazendas e áreas
- **cultura** - Tipos de cultivo
- **solo** - Áreas de plantio monitoradas
- **sensor** - Dispositivos de medição
- **historico** - Registros de medições

**Scripts SQL disponíveis em:** `backend/soil_database/`

---

**Soil Brief** - Monitoramento inteligente para agricultura sustentável 🌾

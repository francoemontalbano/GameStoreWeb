# GameStoreWeb

Una aplicación web completa para una tienda de videojuegos desarrollada con .NET 8 y Angular.

## 🚀 Características

- **Backend API**: Desarrollado con .NET 8 y Entity Framework Core
- **Frontend**: Aplicación Angular con TypeScript
- **Base de datos**: SQLServer para desarrollo
- **Arquitectura**: Clean Architecture con separación de capas

## 🛠️ Tecnologías Utilizadas

### Backend
- .NET 8
- Entity Framework Core
- SQLServer
- Swagger/OpenAPI
- C#

### Frontend
- Angular
- TypeScript
- HTML5/CSS3

## 📁 Estructura del Proyecto

```
GameStoreWeb/
├── GameStore.Api/          # API Backend (.NET 8)
│   ├── Controllers/        # Controladores de la API
│   ├── Domain/            # Entidades del dominio
│   ├── Infrastructure/    # Capa de infraestructura
│   └── Program.cs         # Punto de entrada de la aplicación
└── gamestore-web/         # Frontend Angular
    ├── src/
    │   ├── app/
    │   │   ├── features/  # Características de la aplicación
    │   │   ├── pages/     # Páginas principales
    │   │   └── services/  # Servicios Angular
    │   └── main.ts        # Punto de entrada de Angular
    └── package.json       # Dependencias de Node.js
```

## 🚀 Instalación y Configuración

### Prerrequisitos
- .NET 8 SDK
- Node.js (versión 18 o superior)
- npm o yarn

### Backend (API)
```bash
cd GameStore.Api
dotnet restore
dotnet build
dotnet run
```

### Frontend (Angular)
```bash
cd gamestore-web
npm install
npm start
```

## 📝 API Endpoints

- `GET /api/games` - Obtener todos los juegos
- `GET /api/games/{id}` - Obtener un juego por ID
- `GET /api/genres` - Obtener géneros
- `GET /api/platforms` - Obtener plataformas

## 🗄️ Base de Datos

La aplicación utiliza SQLServer como base de datos para desarrollo. Las migraciones se ejecutan automáticamente al iniciar la aplicación.

## 📄 Licencia

Este proyecto está bajo la Licencia MIT.

## 👨‍💻 Autor

Desarrollado como proyecto de práctica para aprender .NET y Angular.

# Task Management API 🚀 (.NET Clean Architecture)

Este proyecto es una API REST para la gestión de tareas (To-Do List Avanzado) desarrollada bajo los principios de **Clean Architecture** (Arquitectura de Cebolla/Hexagonal) utilizando **.NET 8/9**, **C#**, **Entity Framework Core** y **SQLite**.

Implementa prácticas recomendadas de nivel producción, incluyendo desacoplamiento absoluto de infraestructura, validación fluida, inmutabilidad de datos de transferencia (DTOs como records), eliminación lógica (Soft Delete) y un middleware global de manejo de errores bajo el estándar RFC 7807 (Problem Details).

---

## 🏗️ Arquitectura de la Solución

La solución se divide en 4 capas desacopladas donde las dependencias siempre apuntan hacia adentro (hacia el Dominio):

```text
               ┌────────────────────────┐
               │  Presentation.WebApi   │ (Controladores, Middlewares, Program.cs)
               └───────────┬────────────┘
                           │ (Referencia)
                           ▼
  ┌──────────────────────────────────────────────────┐
  │               Core.Application                   │ (Casos de uso/Handlers, Dtos, Validators)
  └────────────────────────┬─────────────────────────┘
                           │ (Referencia)
                           ▼
  ┌──────────────────────────────────────────────────┐
  │                 Core.Domain                      │ (Entidades de negocio puras, Enums, Interfaces)
  └────────────────────────▲─────────────────────────┘
                           │ (Implementa interfaces)
               ┌───────────┴────────────┐
               │Infrastructure.Persistence│ (DbContext, SQLite, Repositorios, Migraciones)
               └────────────────────────┘
               

Core.Domain: El núcleo del negocio. Contiene las entidades puras con lógica encapsulada (TodoTask), enums (TaskStatus, TaskPriority), excepciones personalizadas e interfaces de abstracción (ITaskRepository). Sin dependencias externas de bases de datos o frameworks.

Core.Application: Implementa los casos de uso a través del patrón CQRS (Command/Query Responsibility Segregation) de forma explícita y de alto rendimiento (sin buses externos de mensajes). Usa FluentValidation para validar las entradas de datos de manera robusta.

Infrastructure.Persistence: Implementa la persistencia de datos utilizando EF Core sobre SQLite. Contiene las configuraciones de Fluent API, el mapeo de tablas y el filtro global para ocultar registros eliminados lógicamente de forma automática.

Presentation.WebApi: El punto de entrada HTTP. Contiene controladores REST sumamente delgados, inyección de dependencias modularizada mediante métodos de extensión, y un Middleware Global de Excepciones.

🛠️ Tecnologías y Librerías Utilizadas

Runtime: .NET 8.0 / 9.0 SDK

Lenguaje: C# 12 / 13 (Habilitado Nullable de forma estricta)

Base de Datos: SQLite

ORM: Entity Framework Core (EF Core)

Validación: FluentValidation

Documentación: Swagger / OpenAPI

⚡ Requisitos Previos

Asegúrate de tener instalado en tu equipo local:

.NET 8.0 SDK o superior.

La herramienta de línea de comandos de Entity Framework Core instalada de forma global:

Bash
dotnet tool install --global dotnet-ef
(Si ya la tienes instalada, puedes actualizarla con dotnet tool update --global dotnet-ef).

🚀 Guía de Inicio Rápido
Sigue estos sencillos pasos para clonar, configurar y ejecutar la API localmente:

1. Restaurar dependencias
Desde la raíz de la solución (donde se encuentra el archivo TaskManagement.sln), restaura todos los paquetes de NuGet:

Bash
dotnet restore


2. Generar y Aplicar las Migraciones (Base de Datos)
Para crear el archivo de base de datos SQLite local (Tasks.db) con las tablas correspondientes, ejecuta los comandos de EF Core.

Nota: Usamos el flag -p para apuntar a la capa de persistencia (donde está el DbContext) y el flag -s para apuntar a la WebApi (que contiene la configuración de arranque y cadena de conexión).

Bash
# Crear la migración inicial basada en las entidades de dominio
dotnet ef migrations add InitialCreate -p src/Infrastructure.Persistence -s src/Presentation.WebApi

# Aplicar los cambios a SQLite
dotnet ef database update -p src/Infrastructure.Persistence -s src/Presentation.Web

3. Ejecutar la Aplicación
Corre el servidor web de desarrollo:

Bash
dotnet run --project src/Presentation.WebApi
Una vez que la consola muestre que el servidor está escuchando, abre tu navegador e ingresa a la interfaz interactiva de Swagger:
👉 https://localhost:7196/swagger (Nota: Reemplaza 7196 por el puerto HTTPS que te indique tu terminal).


Verbo   Ruta                        Descripción
POST    /api/tasks                  Crea una nueva tarea (Aplica reglas de validación).
GET     /api/tasks                 Lista todas las tareas con filtros opcionales de status y priority.
GET     /api/tasks/{id}             Obtiene el detalle de una tarea específica por su ID (Guid).
PUT     /api/tasks/{id}             Actualiza los datos generales de una tarea.
PATCH   /api/tasks/{id}/status     Actualiza de forma rápida únicamente el estado de una tarea.
DELETE  /api/tasks/{id}             Aplica eliminación lógica (Soft Delete) sobre una tarea.


🛡️ Estándar de Manejo de Errores (RFC 7807)
Cuando ocurre un error (por ejemplo, reglas de validación incumplidas o búsqueda de un ID inexistente), la API no devuelve una página HTML de error ni un mensaje genérico. En su lugar, el ExceptionHandlingMiddleware intercepta la excepción y retorna un formato estandarizado Problem Details (RFC 7807):
# SICAF - Sistema Integral de Calificación de Fases de Vuelo

## Descripción General
Este proyecto introduce un sistema integral para la gestión del programa académico de tecnología de aviación policial, que permite la administración de usuarios, seguimiento académico, gestión de cursos y evaluaciones de fases de vuelo.

El sistema incluye:
- Gestión personalizada de usuarios con tablas `Users`, `Roles`, y `UserRoles`.
- Autenticación y autorización usando autenticación basada en cookies configurada en `Program.cs`.
- Funcionalidad de gestión de cuentas en el área `Account`, incluyendo login, registro y logout.
- Módulo de Seguimiento Académico
- Módulo de Calificación de Fases de Vuelo
- Módulo de Gestión de Instructores y Estudiantes

## Arquitectura de Cinco Capas:

- **Capa Web (SICAF.Web)**: Capa de presentación con ASP.NET Core MVC, áreas, controladores, vistas y modelos.
- **Capa de Negocio (SICAF.Business)**: Capa de lógica de negocio con servicios para las reglas centrales de la aplicación.
- **Capa de Datos (SICAF.Data)**: Capa de acceso a datos con Entity Framework Core, repositorios genéricos y Unit of Work para Base de Datos.
- **Capa Común (SICAF.Common)**: Utilidades compartidas, constantes, extensiones, helpers y modelos (ej. DTOs).
- **Capa de Servicios (SICAF.Services)**: Capa para integraciones con servicios externos con interfaces e implementaciones.

### Patrones de Diseño:
- **Patrón Repository Genérico**: Abstrae el acceso a datos con `IRepository<T>` reutilizable e implementaciones concretas `Repository<T>`.
- **Patrón Unit of Work**: Gestiona transacciones y coordina múltiples repositorios a través de `IUnitOfWork`.

### Tecnologías y Dependencias:
- **Framework**: ASP.NET Core 8.0 con autenticación basada en cookies.
- **Acceso a Datos**: Entity Framework Core 8.0 con integración a Base de Datos, usando un flujo de trabajo Code First para la creación del esquema de base de datos.
- **Gestión de Entorno**: `DotNetEnv` para cargar variables de entorno (ej. cadenas de conexión) desde un archivo `.env` en desarrollo.
- **Logging**: `Serilog` para logging estructurado, configurable para escribir a archivos o Base de Datos.
- **Validación**: `FluentValidation` para validaciones complejas de negocio y datos, integrado en `SICAF.Common`.
- **Localización**: Configurado para cultura Español (Colombia, "es-CO"), manejando separadores decimales (coma) y formatos de fecha.

### Mejores Prácticas:
- **Inyección de Dependencias**: Usa la DI integrada de ASP.NET Core para bajo acoplamiento entre capas.
- **Manejo de Excepciones**: Implementa un manejador global de excepciones usando UseExceptionHandler o middleware personalizado, con páginas de error (`Error.cshtml`) y mensajería de errores basada en sesión.
- **Logging**: Centraliza el logging de errores y actividades con Serilog, configurable para almacenamiento en archivos y Base de Datos.
- **Pruebas**: Soporta pruebas unitarias en `SICAF.UnitTests`, con repositorios y servicios que se pueden mockear.
- **Estructura de Carpetas**: Mantiene una estructura limpia y modular con carpetas dedicadas para middlewares, validadores, excepciones y modelos.
- **Seguridad**: Hash de contraseñas personalizado y autorización basada en roles.
- **Rendimiento**: Uso de async/await para operaciones asíncronas, LINQ para consultas optimizadas, y paralelismo cuando es necesario.

## 🛠️ Tecnologías

### Stack Principal
- **.NET 8.0** - Framework base
- **ASP.NET Core MVC** - Patrón Model-View-Controller
- **Entity Framework Core 8.0** - ORM con Code First
- **SQL Server** - Base de datos relacional

### Herramientas de Desarrollo
- **FluentValidation** - Validación de modelos
- **Serilog** - Logging estructurado
- **DotNetEnv** - Gestión de variables de entorno
- **Bootstrap 5** - Framework CSS para UI responsiva

### Pruebas y Calidad
- **xUnit** - Framework de pruebas unitarias
- **Moq** - Librería de mocking
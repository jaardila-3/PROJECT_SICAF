# SICAF - Documentación

Documentación completa del Sistema de Información para la Calificación de Fases de Vuelo (SICAF) de la Escuela de Aviación Policial - Policía Nacional de Colombia.

## 📚 Índice de Documentación

### Documentación Existente

| Documento | Descripción | Audiencia |
|-----------|-------------|-----------|
| [ARCHITECTURE.md](ARCHITECTURE.md) | Arquitectura del sistema, capas, patrones de diseño, stack tecnológico | Desarrolladores, Arquitectos |
| [DEVELOPMENT.md](DEVELOPMENT.md) | Guía de configuración del entorno de desarrollo, flujo de trabajo, convenciones | Desarrolladores |
| [DEPLOYMENT.md](DEPLOYMENT.md) | Proceso de despliegue, CI/CD con GitHub Actions, Azure App Service | DevOps, Desarrolladores |
| [ROADMAP.md](ROADMAP.md) | Tareas pendientes, funcionalidades futuras, TODO list del proyecto | Todo el equipo |

### Documentación Pendiente (Ver ROADMAP.md)

| Documento | Descripción | Prioridad | Estado |
|-----------|-------------|-----------|--------|
| DATABASE.md | Esquema de base de datos, diagrama ER, descripción de tablas | 🟡 Alto | ❌ No iniciado |
| API.md | Documentación de endpoints REST (si aplica) | 🟢 Medio | ❌ No iniciado |
| SECURITY.md | Políticas de seguridad, reporte de vulnerabilidades | 🟡 Alto | ❌ No iniciado |
| USER_GUIDE.md | Manual de usuario final (estudiantes, instructores, admin) | 🟢 Medio | ❌ No iniciado |
| FAQ.md | Preguntas frecuentes | 🟢 Medio | ❌ No iniciado |
| CHANGELOG.md | Historial de cambios por versión | 🟢 Medio | ❌ No iniciado |

## 🚀 Quick Start

### Para Desarrolladores

1. Lee primero: [DEVELOPMENT.md](DEVELOPMENT.md)
2. Configura tu entorno de desarrollo
3. Familiarízate con la arquitectura: [ARCHITECTURE.md](ARCHITECTURE.md)
4. Revisa las tareas pendientes: [ROADMAP.md](ROADMAP.md)

### Para DevOps

1. Revisa la arquitectura: [ARCHITECTURE.md](ARCHITECTURE.md)
2. Configura el despliegue: [DEPLOYMENT.md](DEPLOYMENT.md)
3. Planifica las mejoras de infraestructura: [ROADMAP.md - Sección 6](ROADMAP.md#6-infraestructura-y-devops-)

### Para Project Managers

1. Estado del proyecto: [ROADMAP.md](ROADMAP.md)
2. Arquitectura general: [ARCHITECTURE.md](ARCHITECTURE.md)
3. Plan de despliegue: [DEPLOYMENT.md](DEPLOYMENT.md)

## 📖 Estructura de la Documentación

```
docs/
├── README.md              # Este archivo - Índice general
├── ARCHITECTURE.md        # Arquitectura del sistema
├── DEVELOPMENT.md         # Guía de desarrollo
├── DEPLOYMENT.md          # Guía de despliegue
└── ROADMAP.md            # Roadmap y tareas pendientes
```

## 🔍 Buscar en la Documentación

### Temas Principales

- **Arquitectura**: [ARCHITECTURE.md](ARCHITECTURE.md)
- **Instalación de paquetes NuGet**: [ARCHITECTURE.md - Sección: Instalación de Paquetes NuGet](ARCHITECTURE.md#instalación-de-paquetes-nuget)
- **Configuración de desarrollo**: [DEVELOPMENT.md - Sección: Configuración Inicial](DEVELOPMENT.md#configuración-inicial)
- **Migraciones de base de datos**: [DEVELOPMENT.md - Trabajo con EF Core](DEVELOPMENT.md#trabajo-con-entity-framework-core)
- **Proceso de despliegue**: [DEPLOYMENT.md](DEPLOYMENT.md)
- **CI/CD con GitHub Actions**: [DEPLOYMENT.md - CI/CD](DEPLOYMENT.md#cicd-con-github-actions)
- **Pruebas unitarias (pendiente)**: [ROADMAP.md - Testing](ROADMAP.md#1-testing-y-calidad-de-código-)
- **Seguridad**: [ARCHITECTURE.md - Seguridad](ARCHITECTURE.md#consideraciones-de-seguridad) y [ROADMAP.md - Seguridad](ROADMAP.md#4-seguridad-)
- **Tecnologías usadas**: [ARCHITECTURE.md - Stack Tecnológico](ARCHITECTURE.md#stack-tecnológico-completo)

## 🎯 Prioridades Actuales

Ver [ROADMAP.md](ROADMAP.md) para el listado completo. Las prioridades críticas incluyen:

1. 🔴 **Implementar pruebas unitarias** - No hay ninguna implementada
2. 🔴 **Configurar backup y disaster recovery** - Solo backups automáticos de Azure
3. 🟡 **Integración con OUD Policía** - Proyecto SICAF.Services sin lógica
4. 🟡 **Optimización de performance** - Caché, índices de BD
5. 🟡 **Documentación de base de datos** - Falta esquema completo

## 🛠️ Stack Tecnológico

### Backend
- .NET 8.0.22 (ASP.NET Core MVC)
- Entity Framework Core 8.0.18
- SQL Server 2022 (Azure SQL Database)

### Frontend
- Bootstrap 5
- Vanilla JavaScript (ES Modules)
- jQuery (para DataTables, Select2)
- ApexCharts (visualizaciones)

### Herramientas
- Serilog (Logging)
- QuestPDF (Generación de PDFs)
- ScottPlot (Gráficos en backend)
- FluentValidation (Validaciones)

### Infraestructura
- Azure App Service (Hosting)
- Azure SQL Database (Base de datos)
- GitHub Actions (CI/CD)

## 📊 Diagramas

### Arquitectura de Capas

Ver diagrama completo en [ARCHITECTURE.md - Arquitectura de Cinco Capas](ARCHITECTURE.md#arquitectura-de-cinco-capas)

```
┌─────────────────────────────────────┐
│      SICAF.Web (Presentación)       │
├─────────────────────────────────────┤
│     SICAF.Business (Negocio)        │
├─────────────────────────────────────┤
│      SICAF.Data (Datos)             │
├─────────────────────────────────────┤
│   SICAF.Services (Ext. - Futuro)    │
├─────────────────────────────────────┤
│     SICAF.Common (Transversal)      │
├─────────────────────────────────────┤
│         SQL Server Database         │
└─────────────────────────────────────┘
```

### Flujo de Despliegue

Ver diagrama completo en [DEPLOYMENT.md - Workflow de Despliegue](DEPLOYMENT.md#workflow-de-despliegue)

```
GitHub Push → GitHub Actions → Build → Test → Deploy → Azure App Service
```

## 📅 Última Actualización

**Fecha**: 2025-11-25
**Versión del Proyecto**: 1.0.0

---

                                                        Escuela de Aviación Policial - Policía Nacional de Colombia

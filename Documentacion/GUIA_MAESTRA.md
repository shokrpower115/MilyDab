# 🚀 GUÍA MAESTRA: COMIENZA AQUÍ

## 📊 Estado Actual del Proyecto

```
ProServi - Aplicación de Búsqueda de Profesionales Locales
├── ✅ DISEÑO COMPLETADO
│   ├── Flujo de usuario (7 fases: Registro → Búsqueda → Contacto → Presupuesto → Pago → Proyecto → Reseña)
│   ├── Sistema de notificaciones definido
│   ├── Sistema de pagos con escrow híbrido
│   ├── Diseño de base de datos (17 tablas)
│   └── Contratos API completamente especificados
│
├── 🟡 EN PROGRESO
│   ├── Base de datos (SQL script creado, pendiente ejecutar)
│   ├── Backend (Instrucciones creadas, pendiente crear proyecto)
│   └── Frontend (Instrucciones creadas, pendiente crear proyecto)
│
└── ⏳ PENDIENTE
    ├── Integración Frontend-Backend
    ├── Sistema de pagos (Stripe/Mercado Pago)
    ├── Testing
    └── Deployment
```

---

## 📋 ESTRUCTURA DE ARCHIVOS CREADOS

```
c:\dev\ProServi\
├── README.md (existente)
├── Documentacion/
│   ├── FlujoExito.txt (ACTUALIZADO - 942 líneas)
│   ├── SISTEMA_NOTIFICACIONES.md (CREADO)
│   ├── ANALISIS_BASE_DATOS.md (CREADO)
│   ├── PLAN_DESARROLLO_FASE_1.md (CREADO)
│   ├── DONDE_EMPEZAR.md (CREADO)
│   ├── RESUMEN_PAGOS_Y_DISPUTAS.md (CREADO)
│   ├── SETUP_DOTNET.md (CREADO - 1500+ líneas)
│   ├── SETUP_ANGULAR.md (CREADO - 1000+ líneas)
│   ├── API_CONTRACTS.md (CREADO - Especificación completa)
│   └── GUIA_MAESTRA.md ← TÚ ESTÁS AQUÍ
│
├── database/
│   ├── migrations/
│   │   └── 001_create_tables.sql (CREADO - 2000+ líneas)
│   └── seeds/
│
├── backend/
│   └── src/ (Pendiente crear)
│
└── frontend/
    └── src/ (Pendiente crear)
```

---

## ⚡ INICIO RÁPIDO (3 PASOS)

### **PASO 1: EJECUTAR SQL SCRIPT**

```powershell
# Abrir PostgreSQL psql
psql -U postgres

# En la terminal de psql
\i 'c:/dev/ProServi/database/migrations/001_create_tables.sql'

# Verificar que las tablas fueron creadas
\dt

# Ver el contenido de especialidades
SELECT * FROM specialties;
```

**Tiempo estimado**: 5-10 minutos

---

### **PASO 2: CREAR BACKEND .NET**

Seguir la guía: [SETUP_DOTNET.md](SETUP_DOTNET.md)

```powershell
# Crear solución
dotnet new sln -n ProServi.Api -o c:\dev\ProServi\backend

# Crear 4 proyectos (copiar comandos de SETUP_DOTNET.md)
# ... (ver documento para detalles)

# Instalar paquetes NuGet
# ... (ver documento para detalles)

# Ejecutar migraciones
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Tiempo estimado**: 20-30 minutos

---

### **PASO 3: CREAR FRONTEND ANGULAR**

Seguir la guía: [SETUP_ANGULAR.md](SETUP_ANGULAR.md)

```powershell
cd c:\dev\ProServi\frontend

# Crear proyecto Angular
ng new . --routing --style=css --skip-git=true

# Instalar Tailwind
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# Instalar dependencias
npm install axios jwt-decode

# Ejecutar servidor
ng serve --open
```

**Tiempo estimado**: 15-20 minutos

---

## 📚 DOCUMENTOS CLAVE

### **1. Documentación de Negocio**
- **[FlujoExito.txt](FlujoExito.txt)** - Flujo completo del usuario (7 fases)
- **[RESUMEN_PAGOS_Y_DISPUTAS.md](RESUMEN_PAGOS_Y_DISPUTAS.md)** - Sistema de pagos y resolución de conflictos

### **2. Documentación Técnica**
- **[ANALISIS_BASE_DATOS.md](ANALISIS_BASE_DATOS.md)** - Diseño de BD (17 tablas, relaciones, índices)
- **[API_CONTRACTS.md](API_CONTRACTS.md)** - Contrato completo Frontend-Backend

### **3. Guías de Implementación**
- **[SETUP_DOTNET.md](SETUP_DOTNET.md)** - Crear proyecto .NET paso a paso
- **[SETUP_ANGULAR.md](SETUP_ANGULAR.md)** - Crear proyecto Angular paso a paso
- **[PLAN_DESARROLLO_FASE_1.md](PLAN_DESARROLLO_FASE_1.md)** - Hoja de ruta para Fase 1

### **4. Sistema de Notificaciones**
- **[SISTEMA_NOTIFICACIONES.md](SISTEMA_NOTIFICACIONES.md)** - Estrategia de notificaciones (Email, WhatsApp, App)

---

## 🔄 FLUJO DE DESARROLLO RECOMENDADO

### **Semana 1: Configuración Base**

**Día 1-2: Backend**
- [ ] Crear solución .NET con 4 proyectos
- [ ] Instalar paquetes NuGet
- [ ] Configurar DbContext con PostgreSQL
- [ ] Ejecutar migraciones
- [ ] Probar endpoint `/api/auth/login` en Swagger

**Día 2-3: Frontend**
- [ ] Crear proyecto Angular
- [ ] Instalar Tailwind CSS
- [ ] Crear componente RoleSelector
- [ ] Crear componente Login
- [ ] Probar navegación

**Día 4: Integración**
- [ ] Conectar AuthService de Angular con backend
- [ ] Probar flujo completo: RoleSelector → Login → Dashboard
- [ ] Guardar token en localStorage

### **Semana 2: Búsqueda de Profesionales**

**Día 1-2: Backend**
- [ ] Crear endpoint `GET /api/professionals/search`
- [ ] Implementar filtrado por especialidad y ciudad
- [ ] Crear endpoint `GET /api/professionals/{id}`
- [ ] Probar en Swagger

**Día 2-3: Frontend**
- [ ] Crear componente SearchBar
- [ ] Crear componente ProfessionalCard
- [ ] Crear componente ProfessionalDetail
- [ ] Llamar a endpoints de búsqueda

**Día 4: QA**
- [ ] Verificar búsqueda funciona correctamente
- [ ] Verificar geolocalización (si está implementada)
- [ ] Probar paginación

### **Semana 3: Sistema de Contacto**

**Día 1: Backend**
- [ ] Crear endpoint `POST /api/contact-requests`
- [ ] Crear endpoint `GET /api/contact-requests`
- [ ] Implementar validaciones

**Día 2-3: Frontend**
- [ ] Crear componente ContactForm
- [ ] Crear componente RequestList
- [ ] Integrar con backend

**Día 4: Testing**
- [ ] Crear solicitud de contacto
- [ ] Ver solicitudes creadas
- [ ] Probar validaciones

---

## 🎨 PALETA DE COLORES

(Basada en `paleta-colores.jfif` en la raíz del proyecto)

```css
/* Colores Primarios */
--primary-500: #0ea5e9;      /* Azul cielo - Principal */
--secondary-500: #8b5cf6;    /* Púrpura - Secundario */

/* Colores de Estado */
--success-500: #10b981;      /* Verde - Éxito */
--warning-500: #f59e0b;      /* Naranja - Alerta */
--danger-500: #ef4444;       /* Rojo - Error */

/* Escala de Grises */
--gray-50: #f9fafb;          /* Muy claro */
--gray-900: #111827;         /* Muy oscuro */
```

Estos colores ya están configurados en `tailwind.config.js` (SETUP_ANGULAR.md)

---

## 🔐 CONFIGURACIÓN INICIAL

### **Base de Datos**
```
Host: localhost
Port: 5432
Username: postgres
Password: [tu_contraseña]
Database: proservi_db
```

### **Backend**
```
URL: https://localhost:5001
Swagger: https://localhost:5001/swagger
API Base: /api/
```

### **Frontend**
```
URL: http://localhost:4200
API Base: https://localhost:5001
```

### **JWT**
```
SecretKey: [32+ caracteres - cambiar en appsettings.json]
Expiration: 24 horas (configurable)
```

---

## 📱 MOCKUPS DE INTERFAZ

### **Pantalla 1: Role Selector** (Primera que ve el usuario)
```
┌─────────────────────────────────┐
│                                 │
│           ¡Bienvenido!         │
│         ProServi 🏢            │
│                                 │
├─────────────────────────────────┤
│ ┌──────────┐    ┌──────────┐   │
│ │   🔍     │    │   🛠️    │   │
│ │ Buscar   │    │ Ofrecer  │   │
│ │ Profes.  │    │ Servicios│   │
│ │[Continuar]    │[Continuar]  │
│ └──────────┘    └──────────┘   │
│                                 │
│ ¿Ya tienes cuenta? [Inicia sesión]
└─────────────────────────────────┘
```

### **Pantalla 2: Búsqueda de Profesionales**
```
┌─────────────────────────────────┐
│  [≡ Menú]  Búsqueda  [👤 Perfil]│
├─────────────────────────────────┤
│ Especialidad: [Fontanero    ▼]  │
│ Ciudad:       [Madrid       ▼]  │
│ Distancia:    [10 km        ▼]  │
│ [🔍 Buscar]                     │
├─────────────────────────────────┤
│ ⭐ 4.8 | Carlos López           │
│ Fontanero • 10 años exp.        │
│ "Especialista en tuberías"      │
│ Distancia: 2.3 km               │
│ [Ver Detalles] [Contactar]      │
├─────────────────────────────────┤
│ ⭐ 4.5 | Ana Martínez           │
│ Fontanero • 8 años exp.         │
│ Distancia: 3.1 km               │
│ [Ver Detalles] [Contactar]      │
└─────────────────────────────────┘
```

### **Pantalla 3: Formulario de Contacto**
```
┌─────────────────────────────────┐
│ [◀ Atrás]  Contactar a Carlos   │
├─────────────────────────────────┤
│ Método de contacto:             │
│ ○ Llamada  ○ Visita  ○ Chat    │
│                                 │
│ Descripción:                    │
│ ┌─────────────────────────────┐ │
│ │Necesito revisar una fuga... │ │
│ └─────────────────────────────┘ │
│                                 │
│ Fecha preferida:                │
│ [2024-04-05] [14:00]           │
│                                 │
│ [Cancelar]  [Enviar Solicitud] │
└─────────────────────────────────┘
```

---

## ✅ CHECKLIST PRE-INICIO

### **Antes de empezar, verifica:**

- [ ] PostgreSQL 9.2+ instalado y corriendo
- [ ] SQL script `001_create_tables.sql` listo en `database/migrations/`
- [ ] Node.js 18+ instalado (para Angular)
- [ ] .NET 8 SDK instalado
- [ ] VS Code instalado con extensiones:
  - [ ] Angular Language Service
  - [ ] C# Dev Kit
  - [ ] PostgreSQL Explorer
  - [ ] Tailwind CSS IntelliSense
- [ ] Acceso a PostgreSQL (usuario y contraseña)
- [ ] Puerto 5432 disponible (PostgreSQL)
- [ ] Puerto 5001 disponible (Backend)
- [ ] Puerto 4200 disponible (Frontend)

---

## 🆘 TROUBLESHOOTING RÁPIDO

### **"No puedo conectarme a PostgreSQL"**
```powershell
# Verificar que PostgreSQL está corriendo
Get-Service PostgreSQL*

# Si no está corriendo, iniciar
Start-Service postgresql-x64-9.2
```

### **"Error: Database 'proservi_db' no existe"**
```sql
-- Crear la base de datos manualmente
CREATE DATABASE proservi_db;

-- Luego ejecutar el SQL script
\i 'c:/dev/ProServi/database/migrations/001_create_tables.sql'
```

### **"Error: Port 5001 already in use"**
```powershell
# Encontrar qué proceso está usando el puerto
Get-NetTCPConnection -LocalPort 5001

# Matar el proceso
Stop-Process -Id [PID] -Force
```

### **"Error: Port 4200 already in use"**
```powershell
ng serve --port 4300
```

---

## 📞 REFERENCIAS RÁPIDAS

| Tema | Archivo | Línea |
|------|---------|-------|
| Flujo usuario | FlujoExito.txt | 1-942 |
| BD Schema | ANALISIS_BASE_DATOS.md | Sección "Entidades" |
| API Endpoints | API_CONTRACTS.md | Sección "Autenticación" |
| Colores | SETUP_ANGULAR.md | Line ~50-80 |
| Rutas Angular | SETUP_ANGULAR.md | Sección "Paso 5" |
| Seguridad JWT | SETUP_DOTNET.md | Sección "Program.cs" |

---

## 🚀 PRÓXIMOS PASOS

### **Inmediato (Hoy)**
1. Ejecutar SQL script en PostgreSQL
2. Verificar que las 16 tablas fueron creadas
3. Revisar documentación de contratos API

### **Corto plazo (Esta semana)**
1. Crear proyecto .NET siguiendo SETUP_DOTNET.md
2. Crear proyecto Angular siguiendo SETUP_ANGULAR.md
3. Probar autenticación (login/register)

### **Mediano plazo (Esta mes)**
1. Implementar búsqueda de profesionales
2. Implementar sistema de contacto
3. Implementar sistema de pagos básico

### **Largo plazo (Próximos meses)**
1. Integración con Stripe/Mercado Pago
2. Sistema completo de notificaciones
3. Testing exhaustivo
4. Deployment en producción

---

## 📞 NOTAS IMPORTANTES

### **Seguridad**
⚠️ **CAMBIAR ANTES DE PRODUCCIÓN**:
- JWT SecretKey (appsettings.json)
- PostgreSQL password
- CORS policy (AllowAll solo para desarrollo)

### **Base de datos**
- PostgreSQL 9.2 es bastante antigua. Se recomienda actualizar a 13+ para:
  - PostGIS (geolocalización avanzada)
  - Mejor soporte de JSON
  - Mejor rendimiento
- SQL script es compatible con 9.2 pero sin PostGIS

### **Desarrollo Paralelo**
- Backend y Frontend pueden desarrollarse en paralelo
- Los contratos API están definidos para evitar cambios
- Si necesitas cambiar un endpoint, actualiza API_CONTRACTS.md primero

---

## 📊 MATRIZ DE RESPONSABILIDADES

| Componente | Backend | Frontend | Ambos |
|-----------|---------|----------|-------|
| Autenticación | IAuthService | AuthService | ✅ |
| Búsqueda | Endpoint GET | UI + Llamada | ✅ |
| Base de Datos | Migrations | - | ✅ |
| Validaciones | DTOs | Formularios | ✅ |
| JWT | Generar | Guardar/Enviar | ✅ |
| Errores | HTTP Status | UI Feedback | ✅ |

---

## 🎓 RECURSOS EDUCATIVOS

- [Angular Docs](https://angular.io/docs)
- [Tailwind CSS Docs](https://tailwindcss.com/docs)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [PostgreSQL 9.2 Docs](https://www.postgresql.org/docs/9.2)

---

**Versión**: 1.0 Maestra
**Fecha**: 28/03/2026
**Estado**: 🟢 Listo para Inicio
**Última Actualización**: [Hoy]

---

**¿Listo para empezar? 🚀**

1. Ejecuta el SQL script
2. Sigue SETUP_DOTNET.md
3. Sigue SETUP_ANGULAR.md
4. ¡Construye ProServi! 💪

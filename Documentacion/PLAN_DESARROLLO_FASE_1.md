# 🚀 PLAN DE DESARROLLO - FRONTEND + BACKEND (Fase 1)

## 📋 Objetivo
Crear un MVP funcional con login, registro y página principal usando Angular + Tailwind en el frontend, con backend .NET que expondrá APIs RESTful.

---

## 🎯 POR DÓNDE EMPEZAR

### **PRIORIDAD 1: ESTRUCTURA DEL PROYECTO FRONTEND**

#### Paso 1.1: Configurar Angular Project
```bash
# Estructura que necesitamos
frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── guards/          # Guards de autenticación
│   │   │   ├── interceptors/    # Interceptores HTTP
│   │   │   └── services/        # Servicios globales
│   │   │
│   │   ├── modules/
│   │   │   ├── auth/            # Login y Registro ⭐ PRIMERO
│   │   │   │   ├── components/
│   │   │   │   │   ├── login/
│   │   │   │   │   ├── register/
│   │   │   │   │   └── role-selector/
│   │   │   │   └── services/
│   │   │   │
│   │   │   ├── home/            # Página principal ⭐ SEGUNDO
│   │   │   │   ├── components/
│   │   │   │   │   ├── dashboard/
│   │   │   │   │   └── main/
│   │   │   │   └── services/
│   │   │   │
│   │   │   ├── shared/          # Componentes reutilizables
│   │   │   │   ├── components/
│   │   │   │   │   ├── navbar/
│   │   │   │   │   ├── footer/
│   │   │   │   │   ├── button/
│   │   │   │   │   └── modal/
│   │   │   │   └── pipes/
│   │   │   │
│   │   │   └── [otros módulos]
│   │   │
│   │   ├── app.component.ts
│   │   ├── app-routing.module.ts
│   │   └── app.module.ts
│   │
│   ├── styles/
│   │   ├── tailwind.css
│   │   ├── variables.css        # Variables de paleta de colores
│   │   └── global.css
│   │
│   └── assets/
│       ├── images/
│       ├── icons/
│       └── logo/
│
└── tailwind.config.js
```

#### Paso 1.2: Configurar Tailwind CSS
```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init
```

**tailwind.config.js**:
```javascript
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#f0f9ff',
          100: '#e0f2fe',
          500: '#0ea5e9',  // Sky blue (ejemplo)
          700: '#0369a1',
        },
        secondary: {
          500: '#8b5cf6',  // Purple (ejemplo)
          700: '#6d28d9',
        },
        accent: {
          500: '#ec4899',  // Pink (ejemplo)
        },
      },
    },
  },
  plugins: [],
}
```

---

## 📐 FASE 1: MÓDULO DE AUTENTICACIÓN (Login + Registro)

### **Componentes a crear:**

#### 1. **Role Selector** (Primera pantalla)
```
┌─────────────────────────────────────┐
│         ¡Bienvenido a ProServi!     │
│                                     │
│  [Logo]                             │
│                                     │
│  ¿Qué deseas hacer?                 │
│                                     │
│  ┌─────────────────────────────┐    │
│  │ 🔍 Buscar Profesionales     │    │
│  │ Encuentra expertos cercanos │    │
│  └─────────────────────────────┘    │
│                                     │
│  ┌─────────────────────────────┐    │
│  │ 🛠️ Ofrecer Mis Servicios    │    │
│  │ Regístrate como profesional │    │
│  └─────────────────────────────┘    │
└─────────────────────────────────────┘
```

#### 2. **Login** (Compartido)
```
┌─────────────────────────────────────┐
│         Iniciar Sesión              │
│                                     │
│  Email:                             │
│  [________________]                 │
│                                     │
│  Contraseña:                        │
│  [________________]                 │
│                                     │
│  [ ] Recuérdame                     │
│                                     │
│  [Iniciar Sesión]                   │
│                                     │
│  ¿No tienes cuenta?                 │
│  [Crear una ahora]                  │
└─────────────────────────────────────┘
```

#### 3. **Registro - Usuario Normal** (Cliente)
```
┌─────────────────────────────────────┐
│    Buscar Profesionales             │
│                                     │
│  Nombre Completo:                   │
│  [________________]                 │
│                                     │
│  Email:                             │
│  [________________]                 │
│                                     │
│  Teléfono:                          │
│  [________________]                 │
│                                     │
│  Ciudad:                            │
│  [Dropdown: Seleccionar ciudad]     │
│                                     │
│  Contraseña (mín 8 caracteres):     │
│  [________________]                 │
│                                     │
│  Confirmar Contraseña:              │
│  [________________]                 │
│                                     │
│  [ ] Foto de perfil (opcional)      │
│                                     │
│  [Crear Cuenta]                     │
│  [ ] Acepto términos y condiciones  │
└─────────────────────────────────────┘
```

#### 4. **Registro - Profesional**
```
┌─────────────────────────────────────┐
│    Ofrecer Mis Servicios            │
│                                     │
│  DATOS BÁSICOS                      │
│  Nombre Completo:                   │
│  [________________]                 │
│                                     │
│  Email:                             │
│  [________________]                 │
│                                     │
│  Teléfono:                          │
│  [________________]                 │
│                                     │
│  Ciudad:                            │
│  [Dropdown: Seleccionar ciudad]     │
│                                     │
│  [ ] Foto de perfil (obligatorio)   │
│                                     │
│  ───────────────────────────────    │
│  DATOS PROFESIONALES                │
│                                     │
│  Especialidad:                      │
│  [Dropdown: Plomería, Electricidad] │
│                                     │
│  Años de Experiencia:               │
│  [________________]                 │
│                                     │
│  Descripción de Servicios:          │
│  [________________text area]        │
│                                     │
│  Costo de Visita:                   │
│  [$ ________________]                │
│                                     │
│  ───────────────────────────────    │
│  DOCUMENTACIÓN                      │
│                                     │
│  Número Documento/Cédula:           │
│  [________________]                 │
│                                     │
│  [ ] Certificaciones (obligatorio)  │
│                                     │
│  [Crear Cuenta]                     │
│  [ ] Acepto términos                │
└─────────────────────────────────────┘
```

### **Flujo de Autenticación:**
```
┌─ App inicia
│
└─► ¿Usuario logueado?
    ├─ NO → Role Selector
    │       ├─ Buscar Profesionales → Register Customer
    │       └─ Ofrecer Servicios → Register Professional
    │
    └─ SÍ → Home/Dashboard (Paso siguiente)
```

---

## 🏠 FASE 2: PÁGINA PRINCIPAL (HOME/DASHBOARD)

### **Dashboard para USUARIO (Cliente):**
```
┌────────────────────────────────────────────────────┐
│  ProServi                            [👤] [☰]       │
├────────────────────────────────────────────────────┤
│                                                    │
│  ¡Hola, [Nombre]!                                  │
│                                                    │
│  ┌────────────────────────────────────────────┐   │
│  │  Buscar Profesionales                      │   │
│  │                                            │   │
│  │  Especialidad: [Dropdown]                  │   │
│  │  Ciudad: [Tu Ciudad - Editable]            │   │
│  │  Distancia: [Slider: 5km - 50km]           │   │
│  │                                            │   │
│  │  [🔍 Buscar]                               │   │
│  └────────────────────────────────────────────┘   │
│                                                    │
│  ┌────────────────────────────────────────────┐   │
│  │  Mis Solicitudes Activas                   │   │
│  │                                            │   │
│  │  Solicitud 1: Plomería - En espera         │   │
│  │  Solicitud 2: Electricidad - En progreso   │   │
│  │  Solicitud 3: Pintura - Completada         │   │
│  │                                            │   │
│  │  [Ver todas]                               │   │
│  └────────────────────────────────────────────┘   │
│                                                    │
│  ┌────────────────────────────────────────────┐   │
│  │  Profesionales Favoritos                   │   │
│  │                                            │   │
│  │  [Card 1] [Card 2] [Card 3]                │   │
│  │  [Ver más]                                 │   │
│  └────────────────────────────────────────────┘   │
│                                                    │
└────────────────────────────────────────────────────┘
```

### **Dashboard para PROFESIONAL:**
```
┌────────────────────────────────────────────────────┐
│  ProServi                            [👤] [☰]       │
├────────────────────────────────────────────────────┤
│                                                    │
│  Bienvenido, [Nombre]                             │
│  Perfil: ⏳ En espera de verificación             │
│                                                    │
│  ┌──────────────┬──────────────┬──────────────┐   │
│  │ Solicitudes  │ En Progreso  │ Completados  │   │
│  │      5       │      2       │      12      │   │
│  └──────────────┴──────────────┴──────────────┘   │
│                                                    │
│  ┌────────────────────────────────────────────┐   │
│  │  Nuevas Solicitudes                        │   │
│  │                                            │   │
│  │  🔔 [Usuario 1] - Plomería - 2h ago        │   │
│  │  🔔 [Usuario 2] - Electricidad - 4h ago    │   │
│  │  🔔 [Usuario 3] - Pintura - 1d ago         │   │
│  │                                            │   │
│  │  [Ver todas]                               │   │
│  └────────────────────────────────────────────┘   │
│                                                    │
│  ┌────────────────────────────────────────────┐   │
│  │  Proyectos en Progreso                     │   │
│  │                                            │   │
│  │  [Proyecto 1] - 60% completado             │   │
│  │  [Proyecto 2] - 30% completado             │   │
│  └────────────────────────────────────────────┘   │
│                                                    │
│  ┌────────────────────────────────────────────┐   │
│  │  Mi Calificación: ⭐⭐⭐⭐⭐ (4.8)           │   │
│  │  Proyectos Completados: 12                 │   │
│  │  Ingresos Totales: $3,450                  │   │
│  └────────────────────────────────────────────┘   │
│                                                    │
└────────────────────────────────────────────────────┘
```

---

## 🗄️ TABLAS DE BD NECESARIAS PARA FASE 1-2

### **Tabla: users**
```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    role ENUM('CUSTOMER', 'PROFESSIONAL'),
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(20) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    full_name VARCHAR(255) NOT NULL,
    profile_photo_url TEXT,
    status ENUM('ACTIVE', 'INACTIVE', 'SUSPENDED', 'PENDING_VERIFICATION'),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),
    email_verified BOOLEAN DEFAULT FALSE,
    phone_verified BOOLEAN DEFAULT FALSE,
    last_login TIMESTAMP
);
```

### **Tabla: customers**
```sql
CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    user_id INTEGER UNIQUE NOT NULL REFERENCES users(id),
    city VARCHAR(100) NOT NULL,
    latitude DECIMAL(10,8),
    longitude DECIMAL(11,8),
    address TEXT,
    bio TEXT,
    rating_avg DECIMAL(3,2) DEFAULT 0,
    total_projects INTEGER DEFAULT 0,
    total_spent DECIMAL(10,2) DEFAULT 0,
    created_at TIMESTAMP DEFAULT NOW()
);
```

### **Tabla: professionals**
```sql
CREATE TABLE professionals (
    id SERIAL PRIMARY KEY,
    user_id INTEGER UNIQUE NOT NULL REFERENCES users(id),
    profession VARCHAR(100) NOT NULL,
    sub_specialties TEXT[],
    years_experience INTEGER NOT NULL,
    city VARCHAR(100) NOT NULL,
    latitude DECIMAL(10,8),
    longitude DECIMAL(11,8),
    description_services TEXT NOT NULL,
    price_range_min DECIMAL(10,2),
    price_range_max DECIMAL(10,2),
    visit_cost DECIMAL(10,2) DEFAULT 0,
    is_verified BOOLEAN DEFAULT FALSE,
    verification_date TIMESTAMP,
    rating_avg DECIMAL(3,2) DEFAULT 0,
    total_projects INTEGER DEFAULT 0,
    total_earned DECIMAL(10,2) DEFAULT 0,
    document_number VARCHAR(50) UNIQUE NOT NULL,
    document_verified BOOLEAN DEFAULT FALSE,
    response_rate DECIMAL(5,2) DEFAULT 0,
    response_time_hours INTEGER DEFAULT 24,
    created_at TIMESTAMP DEFAULT NOW()
);
```

### **Tabla: specialties** (Maestro)
```sql
CREATE TABLE specialties (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    icon_url TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);
```

**Datos iniciales**:
```sql
INSERT INTO specialties (name, description) VALUES
('Plomería', 'Servicios de reparación y instalación de tuberías'),
('Electricidad', 'Trabajos eléctricos y reparación'),
('Pintura', 'Servicios de pintura interior y exterior'),
('Carpintería', 'Trabajos de carpintería y muebles'),
('Limpieza', 'Servicios de limpieza profesional'),
('Reparación General', 'Reparaciones varias en el hogar');
```

---

## 🔑 SERVICIOS ANGULAR NECESARIOS

### **1. AuthService** (Autenticación)
```typescript
// core/services/auth.service.ts

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterCustomerRequest {
  fullName: string;
  email: string;
  phone: string;
  city: string;
  password: string;
}

export interface RegisterProfessionalRequest {
  // ... todos los campos de profesional
}

export interface AuthResponse {
  token: string;
  user: UserData;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  login(credentials: LoginRequest): Observable<AuthResponse> { }
  registerCustomer(data: RegisterCustomerRequest): Observable<AuthResponse> { }
  registerProfessional(data: RegisterProfessionalRequest): Observable<AuthResponse> { }
  logout(): void { }
  isLoggedIn(): boolean { }
  getCurrentUser(): UserData | null { }
  getToken(): string | null { }
}
```

### **2. UserService** (Datos del usuario)
```typescript
// core/services/user.service.ts

@Injectable({ providedIn: 'root' })
export class UserService {
  getCurrentUser(): Observable<UserData> { }
  updateProfile(data: UserData): Observable<UserData> { }
  updateLocation(lat: number, lng: number): Observable<void> { }
  getMyRating(): Observable<RatingData> { }
}
```

### **3. ProfessionalService** (Búsqueda de profesionales)
```typescript
// modules/home/services/professional.service.ts

@Injectable({ providedIn: 'root' })
export class ProfessionalService {
  searchNearby(lat: number, lng: number, radius: number, specialty: string): Observable<Professional[]> { }
  getProfessionalDetail(id: number): Observable<Professional> { }
  getFavorites(): Observable<Professional[]> { }
  addToFavorites(id: number): Observable<void> { }
}
```

### **4. SpecialtyService** (Especialidades)
```typescript
// core/services/specialty.service.ts

@Injectable({ providedIn: 'root' })
export class SpecialtyService {
  getAll(): Observable<Specialty[]> { }
  getSubSpecialties(specialtyId: number): Observable<SubSpecialty[]> { }
}
```

---

## 🛣️ RUTAS DEL FRONTEND

```typescript
// app-routing.module.ts

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
    canActivate: [NotAuthGuard] // Solo si NO está logueado
  },
  {
    path: 'home',
    loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule),
    canActivate: [AuthGuard] // Solo si está logueado
  },
  {
    path: '',
    redirectTo: '/auth/role-selector',
    pathMatch: 'full'
  }
];
```

### **Rutas de Auth**
```
/auth/role-selector     → Elegir rol (Customer o Professional)
/auth/login             → Iniciar sesión
/auth/register/customer → Registro cliente
/auth/register/professional → Registro profesional
/auth/forgot-password   → Recuperar contraseña (future)
```

### **Rutas de Home**
```
/home                   → Dashboard principal
/home/search            → Buscar profesionales (se muestra en dashboard)
/home/requests          → Mis solicitudes
/home/projects          → Mis proyectos
/home/profile           → Mi perfil
```

---

## 📱 COMPONENTES A CREAR (Orden de Prioridad)

### **FASE 1 - AUTH**
- [ ] RoleSelector (Primera pantalla)
- [ ] LoginComponent
- [ ] RegisterCustomerComponent
- [ ] RegisterProfessionalComponent
- [ ] AuthLayoutComponent (Navbar + Footer para auth)

### **FASE 2 - HOME BÁSICA**
- [ ] DashboardComponent (Componente padre)
- [ ] MainNavbarComponent
- [ ] SearchBarComponent (Search de profesionales)
- [ ] CardComponent (Card reutilizable para profesionales)
- [ ] StatsComponent (Estadísticas del usuario)
- [ ] RequestListComponent (Lista de solicitudes)
- [ ] HomeLayoutComponent

### **FASE 3+ - DESPUÉS**
- [ ] Professional detail
- [ ] Contact request form
- [ ] Payment integration
- [ ] Reviews
- [ ] Project progress
- [ ] Admin panel

---

## 🎨 PALETA DE COLORES (Desde tu imagen)

**Nota**: Necesitarás revisar tu archivo `paleta-colores.jfif` y mapear:
```
Primary Color:     [Para botones principales, links activos]
Secondary Color:   [Para acentos, hover states]
Success Color:     [Para confirmaciones, estados positivos]
Warning Color:     [Para alertas, estados medios]
Danger Color:      [Para errores, eliminaciones]
Background:        [Color de fondo general]
Text:              [Color de texto principal]
```

---

## 🚀 ORDEN RECOMENDADO DE IMPLEMENTACIÓN

### **Semana 1: AUTH COMPLETO**
```
Día 1: Setup Angular + Tailwind
       └─ Crear estructura de carpetas
       └─ Instalar dependencias (angular, tailwind, jwt, etc)

Día 2-3: AuthService + Backend API
       └─ Crear endpoints básicos en .NET
       └─ Crear tablas: users, customers, professionals, specialties

Día 4-5: Componentes Auth
       └─ RoleSelector
       └─ Login
       └─ Register (Customer + Professional)

Día 6-7: Testing y ajustes
       └─ Probar flujo completo de autenticación
       └─ Validaciones en frontend
```

### **Semana 2: HOME BÁSICA**
```
Día 1-2: Setup Home Module
       └─ Crear layout
       └─ Crear navbar
       └─ Setup rutas

Día 3-4: Dashboard Customer
       └─ Search bar
       └─ Stats
       └─ Requests list

Día 5-6: Dashboard Professional
       └─ Stats adaptadas
       └─ Nueva solicitudes
       └─ Proyectos en progreso

Día 7: Testing y ajustes
```

---

## 📌 CHECKLIST ANTES DE EMPEZAR

- [ ] Revisar paleta de colores en `paleta-colores.jfif`
- [ ] Crear proyecto Angular nuevo
- [ ] Instalar Tailwind CSS
- [ ] Crear estructura de carpetas
- [ ] Instalar dependencias necesarias:
  ```bash
  npm install @angular/common @angular/forms rxjs
  npm install tailwindcss postcss autoprefixer
  npm install jwt-decode
  ```
- [ ] Crear base de datos PostgreSQL local
- [ ] Crear proyecto .NET (si no existe) o configurar backend

---

## ✅ SIGUIENTES PASOS

1. **Confirmar que entiendes la paleta de colores**
2. **Decidir si empezamos con Frontend primero o Backend simultáneamente**
3. **Crear proyecto Angular con estructura propuesta**
4. **Comenzar con RoleSelector component**

---

**Plan Actualizado**: 28/03/2026  
**Versión**: 1.0 (Plan completo para Fase 1-2)

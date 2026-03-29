# 📈 DASHBOARD DEL PROYECTO: PROSERVI

## 🎯 VISIÓN GENERAL

```
┌──────────────────────────────────────────────────────────────────────┐
│                                                                      │
│  PROSERVI: Marketplace de Profesionales Locales                     │
│  "Conecta usuarios con expertos en su zona"                         │
│                                                                      │
│  Usuarios objetivo: 2 tipos                                         │
│  ├── CUSTOMERS: Buscan profesionales (fontaneros, electricistas...)  │
│  └── PROFESSIONALS: Ofrecen servicios por su especialidad            │
│                                                                      │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 📊 MATRIZ DE COMPONENTES

### **Backend (.NET)**
```
┌─────────────────────────────────────────────────────────────┐
│ ProServi.Api (Web API - Puerto 5001)                        │
├─────────────────────────────────────────────────────────────┤
│ Controllers:                                                 │
│ ├── AuthController (/api/auth)                              │
│ │   └── POST login, register/customer, register/professional│
│ ├── ProfessionalsController (/api/professionals)            │
│ │   └── GET search, {id}                                    │
│ ├── ContactRequestsController (/api/contact-requests)       │
│ │   └── POST, GET, PUT (accept/reject)                      │
│ ├── PaymentsController (/api/payments)                      │
│ │   └── POST create, GET list, GET {id}                     │
│ └── UsersController (/api/users)                            │
│     └── GET profile, PUT profile                            │
└─────────────────────────────────────────────────────────────┘
        ↓                                              ↑
        │ Entity Framework Core (ORM)                 │
        │ + Repositories Pattern                      │
        │                                              │
        ↓                                              │
┌─────────────────────────────────────────────────────────────┐
│ ProServiDbContext (16 tablas en PostgreSQL)                │
├─────────────────────────────────────────────────────────────┤
│ Tables:                                                      │
│ ├── users (id, email, password_hash, role)                  │
│ ├── customers (user_id, city, address, lat, lng)            │
│ ├── professionals (user_id, specialty_id, years_exp)        │
│ ├── specialties (id, name)                                  │
│ ├── contact_requests (customer_id, professional_id, status) │
│ ├── budgets (contact_request_id, amount)                    │
│ ├── projects (contact_request_id, status)                   │
│ ├── payments (amount, status, transaction_id)               │
│ ├── reviews (professional_id, rating, comment)              │
│ ├── notifications (user_id, type, read_at)                  │
│ ├── saved_cards (user_id, card_token)                       │
│ ├── disputes (payment_id, status, reason)                   │
│ └── ... (10 tablas más)                                     │
└─────────────────────────────────────────────────────────────┘
        ↓
┌─────────────────────────────────────────────────────────────┐
│ PostgreSQL 9.2 (Host: localhost:5432)                       │
│ Database: proservi_db                                       │
└─────────────────────────────────────────────────────────────┘
```

### **Frontend (Angular)**
```
┌──────────────────────────────────────────────────────────────┐
│ Angular App (Puerto 4200)                                    │
├──────────────────────────────────────────────────────────────┤
│ Modules:                                                      │
│ ├── AuthModule                                               │
│ │   ├── RoleSelectorComponent                                │
│ │   ├── LoginComponent                                       │
│ │   ├── RegisterCustomerComponent                            │
│ │   └── RegisterProfessionalComponent                        │
│ │                                                             │
│ ├── HomeModule                                               │
│ │   ├── DashboardComponent                                   │
│ │   ├── SearchComponent (con ProfessionalCardComponent)      │
│ │   ├── ProfessionalDetailComponent                          │
│ │   ├── ContactFormComponent                                 │
│ │   └── NavbarComponent                                      │
│ │                                                             │
│ └── SharedModule                                             │
│     ├── ButtonComponent                                      │
│     ├── CardComponent                                        │
│     ├── ModalComponent                                       │
│     └── FormFieldComponent                                   │
│                                                              │
│ Services (Core):                                             │
│ ├── AuthService (login, register, logout)                    │
│ ├── UserService (profile, update)                            │
│ ├── ProfessionalService (search, getDetail)                  │
│ ├── ContactRequestService (create, list)                     │
│ └── PaymentService (create, list)                            │
│                                                              │
│ Interceptors & Guards:                                       │
│ ├── AuthInterceptor (añade JWT a headers)                    │
│ ├── AuthGuard (protege rutas autenticadas)                   │
│ └── NotAuthGuard (protege login/register)                    │
│                                                              │
│ Styles:                                                       │
│ └── Tailwind CSS (configurado en tailwind.config.js)         │
└──────────────────────────────────────────────────────────────┘
        ↓                                            ↑
        │ HTTP Calls (AuthInterceptor agrega JWT)   │
        │                                            │
        ↓                                            │
┌──────────────────────────────────────────────────────────────┐
│ Backend API (https://localhost:5001/api/...)                │
└──────────────────────────────────────────────────────────────┘
```

---

## 🔄 FLUJO DE USUARIO

### **Fase 1: REGISTRO**
```
RoleSelector Component
    ↓
¿Quieres buscar o ofrecer servicios?
    ├─→ Buscar Profesionales (CUSTOMER)
    │   ↓
    │   RegisterCustomerComponent
    │   Inputs: fullName, email, phone, city, password
    │   ↓
    │   POST /api/auth/register/customer
    │   ↓
    │   ✅ Usuario creado en tabla 'users' y 'customers'
    │   ✅ JWT token retornado
    │   ↓
    │   Redirige a /home (Dashboard)
    │
    └─→ Ofrecer Servicios (PROFESSIONAL)
        ↓
        RegisterProfessionalComponent
        Inputs: fullName, email, phone, city, profession, 
                yearsExperience, description, documentNumber, password
        ↓
        POST /api/auth/register/professional
        ↓
        ✅ Usuario creado en tabla 'users' y 'professionals'
        ✅ JWT token retornado
        ↓
        Redirige a /home (Dashboard)
```

### **Fase 2: LOGIN**
```
LoginComponent
    ↓
Inputs: email, password
    ↓
POST /api/auth/login
    ↓
✅ JWT token retornado
✅ Token guardado en localStorage
✅ CurrentUser$ actualizado en AuthService
    ↓
Redirige a /home (Dashboard)
```

### **Fase 3: BÚSQUEDA (Customer)**
```
DashboardComponent (Customer)
    ↓
SearchBarComponent
    ├─ Selector de especialidad (GET /api/specialties)
    ├─ Input de ciudad
    └─ Filtros opcionales (distancia, etc)
    ↓
GET /api/professionals/search?specialtyId=X&city=Y
    ↓
Renderiza ProfessionalCardComponent (lista de resultados)
    ├─ Nombre, especialidad
    ├─ Rating y reviews
    ├─ Distancia
    └─ Botón "Ver Detalles" o "Contactar"
    ↓
Click en profesional → ProfessionalDetailComponent
    ├─ GET /api/professionals/{id}
    ├─ Muestra información completa
    ├─ Lista de reviews
    └─ Botón "Solicitar Contacto"
```

### **Fase 4: SOLICITUD DE CONTACTO**
```
ContactFormComponent
    ↓
Inputs:
    ├─ Método: Llamada, Visita, Chat, Videollamada
    ├─ Descripción del problema
    ├─ Fecha y hora preferida
    └─ Dirección (si es visita)
    ↓
POST /api/contact-requests
{
    professionalId: X,
    contactMethod: "VISIT",
    description: "...",
    preferredDateTime: "2024-04-05T14:00:00",
    address: "..."
}
    ↓
✅ Solicitud guardada en tabla 'contact_requests'
✅ Estado inicial: PENDING
✅ Profesional recibe notificación (Email + WhatsApp + App)
    ↓
Professional verá solicitud en su dashboard
    ├─ Aceptar (status → ACCEPTED)
    │   ├─ Cliente notificado (Email + App)
    │   └─ Ahora pueden comunicarse
    └─ Rechazar (status → REJECTED)
        └─ Cliente notificado
```

### **Fase 5: PRESUPUESTO Y PAGO**
```
Profesional acepta solicitud
    ↓
Crea presupuesto (Budget)
    ↓
POST /api/payments/create
{
    contactRequestId: X,
    amount: 150.00,
    description: "Reparación de tubería",
    paymentMethod: "CARD"
}
    ↓
✅ Payment creado con status PENDING
✅ EscrowStatus: PENDING (dinero NO transferido aún)
✅ Cliente notificado (Email + App)
    ↓
Cliente ve presupuesto en su dashboard
    ├─ Aceptar → Realiza pago
    │   ├─ Dinero en ESCROW (retenido)
    │   ├─ Profesional inicia trabajo
    │   ├─ Estado del pago: IN_ESCROW
    │   └─ Payment Status: COMPLETED (pero sin liberar)
    └─ Rechazar → Cancela
        └─ Vuelta a PENDING
```

### **Fase 6: PROYECTO EN CURSO**
```
Project creado cuando pago está en escrow
    ↓
Profesional realiza trabajo
    ↓
Cliente verifica que está satisfecho
    ↓
Cliente marca proyecto como completado
    ├─ Dinero se libera del escrow
    ├─ Payment.EscrowStatus → RELEASED
    ├─ Dinero transferido a profesional
    └─ Notificaciones enviadas a ambos
```

### **Fase 7: RESEÑA Y VALORACIÓN**
```
Proyecto completado
    ↓
Cliente ve opción "Dejar Reseña"
    ↓
POST /api/reviews
{
    projectId: X,
    professionalId: Y,
    rating: 5,
    comment: "Excelente trabajo",
    wouldRecommend: true
}
    ↓
✅ Review guardada
✅ Rating promedio del profesional actualizado
✅ Profesional notificado (Email)
    ↓
Profesional puede responder a la reseña (futuro)
```

---

## 🗺️ MAPA DE RUTAS (Routing)

```
/
├── /auth (Guard: NotAuthGuard - solo no autenticados)
│   ├── /auth/role-selector (Primera pantalla)
│   ├── /auth/login
│   ├── /auth/register/customer
│   └── /auth/register/professional
│
├── /home (Guard: AuthGuard - requiere autenticación)
│   ├── /home/dashboard (por defecto después de login)
│   ├── /home/search
│   ├── /home/professional/:id (detalle profesional)
│   ├── /home/requests (solicitudes enviadas/recibidas)
│   ├── /home/payments
│   ├── /home/projects (proyectos en curso)
│   └── /home/profile (mi perfil)
│
└── ** (wildcard - redirige a /auth/role-selector)
```

---

## 💾 ESTRUCTURA DE DATOS

### **Entidades Principales**

```
┌─────────────────────┐
│      users          │
├─────────────────────┤
│ id (PK)             │
│ email (UNIQUE)      │
│ password_hash       │
│ fullName            │
│ phone               │
│ role (ENUM)         │
│ createdAt           │
└─────────────────────┘
         ↓
    ┌────┴────┐
    ↓         ↓
┌─────────┐ ┌──────────────┐
│customers│ │professionals │
├─────────┤ ├──────────────┤
│user_id  │ │user_id       │
│city     │ │specialty_id  │
│address  │ │yearsExperience
│lat/lng  │ │docNumber     │
└─────────┘ └──────────────┘
    ↓              ↓
    │         ┌────┴──────────────┐
    │         ↓                   ↓
    │    ┌──────────────┐  ┌─────────┐
    │    │contact_      │  │certifi- │
    │    │requests      │  │cations  │
    │    ├──────────────┤  ├─────────┤
    │    │customer_id   │  │prof_id  │
    │    │professional_ │  │name     │
    │    │id            │  │issued_at│
    │    │status (ENUM) │  └─────────┘
    │    └──────────────┘
    │         ↓
    │    ┌─────────────┐
    │    │   budgets   │
    │    ├─────────────┤
    │    │contact_req_ │
    │    │id           │
    │    │amount       │
    │    │description  │
    │    └─────────────┘
    │         ↓
    │    ┌──────────┐
    │    │ projects │
    │    ├──────────┤
    │    │budget_id │
    │    │status    │
    │    └──────────┘
    │         ↓
    │    ┌──────────┐
    │    │ payments │
    │    ├──────────┤
    │    │project_id│
    │    │amount    │
    │    │status    │
    │    │escrow    │
    │    └──────────┘
    │         ↓
    │    ┌──────────┐
    │    │ reviews  │
    │    ├──────────┤
    │    │project_id│
    │    │prof_id   │
    │    │rating    │
    │    │comment   │
    │    └──────────┘
    │
    └──→ (También relacionado con: notifications, preferences, etc)
```

---

## 📱 CASOS DE USO PRINCIPALES

### **Caso 1: Cliente busca fontanero**
```
1. Login → Dashboard
2. Selecciona especialidad "Fontanero"
3. Selecciona ciudad "Madrid"
4. Ve lista de fontaneros con ratings
5. Hace clic en "Carlos López" (4.8 ⭐)
6. Ve detalles: experiencia, certificaciones, reviews
7. Click "Solicitar Contacto"
8. Elige "Visita presencial" para el 5 de abril a las 14:00
9. Se envía solicitud
10. Carlos recibe notificación (Email + WhatsApp + App)
11. Carlos acepta
12. Cliente recibe confirmación
13. Coordinan por teléfono
14. Carlos visita y realiza trabajo
15. Cliente aprueba trabajo → Dinero se libera
16. Cliente deja 5 ⭐ + comentario
```

### **Caso 2: Profesional recibe solicitud**
```
1. Login → Dashboard (Professional)
2. Ve "Tienes 3 solicitudes nuevas"
3. Lee solicitud de María: "Fuga de agua en cocina"
4. Revisa perfil de María
5. Ve que es cliente nuevo (sin historial)
6. Decide aceptar → Click "Aceptar"
7. Crea presupuesto: €150 por reparación
8. María recibe notificación
9. María paga → Dinero en escrow
10. Carlos realiza trabajo (2 horas)
11. María verifica que está bien
12. Marca proyecto como completado
13. Dinero transferido a Carlos (€150)
14. María deja reseña 5⭐
15. Rating de Carlos sube a 4.9⭐
```

---

## 🔐 SEGURIDAD

```
┌──────────────────────────────┐
│   Cliente hace request       │
└──────────┬───────────────────┘
           │
           ↓
┌──────────────────────────────┐
│ AuthInterceptor              │
│ Agrega: Authorization header │
│ Header: "Bearer JWT_TOKEN"   │
└──────────┬───────────────────┘
           │
           ↓
┌──────────────────────────────┐
│ Servidor Backend             │
│ Valida JWT signature         │
│ Verifica token no expirado   │
│ Extrae claims (id, email)    │
└──────────┬───────────────────┘
           │
      ├────┴─────────┐
      │              │
      ↓              ↓
   ✅ Valid    ❌ Invalid
      │              │
      ↓              ↓
  Ejecuta      401 Unauthorized
  endpoint     Redirige a login
```

---

## 📊 VOLUMEN DE DATOS ESPERADO (FASE 1)

```
Usuarios:
├── Customers: 100-500
├── Professionals: 50-200
└── Total: 150-700

Solicitudes de Contacto:
├── Por día: 10-50
├── Activas: 20-100
└── Total: 100-500

Transacciones:
├── Pagos completados: 20-100
├── Monto promedio: €100-200
└── Ingresos mensuales: €2,000-20,000

Reseñas:
├── Total: 50-200
├── Rating promedio: 4.3-4.8 ⭐
└── Comentarios: Variado
```

---

## ⏱️ TIMELINE ESTIMADO

```
Semana 1 (Configuración)
├── Día 1-2: Setup Base de Datos ✅
├── Día 2-3: Setup Backend
├── Día 3-4: Setup Frontend
└── Día 5: Integración Basic Auth

Semana 2 (Búsqueda)
├── Día 1-2: Endpoint Profesionales
├── Día 2-3: UI Búsqueda
├── Día 4-5: Integración

Semana 3 (Contacto)
├── Día 1-2: Endpoint Contacto
├── Día 2-3: UI Contacto
└── Día 4-5: Testing

Semana 4 (Pagos Básico)
├── Día 1-2: Endpoint Pagos
├── Día 2-3: UI Pagos
├── Día 4: Integración
└── Día 5: Testing

TOTAL: 4 semanas para MVP
```

---

## 🎯 MÉTRICAS DE ÉXITO

```
✅ MVP Completado cuando:
   ├── Registro: ✅ Clientes y Profesionales pueden registrarse
   ├── Búsqueda: ✅ Clientes pueden buscar profesionales
   ├── Contacto: ✅ Pueden crear solicitudes
   ├── Pagos: ✅ Pueden realizar pagos en escrow
   ├── Reseñas: ✅ Pueden dejar calificaciones
   ├── Notificaciones: ✅ Email funciona
   ├── JWT: ✅ Seguridad implementada
   └── UI: ✅ Responsive y usable

📈 KPIs a Monitorear:
   ├── Usuarios registrados por día
   ├── Solicitudes de contacto por día
   ├── Tasa de aceptación de solicitudes
   ├── Monto promedio de transacción
   ├── Rating promedio de profesionales
   ├── Tasa de reseñas completadas
   └── Tiempo de respuesta promedio
```

---

## 🚀 SIGUIENTE DESPUÉS DEL MVP

```
Fase 2: Mejoras
├── Integración real de pagos (Stripe/Mercado Pago)
├── Sistema de chat entre usuario y profesional
├── Geolocalización avanzada (PostGIS)
├── Favoritos/Seguir profesionales
├── Historial de búsqueda
└── Filtros avanzados (precio, disponibilidad)

Fase 3: Profesionalización
├── Panel de admin
├── Verificación de identidad
├── Sistema de penalización
├── Soporte por ticket
├── Análisis de datos
└── Marketing tools

Fase 4: Expansión
├── App móvil (React Native)
├── Multi-idioma
├── Multi-moneda
├── Integración con redes sociales
├── Recomendaciones IA
└── Sistema de referidos
```

---

**Dashboard v1.0**
**Creado**: 28/03/2026
**Estado**: 🟢 En Ejecución

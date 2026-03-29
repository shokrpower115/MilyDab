# 📊 ANÁLISIS PARA ESTRUCTURA DE BASE DE DATOS - ProServi

## 🎯 Objetivo
Diseñar una base de datos relacional en **PostgreSQL 13+** con soporte para **geolocalización (PostGIS)** que soporte el flujo completo de ProServi.

---

## 📋 Tabla de Contenidos
1. [Entidades Principales](#entidades-principales)
2. [Relaciones y Cardinalidad](#relaciones-y-cardinalidad)
3. [Extensiones de PostgreSQL](#extensiones-de-postgresql)
4. [Índices Requeridos](#índices-requeridos)
5. [Restricciones y Validaciones](#restricciones-y-validaciones)
6. [Diagrama Entidad-Relación](#diagrama-entidad-relación)

---

## 🏗️ ENTIDADES PRINCIPALES

### 1. **users** (Tabla base para usuarios)
Almacena datos comunes a todos los usuarios del sistema.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ role                  (ENUM: 'CUSTOMER', 'PROFESSIONAL')
├─ email                 (VARCHAR 255, UNIQUE, NOT NULL)
├─ phone                 (VARCHAR 20, UNIQUE, NOT NULL)
├─ password_hash         (VARCHAR 255, NOT NULL)
├─ full_name             (VARCHAR 255, NOT NULL)
├─ profile_photo_url     (TEXT, NULLABLE)
├─ status                (ENUM: 'ACTIVE', 'INACTIVE', 'SUSPENDED', 'PENDING_VERIFICATION')
├─ created_at            (TIMESTAMP DEFAULT NOW())
├─ updated_at            (TIMESTAMP DEFAULT NOW())
├─ email_verified        (BOOLEAN DEFAULT FALSE)
├─ phone_verified        (BOOLEAN DEFAULT FALSE)
└─ last_login            (TIMESTAMP, NULLABLE)

ÍNDICES:
├─ idx_users_email (UNIQUE)
├─ idx_users_phone (UNIQUE)
├─ idx_users_role_status (role, status)
└─ idx_users_created_at
```

**Propósito**: Base compartida para clientes y profesionales

---

### 2. **customers** (Tabla específica para clientes/usuarios normales)
Datos específicos del cliente que busca servicios.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ user_id              (INTEGER, FOREIGN KEY → users.id, NOT NULL, UNIQUE)
├─ city                 (VARCHAR 100, NOT NULL) → Determina búsquedas locales
├─ latitude             (DECIMAL(10,8), NULLABLE) → Para geolocalización
├─ longitude            (DECIMAL(11,8), NULLABLE) → Para geolocalización
├─ address              (TEXT, NULLABLE) → Dirección completa
├─ preferred_contact    (ENUM: 'APP', 'EMAIL', 'WHATSAPP', NULLABLE)
├─ bio                  (TEXT, NULLABLE)
├─ rating_avg           (DECIMAL(3,2) DEFAULT 0) → Rating promedio como cliente
├─ total_projects       (INTEGER DEFAULT 0)
├─ total_spent          (DECIMAL(10,2) DEFAULT 0)
└─ created_at           (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_customers_user_id (UNIQUE)
├─ idx_customers_city
├─ idx_customers_location (latitude, longitude) → Para búsquedas de distancia
└─ idx_customers_rating_avg
```

**Propósito**: Datos específicos del cliente

---

### 3. **professionals** (Tabla específica para profesionales)
Datos específicos del profesional que ofrece servicios.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ user_id              (INTEGER, FOREIGN KEY → users.id, NOT NULL, UNIQUE)
├─ profession           (VARCHAR 100, NOT NULL) → Ejm: Plomería, Electricidad
├─ sub_specialties      (TEXT ARRAY, NULLABLE) → Ejm: ['Reparación tuberías', 'Instalación']
├─ years_experience     (INTEGER, NOT NULL)
├─ city                 (VARCHAR 100, NOT NULL)
├─ latitude             (DECIMAL(10,8), NULLABLE) → Ubicación del profesional
├─ longitude            (DECIMAL(11,8), NULLABLE)
├─ description_services (TEXT, NOT NULL)
├─ price_range_min      (DECIMAL(10,2), NULLABLE)
├─ price_range_max      (DECIMAL(10,2), NULLABLE)
├─ business_hours_start (TIME, NULLABLE) → Hora inicio horario laboral
├─ business_hours_end   (TIME, NULLABLE) → Hora fin horario laboral
├─ visit_cost           (DECIMAL(10,2), DEFAULT 0) → Costo por desplazamiento
├─ is_verified          (BOOLEAN DEFAULT FALSE)
├─ verification_date    (TIMESTAMP, NULLABLE)
├─ rating_avg           (DECIMAL(3,2) DEFAULT 0) → Rating promedio como profesional
├─ total_projects       (INTEGER DEFAULT 0)
├─ total_earned         (DECIMAL(10,2) DEFAULT 0)
├─ document_number      (VARCHAR 50, UNIQUE, NOT NULL)
├─ document_verified    (BOOLEAN DEFAULT FALSE)
├─ response_rate        (DECIMAL(5,2) DEFAULT 0) → % de solicitudes que responde
├─ response_time_hours  (INTEGER DEFAULT 24) → Tiempo promedio de respuesta
└─ created_at           (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_professionals_user_id (UNIQUE)
├─ idx_professionals_profession
├─ idx_professionals_city
├─ idx_professionals_location (latitude, longitude) → Para búsquedas cercanas
├─ idx_professionals_rating_avg
├─ idx_professionals_is_verified
├─ idx_professionals_document_number
└─ idx_professionals_sub_specialties (GIN para array)
```

**Propósito**: Datos específicos del profesional

---

### 4. **contact_requests** (Solicitudes de contacto)
Almacena las solicitudes que hace un usuario a un profesional.

```sql
COLUMNAS:
├─ id                        (SERIAL PRIMARY KEY)
├─ customer_id               (INTEGER, FOREIGN KEY → customers.id, NOT NULL)
├─ professional_id           (INTEGER, FOREIGN KEY → professionals.id, NOT NULL)
├─ contact_method            (ENUM: 'VISIT', 'CALL', 'QUOTE', 'VIDEO_CALL', NOT NULL)
├─ title                     (VARCHAR 255, NOT NULL) → Título del trabajo
├─ description               (TEXT, NOT NULL, MIN 50 chars) → Descripción detallada
├─ urgency                   (ENUM: 'LOW', 'MEDIUM', 'HIGH', DEFAULT 'MEDIUM')
├─ photos_urls              (TEXT ARRAY, NULLABLE) → URLs de fotos del problema
├─ customer_estimated_budget (DECIMAL(10,2), NULLABLE) → Presupuesto estimado del cliente
├─ available_from            (TIMESTAMP, NULLABLE) → Disponibilidad desde
├─ available_to              (TIMESTAMP, NULLABLE) → Disponibilidad hasta
├─ status                    (ENUM: 'PENDING', 'ACCEPTED', 'REJECTED', 'CANCELLED', NOT NULL)
├─ created_at                (TIMESTAMP DEFAULT NOW())
└─ updated_at                (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_contact_requests_customer_id
├─ idx_contact_requests_professional_id
├─ idx_contact_requests_status
├─ idx_contact_requests_created_at
└─ idx_contact_requests_customer_professional (customer_id, professional_id)
```

**Propósito**: Registro de solicitudes de contacto

---

### 5. **budgets** (Presupuestos)
Almacena presupuestos generales y detalles de costos.

```sql
COLUMNAS:
├─ id                        (SERIAL PRIMARY KEY)
├─ contact_request_id        (INTEGER, FOREIGN KEY → contact_requests.id, NOT NULL, UNIQUE)
├─ customer_id               (INTEGER, FOREIGN KEY → customers.id, NOT NULL)
├─ professional_id           (INTEGER, FOREIGN KEY → professionals.id, NOT NULL)
├─ visit_cost                (DECIMAL(10,2), DEFAULT 0) → Costo de visita (si aplica)
├─ estimated_work_cost       (DECIMAL(10,2), NULLABLE) → Estimado del trabajo
├─ platform_commission_rate  (DECIMAL(5,2) DEFAULT 8) → % de comisión (8%)
├─ total_estimated           (DECIMAL(10,2), NULLABLE) → Total estimado
├─ status                    (ENUM: 'PENDING', 'ACCEPTED', 'REJECTED', 'EXPIRED', DEFAULT 'PENDING')
├─ valid_until               (TIMESTAMP, NOT NULL) → Válido por 7 días
├─ payment_required_upfront  (BOOLEAN DEFAULT FALSE) → Si se cobra costo de visita
├─ created_at                (TIMESTAMP DEFAULT NOW())
└─ updated_at                (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_budgets_contact_request_id (UNIQUE)
├─ idx_budgets_customer_id
├─ idx_budgets_professional_id
├─ idx_budgets_status
└─ idx_budgets_valid_until
```

**Propósito**: Presupuestos antes de confirmar trabajo

---

### 6. **projects** (Proyectos/Trabajos)
Almacena información del trabajo realizado después de aceptar la solicitud.

```sql
COLUMNAS:
├─ id                        (SERIAL PRIMARY KEY)
├─ contact_request_id        (INTEGER, FOREIGN KEY → contact_requests.id, NOT NULL, UNIQUE)
├─ customer_id               (INTEGER, FOREIGN KEY → customers.id, NOT NULL)
├─ professional_id           (INTEGER, FOREIGN KEY → professionals.id, NOT NULL)
├─ contact_method            (ENUM: 'VISIT', 'CALL', 'QUOTE', 'VIDEO_CALL')
├─ status                    (ENUM: 'ACTIVE', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED', PAID, NOT NULL)
├─ work_title                (VARCHAR 255, NOT NULL)
├─ work_description          (TEXT, NOT NULL)
├─ estimated_start_date      (TIMESTAMP, NULLABLE)
├─ estimated_end_date        (TIMESTAMP, NULLABLE)
├─ actual_start_date         (TIMESTAMP, NULLABLE)
├─ actual_end_date           (TIMESTAMP, NULLABLE)
├─ location_address          (TEXT, NULLABLE) → Dirección del trabajo
├─ location_latitude         (DECIMAL(10,8), NULLABLE)
├─ location_longitude        (DECIMAL(11,8), NULLABLE)
├─ customer_confirmed        (BOOLEAN DEFAULT FALSE) → Cliente confirma terminación
├─ professional_confirmed    (BOOLEAN DEFAULT FALSE) → Profesional confirma terminación
├─ payment_status            (ENUM: 'NOT_INITIATED', 'IN_ESCROW', 'COMPLETED', 'DISPUTED')
├─ created_at                (TIMESTAMP DEFAULT NOW())
├─ updated_at                (TIMESTAMP DEFAULT NOW())
└─ completed_at              (TIMESTAMP, NULLABLE)

ÍNDICES:
├─ idx_projects_contact_request_id (UNIQUE)
├─ idx_projects_customer_id
├─ idx_projects_professional_id
├─ idx_projects_status
├─ idx_projects_completed_at
└─ idx_projects_location (latitude, longitude)
```

**Propósito**: Registro del trabajo en ejecución

---

### 7. **project_photos** (Fotos del proyecto)
Almacena URLs de fotos del progreso del trabajo.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ project_id            (INTEGER, FOREIGN KEY → projects.id, NOT NULL)
├─ uploaded_by           (ENUM: 'CUSTOMER', 'PROFESSIONAL', NOT NULL)
├─ photo_url             (TEXT, NOT NULL)
├─ description           (TEXT, NULLABLE)
├─ uploaded_at           (TIMESTAMP DEFAULT NOW())
└─ order                 (INTEGER DEFAULT 0) → Para ordenar fotos

ÍNDICES:
├─ idx_project_photos_project_id
└─ idx_project_photos_uploaded_at
```

**Propósito**: Galería de fotos del proyecto

---

### 8. **payments** (Pagos)
Almacena historial de transacciones de pago.

```sql
COLUMNAS:
├─ id                        (SERIAL PRIMARY KEY)
├─ project_id                (INTEGER, FOREIGN KEY → projects.id, NOT NULL)
├─ payer_user_id             (INTEGER, FOREIGN KEY → users.id, NOT NULL) → Usuario que paga
├─ receiver_user_id          (INTEGER, FOREIGN KEY → users.id, NOT NULL) → Profesional que recibe
├─ payment_type              (ENUM: 'VISIT_COST', 'WORK_COST', 'FULL_PROJECT', NOT NULL)
├─ amount                    (DECIMAL(10,2), NOT NULL)
├─ platform_commission       (DECIMAL(10,2) DEFAULT 0) → 8% comisión
├─ net_amount                (DECIMAL(10,2), NOT NULL) → Monto neto profesional
├─ payment_method            (ENUM: 'CARD', 'BANK_TRANSFER', NOT NULL)
├─ external_transaction_id   (VARCHAR 255, NULLABLE) → ID de Stripe/Mercado Pago
├─ status                    (ENUM: 'PENDING', 'COMPLETED', 'FAILED', 'REFUNDED', NOT NULL)
├─ escrow_status             (ENUM: 'IN_ESCROW', 'RELEASED', 'REFUNDED', NULLABLE)
├─ escrow_release_date       (TIMESTAMP, NULLABLE) → Cuándo se libera el dinero
├─ receipt_url               (TEXT, NULLABLE) → URL del recibo
├─ invoice_url               (TEXT, NULLABLE) → URL de la factura
├─ created_at                (TIMESTAMP DEFAULT NOW())
├─ updated_at                (TIMESTAMP DEFAULT NOW())
└─ completed_at              (TIMESTAMP, NULLABLE)

ÍNDICES:
├─ idx_payments_project_id
├─ idx_payments_payer_user_id
├─ idx_payments_receiver_user_id
├─ idx_payments_status
├─ idx_payments_escrow_release_date
└─ idx_payments_created_at
```

**Propósito**: Registro de pagos y transacciones

---

### 9. **saved_cards** (Tarjetas guardadas)
Almacena referencias a tarjetas de crédito guardadas (NO los datos sensibles).

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ user_id               (INTEGER, FOREIGN KEY → users.id, NOT NULL)
├─ card_token            (VARCHAR 255, NOT NULL) → Token de Stripe/Mercado Pago
├─ card_last_four        (VARCHAR 4, NOT NULL) → Últimos 4 dígitos
├─ card_brand            (VARCHAR 50, NULLABLE) → VISA, MASTERCARD, etc
├─ expiry_month          (INTEGER, NULLABLE)
├─ expiry_year           (INTEGER, NULLABLE)
├─ is_default            (BOOLEAN DEFAULT FALSE)
├─ is_verified           (BOOLEAN DEFAULT FALSE)
├─ verification_amount   (DECIMAL(10,2), NULLABLE) → Monto de verificación ($1)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ updated_at            (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_saved_cards_user_id
├─ idx_saved_cards_is_default
└─ idx_saved_cards_created_at
```

**Propósito**: Referencias a tarjetas guardadas (seguro, sin datos sensibles)

---

### 10. **disputes** (Disputas)
Almacena reportes de problemas/disputas entre usuario y profesional.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ project_id            (INTEGER, FOREIGN KEY → projects.id, NOT NULL)
├─ reported_by_user_id   (INTEGER, FOREIGN KEY → users.id, NOT NULL)
├─ reported_against_user_id (INTEGER, FOREIGN KEY → users.id, NOT NULL)
├─ reason                (ENUM: 'INCOMPLETE_WORK', 'QUALITY_ISSUE', 'PRICE_MISMATCH', 'NO_CONTACT', 'FALSE_INFORMATION', OTHER)
├─ description           (TEXT, NOT NULL)
├─ evidence_photos_urls  (TEXT ARRAY, NULLABLE)
├─ status                (ENUM: 'OPEN', 'IN_REVIEW', 'RESOLVED', 'APPEALED', DEFAULT 'OPEN')
├─ resolution_type       (ENUM: 'REFUND', 'HOLD_PAYMENT', 'REJECT_DISPUTE', NULLABLE)
├─ resolution_notes       (TEXT, NULLABLE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
├─ updated_at            (TIMESTAMP DEFAULT NOW())
└─ resolved_at           (TIMESTAMP, NULLABLE)

ÍNDICES:
├─ idx_disputes_project_id
├─ idx_disputes_reported_by_user_id
├─ idx_disputes_status
└─ idx_disputes_created_at
```

**Propósito**: Registro de disputas y problemas

---

### 11. **reviews** (Calificaciones/Reviews)
Almacena reseñas y calificaciones entre usuarios.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ project_id            (INTEGER, FOREIGN KEY → projects.id, NOT NULL)
├─ reviewer_user_id      (INTEGER, FOREIGN KEY → users.id, NOT NULL) → Quien hace la review
├─ reviewed_user_id      (INTEGER, FOREIGN KEY → users.id, NOT NULL) → Quien es revisado
├─ review_type           (ENUM: 'PROFESSIONAL_BY_CUSTOMER', 'CUSTOMER_BY_PROFESSIONAL')
├─ rating                (DECIMAL(3,2) NOT NULL, CHECK rating >= 1 AND rating <= 5)
├─ comment               (TEXT, NULLABLE, MAX 500 chars)
├─ is_anonymous          (BOOLEAN DEFAULT FALSE)
├─ helpful_count         (INTEGER DEFAULT 0) → Cuántos marcan como útil
├─ response_comment      (TEXT, NULLABLE, MAX 300 chars) → Respuesta del revisado
├─ created_at            (TIMESTAMP DEFAULT NOW())
├─ updated_at            (TIMESTAMP DEFAULT NOW())
└─ published             (BOOLEAN DEFAULT TRUE)

ÍNDICES:
├─ idx_reviews_project_id
├─ idx_reviews_reviewed_user_id
├─ idx_reviews_reviewer_user_id
├─ idx_reviews_review_type
└─ idx_reviews_created_at
```

**Propósito**: Calificaciones y reseñas entre usuarios

---

### 12. **notifications** (Notificaciones)
Almacena historial de notificaciones enviadas.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ user_id               (INTEGER, FOREIGN KEY → users.id, NOT NULL)
├─ notification_type     (ENUM: 'NEW_REQUEST', 'REQUEST_ACCEPTED', 'WORK_COMPLETED', 'PAYMENT_RECEIVED', 'DISPUTE_FILED', OTHER)
├─ title                 (VARCHAR 255, NOT NULL)
├─ message               (TEXT, NOT NULL)
├─ related_project_id    (INTEGER, FOREIGN KEY → projects.id, NULLABLE)
├─ related_contact_request_id (INTEGER, FOREIGN KEY → contact_requests.id, NULLABLE)
├─ channels_sent         (TEXT ARRAY) → ['APP', 'EMAIL', 'WHATSAPP']
├─ is_read               (BOOLEAN DEFAULT FALSE)
├─ read_at               (TIMESTAMP, NULLABLE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ updated_at            (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_notifications_user_id
├─ idx_notifications_notification_type
├─ idx_notifications_is_read
└─ idx_notifications_created_at
```

**Propósito**: Historial de notificaciones

---

### 13. **preferences** (Preferencias de usuario)
Almacena preferencias de notificación y configuración del usuario.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ user_id               (INTEGER, FOREIGN KEY → users.id, NOT NULL, UNIQUE)
├─ notifications_app     (BOOLEAN DEFAULT TRUE)
├─ notifications_email   (BOOLEAN DEFAULT TRUE)
├─ notifications_whatsapp (BOOLEAN DEFAULT FALSE)
├─ silent_hours_start    (TIME, NULLABLE)
├─ silent_hours_end      (TIME, NULLABLE)
├─ notification_frequency (ENUM: 'IMMEDIATE', 'DAILY', 'WEEKLY', DEFAULT 'IMMEDIATE')
├─ show_profile_public   (BOOLEAN DEFAULT TRUE) → Para profesionales
├─ show_phone_public     (BOOLEAN DEFAULT FALSE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ updated_at            (TIMESTAMP DEFAULT NOW())

ÍNDICES:
└─ idx_preferences_user_id (UNIQUE)
```

**Propósito**: Preferencias de notificación y privacidad

---

### 14. **certifications** (Certificaciones)
Almacena certificaciones y licencias de profesionales.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ professional_id       (INTEGER, FOREIGN KEY → professionals.id, NOT NULL)
├─ certification_name    (VARCHAR 255, NOT NULL)
├─ issuing_organization  (VARCHAR 255, NOT NULL)
├─ issue_date            (DATE, NOT NULL)
├─ expiry_date           (DATE, NULLABLE)
├─ certificate_url       (TEXT, NULLABLE) → URL del documento
├─ is_verified           (BOOLEAN DEFAULT FALSE)
├─ verified_by_admin     (BOOLEAN DEFAULT FALSE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ updated_at            (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_certifications_professional_id
├─ idx_certifications_is_verified
└─ idx_certifications_expiry_date
```

**Propósito**: Certificaciones del profesional

---

### 15. **specialties** (Especialidades disponibles)
Tabla maestro con las especialidades del sistema.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ name                  (VARCHAR 100, NOT NULL, UNIQUE) → Ejm: Plomería
├─ description           (TEXT, NULLABLE)
├─ icon_url              (TEXT, NULLABLE)
├─ is_active             (BOOLEAN DEFAULT TRUE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ updated_at            (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_specialties_name
└─ idx_specialties_is_active
```

**Propósito**: Catálogo maestro de especialidades

---

### 16. **sub_specialties** (Sub-especialidades)
Sub-categorías dentro de una especialidad.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ specialty_id          (INTEGER, FOREIGN KEY → specialties.id, NOT NULL)
├─ name                  (VARCHAR 100, NOT NULL)
├─ description           (TEXT, NULLABLE)
├─ is_active             (BOOLEAN DEFAULT TRUE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ updated_at            (TIMESTAMP DEFAULT NOW())

ÍNDICES:
├─ idx_sub_specialties_specialty_id
└─ idx_sub_specialties_name
```

**Propósito**: Sub-categorías de especialidades

---

### 17. **admin_actions** (Auditoría)
Registro de acciones administrativas para auditoría.

```sql
COLUMNAS:
├─ id                    (SERIAL PRIMARY KEY)
├─ admin_user_id         (INTEGER, FOREIGN KEY → users.id, NOT NULL)
├─ action_type           (ENUM: 'VERIFY_PROFESSIONAL', 'SUSPEND_USER', 'APPROVE_CERTIFICATION', 'RESOLVE_DISPUTE', OTHER)
├─ target_user_id        (INTEGER, FOREIGN KEY → users.id, NULLABLE)
├─ target_object_type    (VARCHAR 100, NULLABLE) → 'PROFESSIONAL', 'CERTIFICATION', 'DISPUTE'
├─ target_object_id      (INTEGER, NULLABLE)
├─ description           (TEXT, NULLABLE)
├─ created_at            (TIMESTAMP DEFAULT NOW())
└─ ip_address            (INET, NULLABLE)

ÍNDICES:
├─ idx_admin_actions_admin_user_id
├─ idx_admin_actions_target_user_id
└─ idx_admin_actions_created_at
```

**Propósito**: Auditoría de acciones administrativas

---

## 🔗 RELACIONES Y CARDINALIDAD

```
users (1) ──────────────────── (1) customers
  │                              
  │                              
  └────────────────────────── (1) professionals

customers (1) ──────────────── (N) contact_requests ──────────── (N) professionals
   │                               │
   │                               ├─── (1) budgets
   │                               └─── (1) projects
   │                                     │
   │                                     ├─── (N) project_photos
   │                                     ├─── (N) payments
   │                                     └─── (N) reviews
   │
   └────────────────────────── (N) saved_cards
   
professionals (1) ──────────── (N) certifications
                   │
                   ├─── (N) contact_requests
                   └─── (N) projects

disputes (N) ──────────────── (1) project
             └──────────────────── (N) payments

reviews (N) ──────────────── (1) project

notifications (N) ──────────────── (1) user
preferences (1) ────────────────── (1) user
```

---

## 🗄️ EXTENSIONES DE POSTGRESQL

### PostGIS (Geolocalización)
```sql
CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS postgis_topology;
```

**Uso**: Para búsquedas de profesionales cercanos usando coordenadas GPS.

**Tablas afectadas**:
- `customers` → location (POINT)
- `professionals` → location (POINT)
- `projects` → location (POINT)

**Funciones útiles**:
```sql
-- Buscar profesionales dentro de X km
SELECT * FROM professionals 
WHERE ST_DWithin(
    location::geography,
    ST_SetSRID(ST_MakePoint($lon, $lat), 4326)::geography,
    distance_in_meters
);

-- Calcular distancia entre dos puntos
SELECT ST_Distance(
    location1::geography,
    location2::geography
) / 1000 as distance_km;
```

### UUID Extension (Opcional pero recomendado)
```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
```

**Uso**: Generar UUIDs en lugar de SERIAL para IDs más seguros en APIs.

### JSON Extension (Nativa)
PostgreSQL ya tiene soporte para JSON/JSONB.

**Uso**: Almacenar datos dinámicos como sub_specialties (array de strings).

---

## 📍 ÍNDICES REQUERIDOS

### Búsquedas por Geolocalización
```sql
-- Índice GIST para búsquedas de distancia
CREATE INDEX idx_professionals_location_gist 
ON professionals USING GIST (location);

CREATE INDEX idx_customers_location_gist 
ON customers USING GIST (location);

CREATE INDEX idx_projects_location_gist 
ON projects USING GIST (location);
```

### Búsquedas por Especialidad y Ciudad
```sql
CREATE INDEX idx_professionals_specialty_city 
ON professionals(profession, city);

CREATE INDEX idx_professionals_sub_specialties_gin 
ON professionals USING GIN (sub_specialties);
```

### Búsquedas por Estado y Fecha
```sql
CREATE INDEX idx_projects_status_created 
ON projects(status, created_at DESC);

CREATE INDEX idx_contact_requests_status_updated 
ON contact_requests(status, updated_at DESC);

CREATE INDEX idx_payments_status_date 
ON payments(status, created_at DESC);
```

### Búsquedas de Escrow
```sql
CREATE INDEX idx_payments_escrow_release 
ON payments(escrow_release_date) 
WHERE escrow_status = 'IN_ESCROW';
```

### Búsquedas de Notificaciones
```sql
CREATE INDEX idx_notifications_user_unread 
ON notifications(user_id, is_read, created_at DESC);
```

---

## ✅ RESTRICCIONES Y VALIDACIONES

### Restricciones a Nivel BD

```sql
-- Usuarios
ALTER TABLE users ADD CONSTRAINT email_format CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$');
ALTER TABLE users ADD CONSTRAINT phone_format CHECK (phone ~* '^\+?[0-9]{10,}$');

-- Professionals
ALTER TABLE professionals ADD CONSTRAINT experience_positive CHECK (years_experience >= 0);
ALTER TABLE professionals ADD CONSTRAINT visit_cost_positive CHECK (visit_cost >= 0);
ALTER TABLE professionals ADD CONSTRAINT price_range_valid CHECK (price_range_min <= price_range_max OR price_range_max IS NULL);
ALTER TABLE professionals ADD CONSTRAINT rating_range CHECK (rating_avg >= 0 AND rating_avg <= 5);
ALTER TABLE professionals ADD CONSTRAINT response_rate_valid CHECK (response_rate >= 0 AND response_rate <= 100);

-- Customers
ALTER TABLE customers ADD CONSTRAINT rating_range CHECK (rating_avg >= 0 AND rating_avg <= 5);

-- Contact Requests
ALTER TABLE contact_requests ADD CONSTRAINT description_length CHECK (LENGTH(description) >= 50);

-- Budgets
ALTER TABLE budgets ADD CONSTRAINT budget_amounts_positive CHECK (visit_cost >= 0 AND estimated_work_cost >= 0);
ALTER TABLE budgets ADD CONSTRAINT commission_rate_valid CHECK (platform_commission_rate >= 0 AND platform_commission_rate <= 100);

-- Payments
ALTER TABLE payments ADD CONSTRAINT amount_positive CHECK (amount > 0);
ALTER TABLE payments ADD CONSTRAINT commission_positive CHECK (platform_commission >= 0);
ALTER TABLE payments ADD CONSTRAINT net_amount_valid CHECK (net_amount = amount - platform_commission);

-- Reviews
ALTER TABLE reviews ADD CONSTRAINT rating_range CHECK (rating >= 1 AND rating <= 5);
ALTER TABLE reviews ADD CONSTRAINT comment_length CHECK (LENGTH(comment) <= 500 OR comment IS NULL);

-- Disputes
ALTER TABLE disputes ADD CONSTRAINT evidence_photos_valid CHECK (array_length(evidence_photos_urls, 1) >= 0 OR evidence_photos_urls IS NULL);
```

---

## 📐 DIAGRAMA ENTIDAD-RELACIÓN (Simplificado)

```
┌──────────────────────────┐
│         USERS            │ (Base)
├──────────────────────────┤
│ id (PK)                  │
│ email (UNIQUE)           │
│ phone (UNIQUE)           │
│ password_hash            │
│ role (ENUM)              │
│ status (ENUM)            │
└──────┬───────────────────┘
       │
       ├─────────────────────────────┐
       │                             │
       ▼                             ▼
┌──────────────────────────┐  ┌──────────────────────────┐
│      CUSTOMERS           │  │   PROFESSIONALS          │
├──────────────────────────┤  ├──────────────────────────┤
│ id (PK)                  │  │ id (PK)                  │
│ user_id (FK)             │  │ user_id (FK)             │
│ city                     │  │ profession               │
│ location (POINT)         │  │ location (POINT)         │
│ rating_avg               │  │ visit_cost               │
│ total_projects           │  │ is_verified              │
└──────┬───────────────────┘  └────────┬────────────────┘
       │                              │
       │ (1:N)                        │
       │                              │
       └──────────────────┬───────────┘
                          │
                          ▼
              ┌──────────────────────────┐
              │  CONTACT_REQUESTS        │
              ├──────────────────────────┤
              │ id (PK)                  │
              │ customer_id (FK)         │
              │ professional_id (FK)     │
              │ contact_method           │
              │ status (ENUM)            │
              │ created_at               │
              └──────┬────────────────────┘
                     │
                     ├─ (1:1) ───▶ BUDGETS
                     │
                     └─ (1:1) ───▶ PROJECTS
                                  │
                                  ├─ (1:N) ───▶ PROJECT_PHOTOS
                                  ├─ (1:N) ───▶ PAYMENTS
                                  └─ (1:1) ───▶ REVIEWS

PROFESSIONALS (1:N)──▶ CERTIFICATIONS
PROFESSIONALS (1:N)──▶ REVIEWS

USERS (1:N)──▶ NOTIFICATIONS
USERS (1:1)──▶ PREFERENCES
USERS (1:N)──▶ SAVED_CARDS

PROJECTS (1:N)──▶ DISPUTES
```

---

## 📋 PRÓXIMAS ETAPAS

1. **Crear scripts SQL** con definición de todas las tablas
2. **Crear constraints y triggers** para validaciones
3. **Crear vistas (VIEWs)** para consultas comunes
4. **Crear funciones PL/pgSQL** para operaciones complejas
5. **Implementar auditoría** (trigger de historial)
6. **Configurar backups** y replicación

---

**Análisis Completado**: 28/03/2026  
**Versión**: 1.0 (Análisis de estructura de BD)

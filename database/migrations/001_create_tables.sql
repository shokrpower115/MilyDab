# 📊 SCRIPTS SQL - CREACIÓN DE TABLAS

## ⚠️ IMPORTANTE - PostgreSQL 9.2

Tu versión es **PostgreSQL 9.2**. Nota: Esta es una versión antigua (lanzada en 2012). Se recomienda actualizar a **PostgreSQL 13+** para mejor performance con PostGIS.

De todas formas, los scripts funcionarán, pero algunos tipos de datos pueden necesitar ajustes.

---

## 🎯 PASO 1: CREAR BASE DE DATOS

```sql
-- Crear base de datos
CREATE DATABASE proservi_db
  ENCODING 'UTF8'
  LC_COLLATE 'en_US.UTF-8'
  LC_CTYPE 'en_US.UTF-8'
  TEMPLATE template0;

-- Conectarse a la nueva BD
\c proservi_db
```

---

## 🗂️ PASO 2: CREAR EXTENSIONES

```sql
-- Habilitar extensiones necesarias
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
-- CREATE EXTENSION IF NOT EXISTS postgis;  -- Solo si PostgreSQL >= 9.1 y PostGIS instalado
-- CREATE EXTENSION IF NOT EXISTS postgis_topology;
```

**Nota**: PostGIS en PostgreSQL 9.2 puede ser opcional. Si quieres geolocalización, necesitarás actualizar PostgreSQL.

---

## 📝 PASO 3: CREAR ENUMS (TIPOS ENUMERADOS)

```sql
-- Roles de usuario
CREATE TYPE user_role AS ENUM ('CUSTOMER', 'PROFESSIONAL');

-- Estados de usuario
CREATE TYPE user_status AS ENUM ('ACTIVE', 'INACTIVE', 'SUSPENDED', 'PENDING_VERIFICATION');

-- Estados de solicitud de contacto
CREATE TYPE contact_request_status AS ENUM ('PENDING', 'ACCEPTED', 'REJECTED', 'CANCELLED');

-- Métodos de contacto
CREATE TYPE contact_method AS ENUM ('VISIT', 'CALL', 'QUOTE', 'VIDEO_CALL');

-- Urgencia del trabajo
CREATE TYPE urgency_level AS ENUM ('LOW', 'MEDIUM', 'HIGH');

-- Estados del presupuesto
CREATE TYPE budget_status AS ENUM ('PENDING', 'ACCEPTED', 'REJECTED', 'EXPIRED');

-- Estados del proyecto
CREATE TYPE project_status AS ENUM ('ACTIVE', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED');

-- Estados de pago
CREATE TYPE payment_status AS ENUM ('PENDING', 'COMPLETED', 'FAILED', 'REFUNDED');

-- Estados de escrow
CREATE TYPE escrow_status AS ENUM ('IN_ESCROW', 'RELEASED', 'REFUNDED');

-- Tipo de pago
CREATE TYPE payment_method AS ENUM ('CARD', 'BANK_TRANSFER');

-- Tipo de disputa
CREATE TYPE dispute_reason AS ENUM ('INCOMPLETE_WORK', 'QUALITY_ISSUE', 'PRICE_MISMATCH', 'NO_CONTACT', 'FALSE_INFORMATION', 'OTHER');

-- Estados de disputa
CREATE TYPE dispute_status AS ENUM ('OPEN', 'IN_REVIEW', 'RESOLVED', 'APPEALED');

-- Tipo de notificación
CREATE TYPE notification_type AS ENUM ('NEW_REQUEST', 'REQUEST_ACCEPTED', 'WORK_COMPLETED', 'PAYMENT_RECEIVED', 'DISPUTE_FILED', 'OTHER');
```

---

## 📋 PASO 4: CREAR TABLAS

### **1. TABLA: users (Base de usuarios)**

```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    role user_role NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    phone VARCHAR(20) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    full_name VARCHAR(255) NOT NULL,
    profile_photo_url TEXT,
    status user_status NOT NULL DEFAULT 'PENDING_VERIFICATION',
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    email_verified BOOLEAN NOT NULL DEFAULT FALSE,
    phone_verified BOOLEAN NOT NULL DEFAULT FALSE,
    last_login TIMESTAMP
);

-- Crear índices para tabla users
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_phone ON users(phone);
CREATE INDEX idx_users_role_status ON users(role, status);
CREATE INDEX idx_users_created_at ON users(created_at);

-- Validaciones
ALTER TABLE users ADD CONSTRAINT email_format 
    CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$');
ALTER TABLE users ADD CONSTRAINT phone_format 
    CHECK (phone ~* '^\+?[0-9]{10,}$');
```

### **2. TABLA: customers (Clientes/Usuarios normales)**

```sql
CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    city VARCHAR(100) NOT NULL,
    latitude DECIMAL(10,8),
    longitude DECIMAL(11,8),
    address TEXT,
    bio TEXT,
    rating_avg DECIMAL(3,2) NOT NULL DEFAULT 0,
    total_projects INTEGER NOT NULL DEFAULT 0,
    total_spent DECIMAL(10,2) NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_customers_user_id ON customers(user_id);
CREATE INDEX idx_customers_city ON customers(city);
CREATE INDEX idx_customers_location ON customers(latitude, longitude);
CREATE INDEX idx_customers_rating_avg ON customers(rating_avg);

-- Validaciones
ALTER TABLE customers ADD CONSTRAINT rating_range 
    CHECK (rating_avg >= 0 AND rating_avg <= 5);
ALTER TABLE customers ADD CONSTRAINT total_spent_positive 
    CHECK (total_spent >= 0);
```

### **3. TABLA: professionals (Profesionales)**

```sql
CREATE TABLE professionals (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    profession VARCHAR(100) NOT NULL,
    sub_specialties TEXT[],
    years_experience INTEGER NOT NULL,
    city VARCHAR(100) NOT NULL,
    latitude DECIMAL(10,8),
    longitude DECIMAL(11,8),
    description_services TEXT NOT NULL,
    price_range_min DECIMAL(10,2),
    price_range_max DECIMAL(10,2),
    business_hours_start TIME,
    business_hours_end TIME,
    visit_cost DECIMAL(10,2) NOT NULL DEFAULT 0,
    is_verified BOOLEAN NOT NULL DEFAULT FALSE,
    verification_date TIMESTAMP,
    rating_avg DECIMAL(3,2) NOT NULL DEFAULT 0,
    total_projects INTEGER NOT NULL DEFAULT 0,
    total_earned DECIMAL(10,2) NOT NULL DEFAULT 0,
    document_number VARCHAR(50) NOT NULL UNIQUE,
    document_verified BOOLEAN NOT NULL DEFAULT FALSE,
    response_rate DECIMAL(5,2) NOT NULL DEFAULT 0,
    response_time_hours INTEGER NOT NULL DEFAULT 24,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_professionals_user_id ON professionals(user_id);
CREATE INDEX idx_professionals_profession ON professionals(profession);
CREATE INDEX idx_professionals_city ON professionals(city);
CREATE INDEX idx_professionals_location ON professionals(latitude, longitude);
CREATE INDEX idx_professionals_rating_avg ON professionals(rating_avg);
CREATE INDEX idx_professionals_is_verified ON professionals(is_verified);
CREATE INDEX idx_professionals_document_number ON professionals(document_number);

-- Validaciones
ALTER TABLE professionals ADD CONSTRAINT experience_positive 
    CHECK (years_experience >= 0);
ALTER TABLE professionals ADD CONSTRAINT visit_cost_positive 
    CHECK (visit_cost >= 0);
ALTER TABLE professionals ADD CONSTRAINT price_range_valid 
    CHECK (price_range_min <= price_range_max OR price_range_max IS NULL);
ALTER TABLE professionals ADD CONSTRAINT rating_range 
    CHECK (rating_avg >= 0 AND rating_avg <= 5);
ALTER TABLE professionals ADD CONSTRAINT response_rate_valid 
    CHECK (response_rate >= 0 AND response_rate <= 100);
```

### **4. TABLA: specialties (Catálogo de especialidades)**

```sql
CREATE TABLE specialties (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    icon_url TEXT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_specialties_name ON specialties(name);
CREATE INDEX idx_specialties_is_active ON specialties(is_active);

-- Insertar datos iniciales
INSERT INTO specialties (name, description) VALUES
('Plomería', 'Servicios de reparación y instalación de tuberías'),
('Electricidad', 'Trabajos eléctricos y reparación'),
('Pintura', 'Servicios de pintura interior y exterior'),
('Carpintería', 'Trabajos de carpintería y muebles'),
('Limpieza', 'Servicios de limpieza profesional'),
('Reparación General', 'Reparaciones varias en el hogar'),
('Albañilería', 'Trabajos de construcción y reformas'),
('Jardinería', 'Servicios de jardinería y mantenimiento de áreas verdes'),
('Refrigeración', 'Reparación y mantenimiento de sistemas de aire acondicionado'),
('Fontanería', 'Servicios avanzados de fontanería');
```

### **5. TABLA: contact_requests (Solicitudes de contacto)**

```sql
CREATE TABLE contact_requests (
    id SERIAL PRIMARY KEY,
    customer_id INTEGER NOT NULL REFERENCES customers(id) ON DELETE CASCADE,
    professional_id INTEGER NOT NULL REFERENCES professionals(id) ON DELETE CASCADE,
    contact_method contact_method NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    urgency urgency_level NOT NULL DEFAULT 'MEDIUM',
    photos_urls TEXT[],
    customer_estimated_budget DECIMAL(10,2),
    available_from TIMESTAMP,
    available_to TIMESTAMP,
    status contact_request_status NOT NULL DEFAULT 'PENDING',
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_contact_requests_customer_id ON contact_requests(customer_id);
CREATE INDEX idx_contact_requests_professional_id ON contact_requests(professional_id);
CREATE INDEX idx_contact_requests_status ON contact_requests(status);
CREATE INDEX idx_contact_requests_created_at ON contact_requests(created_at);
CREATE INDEX idx_contact_requests_customer_professional ON contact_requests(customer_id, professional_id);

-- Validaciones
ALTER TABLE contact_requests ADD CONSTRAINT description_length 
    CHECK (LENGTH(description) >= 50);
ALTER TABLE contact_requests ADD CONSTRAINT budget_positive 
    CHECK (customer_estimated_budget > 0 OR customer_estimated_budget IS NULL);
```

### **6. TABLA: budgets (Presupuestos)**

```sql
CREATE TABLE budgets (
    id SERIAL PRIMARY KEY,
    contact_request_id INTEGER NOT NULL UNIQUE REFERENCES contact_requests(id) ON DELETE CASCADE,
    customer_id INTEGER NOT NULL REFERENCES customers(id) ON DELETE CASCADE,
    professional_id INTEGER NOT NULL REFERENCES professionals(id) ON DELETE CASCADE,
    visit_cost DECIMAL(10,2) NOT NULL DEFAULT 0,
    estimated_work_cost DECIMAL(10,2),
    platform_commission_rate DECIMAL(5,2) NOT NULL DEFAULT 8,
    total_estimated DECIMAL(10,2),
    status budget_status NOT NULL DEFAULT 'PENDING',
    valid_until TIMESTAMP NOT NULL,
    payment_required_upfront BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_budgets_contact_request_id ON budgets(contact_request_id);
CREATE INDEX idx_budgets_customer_id ON budgets(customer_id);
CREATE INDEX idx_budgets_professional_id ON budgets(professional_id);
CREATE INDEX idx_budgets_status ON budgets(status);
CREATE INDEX idx_budgets_valid_until ON budgets(valid_until);

-- Validaciones
ALTER TABLE budgets ADD CONSTRAINT budget_amounts_positive 
    CHECK (visit_cost >= 0 AND estimated_work_cost >= 0 OR estimated_work_cost IS NULL);
ALTER TABLE budgets ADD CONSTRAINT commission_rate_valid 
    CHECK (platform_commission_rate >= 0 AND platform_commission_rate <= 100);
```

### **7. TABLA: projects (Proyectos/Trabajos)**

```sql
CREATE TABLE projects (
    id SERIAL PRIMARY KEY,
    contact_request_id INTEGER NOT NULL UNIQUE REFERENCES contact_requests(id) ON DELETE CASCADE,
    customer_id INTEGER NOT NULL REFERENCES customers(id) ON DELETE CASCADE,
    professional_id INTEGER NOT NULL REFERENCES professionals(id) ON DELETE CASCADE,
    contact_method contact_method NOT NULL,
    status project_status NOT NULL DEFAULT 'ACTIVE',
    work_title VARCHAR(255) NOT NULL,
    work_description TEXT NOT NULL,
    estimated_start_date TIMESTAMP,
    estimated_end_date TIMESTAMP,
    actual_start_date TIMESTAMP,
    actual_end_date TIMESTAMP,
    location_address TEXT,
    location_latitude DECIMAL(10,8),
    location_longitude DECIMAL(11,8),
    customer_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    professional_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    payment_status payment_status NOT NULL DEFAULT 'PENDING',
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    completed_at TIMESTAMP
);

-- Crear índices
CREATE INDEX idx_projects_contact_request_id ON projects(contact_request_id);
CREATE INDEX idx_projects_customer_id ON projects(customer_id);
CREATE INDEX idx_projects_professional_id ON projects(professional_id);
CREATE INDEX idx_projects_status ON projects(status);
CREATE INDEX idx_projects_completed_at ON projects(completed_at);
CREATE INDEX idx_projects_location ON projects(location_latitude, location_longitude);
CREATE INDEX idx_projects_payment_status ON projects(payment_status);
```

### **8. TABLA: project_photos (Fotos del proyecto)**

```sql
CREATE TABLE project_photos (
    id SERIAL PRIMARY KEY,
    project_id INTEGER NOT NULL REFERENCES projects(id) ON DELETE CASCADE,
    uploaded_by VARCHAR(50) NOT NULL, -- 'CUSTOMER' o 'PROFESSIONAL'
    photo_url TEXT NOT NULL,
    description TEXT,
    uploaded_at TIMESTAMP NOT NULL DEFAULT NOW(),
    "order" INTEGER NOT NULL DEFAULT 0
);

-- Crear índices
CREATE INDEX idx_project_photos_project_id ON project_photos(project_id);
CREATE INDEX idx_project_photos_uploaded_at ON project_photos(uploaded_at);
```

### **9. TABLA: payments (Pagos)**

```sql
CREATE TABLE payments (
    id SERIAL PRIMARY KEY,
    project_id INTEGER NOT NULL REFERENCES projects(id) ON DELETE CASCADE,
    payer_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    receiver_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    payment_type VARCHAR(50) NOT NULL, -- 'VISIT_COST', 'WORK_COST', 'FULL_PROJECT'
    amount DECIMAL(10,2) NOT NULL,
    platform_commission DECIMAL(10,2) NOT NULL DEFAULT 0,
    net_amount DECIMAL(10,2) NOT NULL,
    payment_method payment_method NOT NULL,
    external_transaction_id VARCHAR(255),
    status payment_status NOT NULL DEFAULT 'PENDING',
    escrow_status escrow_status,
    escrow_release_date TIMESTAMP,
    receipt_url TEXT,
    invoice_url TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    completed_at TIMESTAMP
);

-- Crear índices
CREATE INDEX idx_payments_project_id ON payments(project_id);
CREATE INDEX idx_payments_payer_user_id ON payments(payer_user_id);
CREATE INDEX idx_payments_receiver_user_id ON payments(receiver_user_id);
CREATE INDEX idx_payments_status ON payments(status);
CREATE INDEX idx_payments_escrow_release_date ON payments(escrow_release_date) WHERE escrow_status = 'IN_ESCROW';
CREATE INDEX idx_payments_created_at ON payments(created_at);

-- Validaciones
ALTER TABLE payments ADD CONSTRAINT amount_positive 
    CHECK (amount > 0);
ALTER TABLE payments ADD CONSTRAINT commission_positive 
    CHECK (platform_commission >= 0);
ALTER TABLE payments ADD CONSTRAINT net_amount_valid 
    CHECK (net_amount = amount - platform_commission);
```

### **10. TABLA: saved_cards (Tarjetas guardadas)**

```sql
CREATE TABLE saved_cards (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    card_token VARCHAR(255) NOT NULL,
    card_last_four VARCHAR(4) NOT NULL,
    card_brand VARCHAR(50),
    expiry_month INTEGER,
    expiry_year INTEGER,
    is_default BOOLEAN NOT NULL DEFAULT FALSE,
    is_verified BOOLEAN NOT NULL DEFAULT FALSE,
    verification_amount DECIMAL(10,2),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_saved_cards_user_id ON saved_cards(user_id);
CREATE INDEX idx_saved_cards_is_default ON saved_cards(is_default);
CREATE INDEX idx_saved_cards_created_at ON saved_cards(created_at);
```

### **11. TABLA: disputes (Disputas)**

```sql
CREATE TABLE disputes (
    id SERIAL PRIMARY KEY,
    project_id INTEGER NOT NULL REFERENCES projects(id) ON DELETE CASCADE,
    reported_by_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    reported_against_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    reason dispute_reason NOT NULL,
    description TEXT NOT NULL,
    evidence_photos_urls TEXT[],
    status dispute_status NOT NULL DEFAULT 'OPEN',
    resolution_type VARCHAR(50),
    resolution_notes TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    resolved_at TIMESTAMP
);

-- Crear índices
CREATE INDEX idx_disputes_project_id ON disputes(project_id);
CREATE INDEX idx_disputes_reported_by_user_id ON disputes(reported_by_user_id);
CREATE INDEX idx_disputes_status ON disputes(status);
CREATE INDEX idx_disputes_created_at ON disputes(created_at);
```

### **12. TABLA: reviews (Calificaciones)**

```sql
CREATE TABLE reviews (
    id SERIAL PRIMARY KEY,
    project_id INTEGER NOT NULL REFERENCES projects(id) ON DELETE CASCADE,
    reviewer_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    reviewed_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    review_type VARCHAR(50) NOT NULL, -- 'PROFESSIONAL_BY_CUSTOMER' o 'CUSTOMER_BY_PROFESSIONAL'
    rating DECIMAL(3,2) NOT NULL,
    comment TEXT,
    is_anonymous BOOLEAN NOT NULL DEFAULT FALSE,
    helpful_count INTEGER NOT NULL DEFAULT 0,
    response_comment TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    published BOOLEAN NOT NULL DEFAULT TRUE
);

-- Crear índices
CREATE INDEX idx_reviews_project_id ON reviews(project_id);
CREATE INDEX idx_reviews_reviewed_user_id ON reviews(reviewed_user_id);
CREATE INDEX idx_reviews_reviewer_user_id ON reviews(reviewer_user_id);
CREATE INDEX idx_reviews_created_at ON reviews(created_at);

-- Validaciones
ALTER TABLE reviews ADD CONSTRAINT rating_range 
    CHECK (rating >= 1 AND rating <= 5);
ALTER TABLE reviews ADD CONSTRAINT comment_length 
    CHECK (LENGTH(comment) <= 500 OR comment IS NULL);
```

### **13. TABLA: certifications (Certificaciones)**

```sql
CREATE TABLE certifications (
    id SERIAL PRIMARY KEY,
    professional_id INTEGER NOT NULL REFERENCES professionals(id) ON DELETE CASCADE,
    certification_name VARCHAR(255) NOT NULL,
    issuing_organization VARCHAR(255) NOT NULL,
    issue_date DATE NOT NULL,
    expiry_date DATE,
    certificate_url TEXT,
    is_verified BOOLEAN NOT NULL DEFAULT FALSE,
    verified_by_admin BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_certifications_professional_id ON certifications(professional_id);
CREATE INDEX idx_certifications_is_verified ON certifications(is_verified);
CREATE INDEX idx_certifications_expiry_date ON certifications(expiry_date);
```

### **14. TABLA: notifications (Notificaciones)**

```sql
CREATE TABLE notifications (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    notification_type notification_type NOT NULL,
    title VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    related_project_id INTEGER REFERENCES projects(id) ON DELETE SET NULL,
    related_contact_request_id INTEGER REFERENCES contact_requests(id) ON DELETE SET NULL,
    channels_sent TEXT[],
    is_read BOOLEAN NOT NULL DEFAULT FALSE,
    read_at TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_notifications_user_id ON notifications(user_id);
CREATE INDEX idx_notifications_notification_type ON notifications(notification_type);
CREATE INDEX idx_notifications_is_read ON notifications(is_read);
CREATE INDEX idx_notifications_user_unread ON notifications(user_id, is_read, created_at DESC);
CREATE INDEX idx_notifications_created_at ON notifications(created_at);
```

### **15. TABLA: preferences (Preferencias del usuario)**

```sql
CREATE TABLE preferences (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL UNIQUE REFERENCES users(id) ON DELETE CASCADE,
    notifications_app BOOLEAN NOT NULL DEFAULT TRUE,
    notifications_email BOOLEAN NOT NULL DEFAULT TRUE,
    notifications_whatsapp BOOLEAN NOT NULL DEFAULT FALSE,
    silent_hours_start TIME,
    silent_hours_end TIME,
    notification_frequency VARCHAR(50) NOT NULL DEFAULT 'IMMEDIATE', -- IMMEDIATE, DAILY, WEEKLY
    show_profile_public BOOLEAN NOT NULL DEFAULT TRUE,
    show_phone_public BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Crear índices
CREATE INDEX idx_preferences_user_id ON preferences(user_id);
```

### **16. TABLA: admin_actions (Auditoría)**

```sql
CREATE TABLE admin_actions (
    id SERIAL PRIMARY KEY,
    admin_user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    action_type VARCHAR(100) NOT NULL,
    target_user_id INTEGER REFERENCES users(id) ON DELETE SET NULL,
    target_object_type VARCHAR(100),
    target_object_id INTEGER,
    description TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    ip_address INET
);

-- Crear índices
CREATE INDEX idx_admin_actions_admin_user_id ON admin_actions(admin_user_id);
CREATE INDEX idx_admin_actions_target_user_id ON admin_actions(target_user_id);
CREATE INDEX idx_admin_actions_created_at ON admin_actions(created_at);
```

---

## 🔄 PASO 5: CREAR VISTAS ÚTILES

```sql
-- Vista: Información completa del profesional
CREATE VIEW v_professional_details AS
SELECT
    p.id,
    p.user_id,
    u.full_name,
    u.email,
    u.phone,
    u.profile_photo_url,
    p.profession,
    p.city,
    p.latitude,
    p.longitude,
    p.years_experience,
    p.visit_cost,
    p.rating_avg,
    p.total_projects,
    p.is_verified,
    COUNT(DISTINCT c.id) as total_certifications,
    AVG(r.rating) as average_rating
FROM professionals p
JOIN users u ON p.user_id = u.id
LEFT JOIN certifications c ON p.id = c.id
LEFT JOIN reviews r ON p.id = (SELECT professional_id FROM projects WHERE id = r.project_id)
GROUP BY p.id, u.id;

-- Vista: Proyectos activos con detalles
CREATE VIEW v_active_projects AS
SELECT
    pr.id,
    pr.work_title,
    pr.status,
    c.full_name as customer_name,
    p.full_name as professional_name,
    pr.created_at,
    pr.updated_at,
    EXTRACT(DAY FROM NOW() - pr.created_at) as days_active
FROM projects pr
JOIN customers cust ON pr.customer_id = cust.id
JOIN users c ON cust.user_id = c.id
JOIN professionals prof ON pr.professional_id = prof.id
JOIN users p ON prof.user_id = p.id
WHERE pr.status IN ('ACTIVE', 'IN_PROGRESS');

-- Vista: Resumen de ingresos del profesional
CREATE VIEW v_professional_earnings AS
SELECT
    p.id,
    p.user_id,
    u.full_name,
    COUNT(DISTINCT pay.id) as total_transactions,
    SUM(CASE WHEN pay.status = 'COMPLETED' THEN pay.net_amount ELSE 0 END) as total_earned,
    SUM(CASE WHEN pay.status = 'PENDING' THEN pay.net_amount ELSE 0 END) as pending_amount,
    MAX(pay.completed_at) as last_payment_date
FROM professionals p
JOIN users u ON p.user_id = u.id
LEFT JOIN payments pay ON p.user_id = pay.receiver_user_id
GROUP BY p.id, u.id;
```

---

## ✅ PASO 6: CREAR TRIGGERS PARA AUDITORÍA (Opcional pero recomendado)

```sql
-- Tabla de auditoría
CREATE TABLE audit_log (
    id SERIAL PRIMARY KEY,
    table_name VARCHAR(100),
    operation VARCHAR(10), -- INSERT, UPDATE, DELETE
    record_id INTEGER,
    changed_data JSONB,
    changed_at TIMESTAMP DEFAULT NOW(),
    changed_by_user_id INTEGER
);

-- Trigger para registrar cambios en users
CREATE OR REPLACE FUNCTION audit_users_changes()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO audit_log (table_name, operation, record_id, changed_data)
    VALUES (
        'users',
        TG_OP,
        COALESCE(NEW.id, OLD.id),
        row_to_json(NEW)
    );
    RETURN COALESCE(NEW, OLD);
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER users_audit_trigger
AFTER INSERT OR UPDATE OR DELETE ON users
FOR EACH ROW EXECUTE FUNCTION audit_users_changes();
```

---

## 📋 CHECKLIST DE INSTALACIÓN

- [ ] Crear base de datos `proservi_db`
- [ ] Ejecutar todos los scripts SQL en orden
- [ ] Verificar que todas las tablas se crearon correctamente
- [ ] Verificar relaciones foregin keys
- [ ] Verificar índices creados
- [ ] Verificar datos iniciales en `specialties`
- [ ] Crear usuario de prueba (opcional)

---

## 🧪 SCRIPT DE PRUEBA (Crear usuario de prueba)

```sql
-- Crear usuario de prueba (Cliente)
BEGIN;

INSERT INTO users (
    role, email, phone, password_hash, full_name, status, email_verified, phone_verified
) VALUES (
    'CUSTOMER',
    'cliente@test.com',
    '+1234567890',
    'hashed_password_here', -- En la práctica usar bcrypt
    'Juan Pérez',
    'ACTIVE',
    true,
    true
) RETURNING id AS user_id;

-- Luego insertar en customers (reemplazar 1 con el user_id retornado)
INSERT INTO customers (user_id, city, latitude, longitude, rating_avg)
VALUES (1, 'Madrid', 40.4168, -3.7038, 0);

-- Crear profesional de prueba
INSERT INTO users (
    role, email, phone, password_hash, full_name, status, email_verified, phone_verified
) VALUES (
    'PROFESSIONAL',
    'profesional@test.com',
    '+0987654321',
    'hashed_password_here',
    'Carlos García',
    'ACTIVE',
    true,
    true
) RETURNING id AS user_id;

-- Insertar en professionals (reemplazar 2 con el user_id retornado)
INSERT INTO professionals (
    user_id, profession, years_experience, city, latitude, longitude,
    description_services, document_number, is_verified
) VALUES (
    2,
    'Plomería',
    5,
    'Madrid',
    40.4200,
    -3.7050,
    'Especialista en reparación y instalación de tuberías',
    'DNI123456789',
    true
);

COMMIT;

-- Verificar datos creados
SELECT * FROM users;
SELECT * FROM customers;
SELECT * FROM professionals;
```

---

## 📞 PRÓXIMOS PASOS

1. ✅ Ejecutar estos scripts en PostgreSQL
2. ✅ Crear proyecto .NET
3. ✅ Crear proyecto Angular
4. ✅ Conectar ambos

**Documento Actualizado**: 28/03/2026
**PostgreSQL Version**: 9.2 compatible

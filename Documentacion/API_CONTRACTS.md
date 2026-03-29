# 📋 CONTRATOS API: FRONTEND ↔ BACKEND

## 🎯 Resumen

Este documento define el contrato API entre el frontend Angular y el backend .NET para asegurar que ambos se comunicen correctamente.

---

## 🔐 AUTENTICACIÓN

### **POST /api/auth/login**

**Descripción**: Autentica un usuario existente

**Request**:
```json
{
  "email": "usuario@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "email": "usuario@example.com",
    "fullName": "Juan Pérez",
    "role": "CUSTOMER",
    "profilePhotoUrl": "https://api.example.com/photos/user-1.jpg"
  }
}
```

**Response (401 Unauthorized)**:
```json
{
  "message": "Credenciales inválidas",
  "code": "INVALID_CREDENTIALS"
}
```

---

### **POST /api/auth/register/customer**

**Descripción**: Registra un nuevo cliente (Usuario que busca profesionales)

**Request**:
```json
{
  "fullName": "María García",
  "email": "maria@example.com",
  "phone": "+34912345678",
  "city": "Madrid",
  "password": "SecurePassword123!"
}
```

**Response (201 Created)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 2,
    "email": "maria@example.com",
    "fullName": "María García",
    "role": "CUSTOMER",
    "profilePhotoUrl": null
  }
}
```

**Response (409 Conflict)**:
```json
{
  "message": "El email ya está registrado",
  "code": "EMAIL_ALREADY_EXISTS"
}
```

---

### **POST /api/auth/register/professional**

**Descripción**: Registra un nuevo profesional

**Request**:
```json
{
  "fullName": "Carlos López",
  "email": "carlos@example.com",
  "phone": "+34912345678",
  "city": "Madrid",
  "profession": "Fontanero",
  "yearsExperience": 10,
  "descriptionServices": "Especialista en tuberías y fontanería residencial",
  "documentNumber": "12345678A",
  "password": "SecurePassword123!"
}
```

**Response (201 Created)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 3,
    "email": "carlos@example.com",
    "fullName": "Carlos López",
    "role": "PROFESSIONAL",
    "profilePhotoUrl": null
  }
}
```

**Response (400 Bad Request)**:
```json
{
  "message": "Documento de identidad ya registrado",
  "code": "DOCUMENT_ALREADY_EXISTS"
}
```

---

## 🔎 BÚSQUEDA DE PROFESIONALES

### **GET /api/professionals/search**

**Descripción**: Busca profesionales por especialidad y ciudad

**Query Parameters**:
| Parámetro | Tipo | Obligatorio | Ejemplo |
|-----------|------|-------------|---------|
| `specialtyId` | number | Sí | `1` |
| `city` | string | Sí | `Madrid` |
| `latitude` | number | No | `40.4168` |
| `longitude` | number | No | `-3.7038` |
| `radiusKm` | number | No | `10` (por defecto) |
| `page` | number | No | `1` |
| `pageSize` | number | No | `20` |

**Response (200 OK)**:
```json
{
  "total": 45,
  "page": 1,
  "pageSize": 20,
  "totalPages": 3,
  "data": [
    {
      "id": 3,
      "fullName": "Carlos López",
      "email": "carlos@example.com",
      "phone": "+34912345678",
      "city": "Madrid",
      "specialty": "Fontanero",
      "yearsExperience": 10,
      "descriptionServices": "Especialista en tuberías y fontanería residencial",
      "profilePhotoUrl": "https://api.example.com/photos/prof-3.jpg",
      "rating": 4.8,
      "reviewCount": 25,
      "distanceKm": 2.3,
      "isAvailable": true,
      "responseTimeHours": 2
    },
    {
      "id": 4,
      "fullName": "Ana Martínez",
      "email": "ana@example.com",
      "phone": "+34987654321",
      "city": "Madrid",
      "specialty": "Fontanero",
      "yearsExperience": 8,
      "descriptionServices": "Reparación y mantenimiento de fontanería",
      "profilePhotoUrl": null,
      "rating": 4.5,
      "reviewCount": 18,
      "distanceKm": 3.1,
      "isAvailable": true,
      "responseTimeHours": 4
    }
  ]
}
```

---

### **GET /api/professionals/{id}**

**Descripción**: Obtiene detalles completos de un profesional

**Response (200 OK)**:
```json
{
  "id": 3,
  "fullName": "Carlos López",
  "email": "carlos@example.com",
  "phone": "+34912345678",
  "city": "Madrid",
  "specialty": "Fontanero",
  "subSpecialties": ["Tuberías", "Desagües", "Instalación"],
  "yearsExperience": 10,
  "descriptionServices": "Especialista en tuberías y fontanería residencial",
  "profilePhotoUrl": "https://api.example.com/photos/prof-3.jpg",
  "rating": 4.8,
  "reviewCount": 25,
  "distanceKm": 2.3,
  "isAvailable": true,
  "responseTimeHours": 2,
  "certifications": [
    {
      "id": 1,
      "name": "Certificación Fontanería Residencial",
      "issuer": "Colegio Profesional",
      "issueDate": "2015-03-15",
      "expiryDate": null
    }
  ],
  "reviews": [
    {
      "id": 1,
      "authorName": "Juan Pérez",
      "rating": 5,
      "comment": "Excelente trabajo, muy profesional",
      "createdAt": "2024-03-10"
    }
  ]
}
```

---

## 📬 SOLICITUD DE CONTACTO

### **POST /api/contact-requests**

**Descripción**: Crea una solicitud de contacto hacia un profesional

**Headers**:
```
Authorization: Bearer {token}
```

**Request**:
```json
{
  "professionalId": 3,
  "contactMethod": "VISIT",
  "description": "Necesito revisar una fuga de agua en la cocina",
  "preferredDateTime": "2024-04-05T14:00:00",
  "address": "Calle Principal 123, Madrid"
}
```

**Response (201 Created)**:
```json
{
  "id": 1,
  "customerId": 2,
  "professionalId": 3,
  "status": "PENDING",
  "contactMethod": "VISIT",
  "description": "Necesito revisar una fuga de agua en la cocina",
  "preferredDateTime": "2024-04-05T14:00:00",
  "address": "Calle Principal 123, Madrid",
  "createdAt": "2024-03-25T10:30:00"
}
```

---

### **GET /api/contact-requests**

**Descripción**: Obtiene las solicitudes de contacto del usuario autenticado

**Query Parameters**:
| Parámetro | Tipo | Ejemplo |
|-----------|------|---------|
| `status` | string | `PENDING, ACCEPTED, REJECTED, COMPLETED` |
| `page` | number | `1` |
| `pageSize` | number | `20` |

**Response (200 OK)**:
```json
{
  "total": 5,
  "page": 1,
  "pageSize": 20,
  "data": [
    {
      "id": 1,
      "customerId": 2,
      "professionalId": 3,
      "professionalName": "Carlos López",
      "professionalPhone": "+34912345678",
      "status": "ACCEPTED",
      "contactMethod": "VISIT",
      "description": "Necesito revisar una fuga de agua",
      "preferredDateTime": "2024-04-05T14:00:00",
      "address": "Calle Principal 123, Madrid",
      "createdAt": "2024-03-25T10:30:00",
      "respondedAt": "2024-03-25T11:15:00"
    }
  ]
}
```

---

## 💰 PAGOS

### **POST /api/payments/create**

**Descripción**: Crea una solicitud de pago / presupuesto

**Headers**:
```
Authorization: Bearer {token}
```

**Request**:
```json
{
  "contactRequestId": 1,
  "amount": 150.00,
  "description": "Reparación de tubería principal",
  "paymentMethod": "CARD",
  "currency": "EUR"
}
```

**Response (201 Created)**:
```json
{
  "id": 1,
  "contactRequestId": 1,
  "amount": 150.00,
  "description": "Reparación de tubería principal",
  "status": "PENDING",
  "paymentMethod": "CARD",
  "currency": "EUR",
  "createdAt": "2024-03-25T12:00:00",
  "dueDate": "2024-04-01",
  "professionalId": 3,
  "customerId": 2,
  "escrowStatus": "PENDING"
}
```

---

### **GET /api/payments/{id}**

**Descripción**: Obtiene detalles de un pago

**Response (200 OK)**:
```json
{
  "id": 1,
  "contactRequestId": 1,
  "amount": 150.00,
  "description": "Reparación de tubería principal",
  "status": "COMPLETED",
  "paymentMethod": "CARD",
  "currency": "EUR",
  "createdAt": "2024-03-25T12:00:00",
  "completedAt": "2024-03-25T14:30:00",
  "transactionId": "TXN-2024-001",
  "professionalId": 3,
  "customerId": 2,
  "escrowStatus": "RELEASED"
}
```

---

## 👤 PERFIL DE USUARIO

### **GET /api/users/profile**

**Descripción**: Obtiene el perfil del usuario autenticado

**Headers**:
```
Authorization: Bearer {token}
```

**Response (200 OK)**:
```json
{
  "id": 2,
  "email": "maria@example.com",
  "fullName": "María García",
  "phone": "+34912345678",
  "city": "Madrid",
  "role": "CUSTOMER",
  "profilePhotoUrl": "https://api.example.com/photos/user-2.jpg",
  "createdAt": "2024-01-15",
  "updatedAt": "2024-03-20",
  "address": "Calle Principal 123",
  "latitude": 40.4168,
  "longitude": -3.7038
}
```

---

### **PUT /api/users/profile**

**Descripción**: Actualiza el perfil del usuario autenticado

**Headers**:
```
Authorization: Bearer {token}
```

**Request**:
```json
{
  "fullName": "María García García",
  "phone": "+34912345678",
  "city": "Barcelona",
  "address": "Calle Nueva 456",
  "latitude": 41.3874,
  "longitude": 2.1686,
  "profilePhotoUrl": "https://storage.example.com/new-photo.jpg"
}
```

**Response (200 OK)**:
```json
{
  "id": 2,
  "email": "maria@example.com",
  "fullName": "María García García",
  "phone": "+34912345678",
  "city": "Barcelona",
  "address": "Calle Nueva 456",
  "latitude": 41.3874,
  "longitude": 2.1686,
  "profilePhotoUrl": "https://storage.example.com/new-photo.jpg",
  "updatedAt": "2024-03-25T15:00:00"
}
```

---

## ⭐ RESEÑAS

### **POST /api/reviews**

**Descripción**: Crea una reseña/calificación de un profesional

**Headers**:
```
Authorization: Bearer {token}
```

**Request**:
```json
{
  "projectId": 1,
  "professionalId": 3,
  "rating": 5,
  "comment": "Excelente trabajo, muy profesional y puntual",
  "wouldRecommend": true
}
```

**Response (201 Created)**:
```json
{
  "id": 1,
  "projectId": 1,
  "professionalId": 3,
  "customerId": 2,
  "rating": 5,
  "comment": "Excelente trabajo, muy profesional y puntual",
  "wouldRecommend": true,
  "createdAt": "2024-03-25T16:00:00"
}
```

---

## 🔄 FLUJO RECOMENDADO

### **Para un Cliente (Customer)**:

1. ✅ **Registro**: `POST /api/auth/register/customer`
2. ✅ **Login**: `POST /api/auth/login`
3. ✅ **Búsqueda**: `GET /api/professionals/search?specialtyId=1&city=Madrid`
4. ✅ **Ver Detalles**: `GET /api/professionals/{id}`
5. ✅ **Contactar**: `POST /api/contact-requests`
6. ✅ **Ver Solicitudes**: `GET /api/contact-requests`
7. ✅ **Pagar**: `POST /api/payments/create`
8. ✅ **Reseña**: `POST /api/reviews`

### **Para un Profesional (Professional)**:

1. ✅ **Registro**: `POST /api/auth/register/professional`
2. ✅ **Login**: `POST /api/auth/login`
3. ✅ **Ver Solicitudes**: `GET /api/contact-requests` (recibidas)
4. ✅ **Aceptar Solicitud**: `PUT /api/contact-requests/{id}` (status: ACCEPTED)
5. ✅ **Crear Presupuesto**: `POST /api/payments/create`
6. ✅ **Ver Pagos**: `GET /api/payments` (realizados)

---

## 🛠️ CÓDIGOS DE ERROR

| Código | Significado |
|--------|-------------|
| `400` | Bad Request - Datos inválidos |
| `401` | Unauthorized - Token faltante o inválido |
| `403` | Forbidden - Sin permisos |
| `404` | Not Found - Recurso no encontrado |
| `409` | Conflict - Email ya registrado |
| `500` | Internal Server Error - Error del servidor |

---

## 📝 Notas Importantes

1. **Token JWT**: Todos los endpoints protegidos requieren un header `Authorization: Bearer {token}`
2. **CORS**: El backend debe permitir solicitudes desde `http://localhost:4200` en desarrollo
3. **Timestamps**: Todos los timestamps están en formato ISO 8601 (UTC)
4. **Paginación**: Por defecto, pageSize es 20. Máximo permitido es 100
5. **Moneda**: Por defecto en EUR. Soporta: EUR, USD, GBP
6. **Geolocalización**: Coordenadas en WGS84 (EPSG:4326)

---

**Versión**: 1.0
**Última actualización**: 28/03/2026

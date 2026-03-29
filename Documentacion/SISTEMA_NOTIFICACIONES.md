# 🔔 SISTEMA DE NOTIFICACIONES - ProServi

## 📋 Resumen Ejecutivo

El sistema de notificaciones utiliza un **combo de canales** para asegurar que los usuarios reciban alertas importantes.

### Canales Disponibles
- 📱 **App**: Notificaciones in-app (push)
- 📧 **Email**: Correo electrónico detallado
- 💬 **WhatsApp**: Mensajes por WhatsApp

---

## 👷 NOTIFICACIONES AL PROFESIONAL

### Notificación #1: NUEVA OFERTA/SOLICITUD
**Cuándo**: Usuario crea una solicitud de contacto  
**Canales**: 📱 + 📧 + 💬 (SIMULTÁNEO)

```
┌─────────────────────────────────────────────────────┐
│ 🔔 APP (Inmediato)                                  │
│ ├─ Pop-up: "Nueva solicitud de [usuario]"          │
│ ├─ Foto y datos del usuario                        │
│ ├─ Descripción del trabajo                         │
│ ├─ Método de contacto elegido                      │
│ └─ Botones: Ver detalles | Aceptar | Rechazar     │
├─────────────────────────────────────────────────────┤
│ 📧 EMAIL (1-2 minutos después)                     │
│ ├─ Asunto: 📋 Nueva solicitud de [usuario]        │
│ ├─ Contenido completo de la solicitud             │
│ ├─ Datos del usuario                              │
│ └─ Botones: Ver solicitud | Aceptar | Rechazar   │
├─────────────────────────────────────────────────────┤
│ 💬 WHATSAPP (1-2 minutos después)                  │
│ ├─ "👷 ¡NUEVA SOLICITUD!"                         │
│ ├─ Nombre usuario, especialidad, método           │
│ ├─ "⏰ Responde en las próximas 24 horas"         │
│ └─ Link: Abre la app para ver detalles            │
└─────────────────────────────────────────────────────┘
```

**Acción Esperada**: Aceptar o rechazar en máximo 24 horas

---

### Notificación #2: PAGO DEPOSITADO EN CUENTA
**Cuándo**: Se acredita el pago (después de 48h sin disputa)  
**Canales**: 📱 + 📧 + 💬

```
┌─────────────────────────────────────────────────────┐
│ 🔔 APP (Instantáneo cuando se acredita)            │
│ ├─ Popup verde: ✅ "¡Se ha depositado tu pago!"   │
│ ├─ Monto neto recibido                            │
│ ├─ Proyecto #ID                                    │
│ ├─ Nombre del cliente                             │
│ ├─ Especialidad del trabajo                       │
│ └─ Botones: Ver detalles | Ver proyecto           │
├─────────────────────────────────────────────────────┤
│ 📧 EMAIL                                           │
│ ├─ Asunto: 💰 Pago depositado - Proyecto #[ID]  │
│ ├─ Desglose de pago:                             │
│ │  • Monto bruto (total trabajo)                  │
│ │  • Comisión plataforma (-8%)                    │
│ │  • Monto neto a recibir                         │
│ ├─ Datos del cliente y trabajo                    │
│ ├─ Número de transacción                         │
│ ├─ Disponibilidad: "1-2 días hábiles"            │
│ └─ Botón: Ver factura/detalles                   │
├─────────────────────────────────────────────────────┤
│ 💬 WHATSAPP                                        │
│ └─ "💵 Tu pago de $XXX ha sido depositado!       │
│    Disponible en tu banco en 1-2 días hábiles"   │
└─────────────────────────────────────────────────────┘
```

**Acción Esperada**: Ninguna (informativa)

---

## 👤 NOTIFICACIONES AL USUARIO

### Notificación #1: PROFESIONAL CONFIRMÓ LA PROPUESTA
**Cuándo**: Profesional acepta la solicitud de contacto  
**Canales**: 📱 + 📧 (SIN WhatsApp para evitar spam)

```
┌─────────────────────────────────────────────────────┐
│ 🔔 APP (Instantáneo)                               │
│ ├─ Popup verde: ✅ "¡[Profesional] ha aceptado!"  │
│ ├─ Foto y nombre del profesional                  │
│ ├─ Propuesta actualizada                          │
│ ├─ Si hay cambios: mostrar nuevo precio/costo    │
│ └─ Botones: Ver detalles | Confirmar y proceder  │
├─────────────────────────────────────────────────────┤
│ 📧 EMAIL                                           │
│ ├─ Asunto: ✅ [Profesional] ha aceptado tu       │
│ │           solicitud                             │
│ ├─ Detalles de la propuesta actualizada          │
│ ├─ Información del profesional                   │
│ ├─ Próximos pasos                                │
│ └─ Botón: Ver propuesta completa                 │
└─────────────────────────────────────────────────────┘
```

**Acción Esperada**: Confirmar propuesta y proceder (o rechazar)

---

### Notificación #2: PROFESIONAL CONFIRMÓ TERMINACIÓN
**Cuándo**: Profesional marca el trabajo como completado  
**Canales**: 📱 + 📧 + 💬 (según preferencia del usuario)

```
┌─────────────────────────────────────────────────────┐
│ 🔔 APP (Instantáneo)                               │
│ ├─ "[Profesional] ha completado el trabajo"       │
│ ├─ Galería de fotos del resultado final           │
│ ├─ Detalles de lo realizado                       │
│ ├─ Instrucciones de mantenimiento (si aplica)     │
│ ├─ ⭐ IMPORTANTE: Opción para reportar si no está  │
│ │    satisfecho ANTES de pagar                    │
│ └─ Botones: Revisar | Confirmar | Reportar       │
├─────────────────────────────────────────────────────┤
│ 📧 EMAIL                                           │
│ ├─ Asunto: ✔️ [Profesional] ha completado tu     │
│ │           trabajo                              │
│ ├─ Galería de fotos                              │
│ ├─ Resumen de lo realizado                       │
│ ├─ Instrucciones de mantenimiento                │
│ └─ Botón: Confirmar en la app                    │
├─────────────────────────────────────────────────────┤
│ 💬 WHATSAPP (SOLO si usuario lo activó)          │
│ └─ "El trabajo está listo. Abre la app para      │
│    revisar las fotos y detalles."                │
└─────────────────────────────────────────────────────┘
```

**Acción Esperada**: Confirmar satisfacción O reportar problema (ANTES de pagar)

---

## 📊 Tabla Resumen por Etapa

| Evento | Profesional | Usuario | Canales |
|--------|-------------|---------|---------|
| **Usuario crea solicitud** | ✅ Oferta | — | 📱+📧+💬 |
| **Prof. ACEPTA** | — | ✅ Confirmó | 📱+📧 |
| **Prof. termina trabajo** | — | ✅ Completó | 📱+📧+💬* |
| **Usuario confirma + Pago acreditado** | ✅ ¡Pago! | — | 📱+📧+💬 |

\* = Solo si usuario lo habilitó en preferencias

---

## ⚙️ Configuración de Preferencias

### Para Todos los Usuarios
- Notificaciones en app: ✓ ON/OFF
- Notificaciones por email: ✓ ON/OFF
- Notificaciones por WhatsApp: ✓ ON/OFF
- Horario de silencio: [HH:MM] a [HH:MM]
- Frecuencia: Inmediato / Diario / Semanal

### Específico del Profesional
- Alertas de nuevas solicitudes: TODAS / Por especialidad / NINGUNA
- Alertas de pago: SIEMPRE (no se puede desactivar)

### Específico del Usuario
- Alertas de aceptación: SIEMPRE (no se puede desactivar)
- Alertas de finalización: SIEMPRE (no se puede desactivar)

---

## 🎯 Consideraciones de Diseño

### ✅ Por qué este combo de canales:

1. **Cobertura**: Algunos prefieren app, otros email, otros WhatsApp
2. **Crítico para profesionales**: Nueva solicitud = COMBO (no pueden perderla)
3. **Importante para usuario**: Confirmación y terminación = COMBO
4. **Pago**: Siempre COMBO para profesional (es dinero)
5. **Evitar spam**: Usuario NO recibe WhatsApp en aceptación (app+email es suficiente)

### ⚠️ Casos especiales:

**Si profesional RECHAZA**: Notificación al usuario NO es urgente
- Solo app y email (sin WhatsApp)
- Se sugieren alternativas en la app

**Si hay DISPUTA**: Ambos reciben notificación especial
- Más información en la app
- Email con detalles de la disputa
- WhatsApp: "Se ha reportado un problema con tu proyecto"

---

## 🚀 Implementación

### Fase 1 (MVP)
```
✓ Notificación en app (push)
✓ Email básico con detalles
✓ Manual de WhatsApp (usuario envía numero en registro)
```

### Fase 2
```
✓ Integración con API WhatsApp oficial
✓ Configuración de preferencias
✓ Horarios de silencio
```

### Fase 3
```
✓ Smart notifications (machine learning para horarios óptimos)
✓ Digestos (agrupar notificaciones si hay varias)
✓ Notificaciones inteligentes (contextuales)
```

---

**Documento Actualizado**: 28/03/2026  
**Versión**: 1.0 (Sistema de notificaciones completo)

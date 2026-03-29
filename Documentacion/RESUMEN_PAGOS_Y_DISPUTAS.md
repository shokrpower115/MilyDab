# 💳 SISTEMA DE PAGOS Y DISPUTAS - RESUMEN EJECUTIVO

## 📊 Flujo de Pagos Actualizado

### Fase 1: Presupuesto General (Cuando crea solicitud)
```
Usuario completa formulario
    ↓
Sistema crea PRESUPUESTO GENERAL
├─ Si es VISITA PRESENCIAL:
│  └─ Se añade costo de visita al presupuesto
│     └─ Usuario ve advertencia: "Se cobrará $XX por visita"
│        └─ Se registra tarjeta (Auth de $1 para validar)
│
└─ Si es otro método (llamada, cotización, videollamada):
   └─ GRATIS en esta etapa
   └─ No se cobra nada aún
```

### Fase 2: Aceptación del Profesional
```
Profesional acepta solicitud
    ↓
├─ Si es VISITA PRESENCIAL:
│  ├─ Costo de visita: SE COBRA AHORA (con tarjeta registrada)
│  └─ Fondos en escrow por 48 horas
│
└─ Si es otro método:
   └─ NO se cobra nada
   └─ Se espera a que terminen y confirmen el trabajo
```

### Fase 3: Ejecución y Completación
```
Ambos confirman trabajo completado
    ↓
Sistema calcula PAGO FINAL:
├─ Trabajos VISITADOS:
│  └─ Costo de visita (YA PAGADO antes)
│  └─ Costo del trabajo NUEVO
│  └─ Comisión plataforma (8% del total)
│
└─ Trabajos SIN VISITA (llamada, cotización):
   └─ Costo del trabajo NUEVO
   └─ Comisión plataforma (8%)
```

### Fase 4: Liquidación
```
Usuario paga costo del trabajo
    ↓
Fondos en escrow ProServi por 48 horas
    ↓
├─ Si NO hay disputa:
│  └─ Fondos se acreditan a profesional (24-48h)
│
└─ Si hay disputa:
   └─ Fondos retenidos mientras se resuelve
   └─ Máximo 7 días de investigación
```

---

## 💰 Métodos de Pago Permitidos

### 1️⃣ TARJETA DE CRÉDITO/DÉBITO
**Integración**: API del Banco (Stripe, Mercado Pago, Openpay)

**Flujo**:
- Usuario ingresa datos: Número, vencimiento, CVV, nombre
- Validación: Charge de $1 USD (se devuelve en 1-2 días)
- Confirmación: Recibo por email
- Para próximas compras: Opción de "Usar tarjeta guardada" con PIN/biometría

**Ventajas**:
- ✅ Instantáneo
- ✅ Seguro (cifrado PCI)
- ✅ Trazabilidad completa

### 2️⃣ TRANSFERENCIA BANCARIA
**Flujo**:
- Se muestran datos bancarios del profesional
- Usuario realiza transferencia manual desde su banco
- Espera de 24-48 horas para confirmación
- Sistema recibe notificación y libera fondos

**Ventajas**:
- ✅ Usuario controla proceso
- ✅ No requiere dato sensible en app
- ✅ Alternativa para quien no tiene tarjeta

**Desventajas**:
- ❌ Más lento
- ❌ Mayor riesgo de error

---

## 🚨 SISTEMA DE REPORTES Y DISPUTAS

### Escenarios Comunes

| Problema | Usuario puede reportar | Tiempo límite | Resolución |
|----------|----------------------|--------------|-----------|
| Trabajo incompleto | Antes de confirmar O dentro de 7 días | 7 días | Automática (si hay prueba) |
| Profesional no responde | Después de 48h sin contacto | 48h | Manual (equipo) |
| Precio mayor a lo acordado | Antes de pagar o dentro de 48h | 48h | Automática |
| Trabajo no realizado | Antes de pagar o inmediatamente | Inmediato | Reembolso automático |
| Pago duplicado | Inmediatamente | 24h | Automática (reversa) |
| Información falsa profesional | En cualquier momento | Indefinido | Manual + bloqueo |

---

## 🎯 OPCIÓN RECOMENDADA: SISTEMA HÍBRIDO (Opción C)

### Capa 1: PREVENCIÓN (75% de problemas se evitan)
- ✅ Presupuesto detallado aprobado por ambas partes
- ✅ Confirmación explícita antes de pagar
- ✅ Sistema de reputación (bloquea usuarios con mala historia)
- ✅ Feedback inmediato durante ejecución

### Capa 2: PROTECCIÓN CON ESCROW INTELIGENTE

```
Trabajos por MONTO:

┌─────────────────────────┬──────────────┬─────────────────────┐
│ RANGO DE PRECIO         │ RETENCIÓN    │ LIBERACIÓN          │
├─────────────────────────┼──────────────┼─────────────────────┤
│ Menos de $50            │ NINGUNA      │ Inmediata           │
│ (asesoría, consulta)    │ (riesgo bajo)│ Usuario reporta     │
│                         │              │ en 7 días si quiere │
├─────────────────────────┼──────────────┼─────────────────────┤
│ $50 - $300              │ 48 horas     │ Automática si no    │
│ (trabajos pequeños)     │ (escrow)     │ hay disputa         │
├─────────────────────────┼──────────────┼─────────────────────┤
│ Más de $300             │ 7 días       │ Hitos de trabajo:   │
│ (trabajos grandes)      │ (máximo)     │ 50% inicio, 50% fin │
└─────────────────────────┴──────────────┴─────────────────────┘
```

### Capa 3: RESOLUCIÓN DE DISPUTAS

**Bot Automático resuelve** (80% de casos):
- ✅ Profesional rechazó sin motivo → Reembolso
- ✅ Nunca hubo contacto (logs lo prueban) → Reembolso
- ✅ Trabajo incompleto (fotos vs. descripción) → Reembolso
- ✅ Pago duplicado → Reversa automática

**Equipo Humano resuelve** (20% de casos):
- ❓ Calidad subjetiva ("No me gustó")
- ❓ Conflicto de precios discutible
- ❓ Ambigüedad en acuerdo

---

## 📋 Implementación en 3 Fases

### FASE 1: MVP (Semana 1-2)
```
✓ Integración API de pagos (tarjeta)
✓ Escrow básico (48 horas retención)
✓ Bot de resolución automática (casos obvios)
✓ Email de confirmación de pago
```

### FASE 2: Escalabilidad (Semana 3-4)
```
✓ Transferencia bancaria integrada
✓ Categorización automática de disputas
✓ Dashboard de disputes (admin)
✓ Notificaciones de pago en escrow
```

### FASE 3: Madurez (Mes 2)
```
✓ Equipo humano para resolución manual
✓ Sistema de apelaciones
✓ Reportes de transacciones (impuestos)
✓ Integración con contabilidad
```

---

## 🔒 Medidas de Seguridad

### Validación de Tarjetas
- ✅ PCI DSS Compliance (no almacenar datos crudos)
- ✅ Tokenización mediante API del banco
- ✅ Verificación de $1 USD antes de charge real
- ✅ CVV requerido en cada transacción (o mediante tokenización segura)

### Anti-Fraude
- ✅ Detección de múltiples charques fallidos
- ✅ Bloqueo de usuario después de 3 intentos
- ✅ Verificación de IP y dispositivo
- ✅ Límites de transacción diaria por usuario

### Transparencia
- ✅ Recibos detallados por email
- ✅ Historial de transacciones en app
- ✅ Desglose claro de comisiones
- ✅ Descarga de facturas (para profesionales)

---

## 📞 Contacto Soporte

- Disputa en tránsito: equipo responde en 24h
- Problema con pago: reversión en máximo 48h
- Bloqueo de cuenta: apelación en 72h

---

**Documento Actualizado**: 28/03/2026
**Versión**: 2.0 (Con sistema de pagos y disputas detallado)

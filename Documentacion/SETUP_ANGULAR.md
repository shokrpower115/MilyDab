# 🎨 GUÍA: CREAR PROYECTO ANGULAR

## Paso 1: Crear proyecto Angular con Tailwind

```powershell
# Navegar a la carpeta frontend
cd c:\dev\ProServi\frontend

# Crear proyecto Angular (si no existe)
ng new . --routing --style=css --skip-git=true

# Instalar Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# Instalar dependencias adicionales
npm install axios
npm install @ngx-translate/core @ngx-translate/http-loader
npm install jwt-decode
```

---

## Paso 2: Configurar Tailwind

### **tailwind.config.js**
```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        // Ajusta estos colores según tu paleta-colores.jfif
        primary: {
          50: '#f0f9ff',
          100: '#e0f2fe',
          200: '#bae6fd',
          300: '#7dd3fc',
          400: '#38bdf8',
          500: '#0ea5e9',  // Color primario principal
          600: '#0284c7',
          700: '#0369a1',
          800: '#075985',
          900: '#0c3d66',
        },
        secondary: {
          50: '#f5f3ff',
          100: '#ede9fe',
          500: '#8b5cf6',
          700: '#6d28d9',
        },
        success: {
          500: '#10b981',
          700: '#059669',
        },
        warning: {
          500: '#f59e0b',
          700: '#d97706',
        },
        danger: {
          500: '#ef4444',
          700: '#dc2626',
        },
      },
      fontFamily: {
        sans: ['Segoe UI', 'Roboto', 'sans-serif'],
      },
    },
  },
  plugins: [],
}
```

### **src/styles.css**
```css
@tailwind base;
@tailwind components;
@tailwind utilities;

/* Variables CSS personalizadas */
:root {
  --primary-color: #0ea5e9;
  --secondary-color: #8b5cf6;
  --success-color: #10b981;
  --warning-color: #f59e0b;
  --danger-color: #ef4444;
}

/* Animaciones globales */
@layer components {
  .btn-primary {
    @apply px-4 py-2 rounded-lg bg-primary-500 text-white font-semibold hover:bg-primary-700 transition;
  }

  .btn-secondary {
    @apply px-4 py-2 rounded-lg border-2 border-secondary-500 text-secondary-500 font-semibold hover:bg-secondary-50 transition;
  }

  .card {
    @apply bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition;
  }

  .input-field {
    @apply w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:border-primary-500 focus:ring-2 focus:ring-primary-200;
  }
}
```

---

## Paso 3: Crear estructura de carpetas

```powershell
# Crear carpetas principales
mkdir -p src/app/core/guards
mkdir -p src/app/core/interceptors
mkdir -p src/app/core/services
mkdir -p src/app/modules/auth/components/{login,register,role-selector}
mkdir -p src/app/modules/auth/services
mkdir -p src/app/modules/home/components/{dashboard,navbar,search-bar}
mkdir -p src/app/modules/home/services
mkdir -p src/app/modules/shared/components/{button,card,modal}
mkdir -p src/app/modules/shared/pipes
mkdir -p src/assets/{images,icons,logo}
```

---

## Paso 4: Crear servicios principales

### **src/app/core/services/auth.service.ts**
```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import * as jwt_decode from 'jwt-decode';

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
  fullName: string;
  email: string;
  phone: string;
  city: string;
  profession: string;
  yearsExperience: number;
  descriptionServices: string;
  documentNumber: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: UserData;
}

export interface UserData {
  id: number;
  email: string;
  fullName: string;
  role: 'CUSTOMER' | 'PROFESSIONAL';
  profilePhotoUrl?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:5001/api/auth';
  private currentUserSubject = new BehaviorSubject<UserData | null>(this.getUserFromLocalStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        map(response => {
          this.setToken(response.token);
          this.currentUserSubject.next(response.user);
          return response;
        })
      );
  }

  registerCustomer(data: RegisterCustomerRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register/customer`, data)
      .pipe(
        map(response => {
          this.setToken(response.token);
          this.currentUserSubject.next(response.user);
          return response;
        })
      );
  }

  registerProfessional(data: RegisterProfessionalRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register/professional`, data)
      .pipe(
        map(response => {
          this.setToken(response.token);
          this.currentUserSubject.next(response.user);
          return response;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): UserData | null {
    return this.currentUserSubject.value;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  private setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  private getUserFromLocalStorage(): UserData | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwt_decode.jwtDecode(token);
      return {
        id: decoded.id,
        email: decoded.email,
        fullName: decoded.fullName,
        role: decoded.role
      };
    } catch {
      return null;
    }
  }
}
```

### **src/app/core/guards/auth.guard.ts**
```typescript
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.router.navigate(['/auth/role-selector']);
    return false;
  }
}
```

### **src/app/core/guards/not-auth.guard.ts**
```typescript
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class NotAuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this.authService.isLoggedIn()) {
      return true;
    }

    this.router.navigate(['/home']);
    return false;
  }
}
```

### **src/app/core/interceptors/auth.interceptor.ts**
```typescript
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.authService.getToken();
    
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request);
  }
}
```

---

## Paso 5: Crear rutas

### **src/app/app-routing.module.ts**
```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotAuthGuard } from './core/guards/not-auth.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
    canActivate: [NotAuthGuard]
  },
  {
    path: 'home',
    loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule),
    canActivate: [AuthGuard]
  },
  {
    path: '',
    redirectTo: '/auth/role-selector',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/auth/role-selector'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

### **src/app/modules/auth/auth-routing.module.ts**
```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleSelectorComponent } from './components/role-selector/role-selector.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterCustomerComponent } from './components/register/register-customer.component';
import { RegisterProfessionalComponent } from './components/register/register-professional.component';

const routes: Routes = [
  { path: 'role-selector', component: RoleSelectorComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register/customer', component: RegisterCustomerComponent },
  { path: 'register/professional', component: RegisterProfessionalComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
```

---

## Paso 6: Crear módulos

### **src/app/modules/auth/auth.module.ts**
```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';

import { RoleSelectorComponent } from './components/role-selector/role-selector.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterCustomerComponent } from './components/register/register-customer.component';
import { RegisterProfessionalComponent } from './components/register/register-professional.component';

@NgModule({
  declarations: [
    RoleSelectorComponent,
    LoginComponent,
    RegisterCustomerComponent,
    RegisterProfessionalComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
```

---

## Paso 7: Crear primer componente (Role Selector)

### **src/app/modules/auth/components/role-selector/role-selector.component.ts**
```typescript
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-role-selector',
  templateUrl: './role-selector.component.html',
  styleUrls: ['./role-selector.component.css']
})
export class RoleSelectorComponent {
  constructor(private router: Router) {}

  selectRole(role: 'customer' | 'professional'): void {
    if (role === 'customer') {
      this.router.navigate(['/auth/register/customer']);
    } else {
      this.router.navigate(['/auth/register/professional']);
    }
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}
```

### **src/app/modules/auth/components/role-selector/role-selector.component.html**
```html
<div class="flex flex-col items-center justify-center min-h-screen bg-gradient-to-b from-primary-50 to-white">
  <!-- Logo -->
  <img src="assets/logo/logo.png" alt="ProServi" class="w-32 mb-8" *ngIf="false">
  <div class="w-32 h-32 bg-primary-500 rounded-full flex items-center justify-center mb-8">
    <span class="text-white text-4xl">PS</span>
  </div>

  <!-- Título -->
  <h1 class="text-4xl font-bold text-gray-800 mb-2">¡Bienvenido a ProServi!</h1>
  <p class="text-gray-600 mb-12 text-lg">Conecta con los mejores profesionales de tu zona</p>

  <!-- Opciones -->
  <div class="grid grid-cols-1 md:grid-cols-2 gap-6 w-full max-w-2xl px-4 mb-8">
    <!-- Card: Buscar Profesionales -->
    <div class="card cursor-pointer hover:shadow-2xl transition transform hover:scale-105"
         (click)="selectRole('customer')">
      <div class="text-6xl mb-4 text-center">🔍</div>
      <h2 class="text-2xl font-bold text-gray-800 mb-2 text-center">Buscar Profesionales</h2>
      <p class="text-gray-600 text-center">Encuentra expertos en tu área cercanos a ti</p>
      <button class="btn-primary w-full mt-4">Continuar</button>
    </div>

    <!-- Card: Ofrecer Servicios -->
    <div class="card cursor-pointer hover:shadow-2xl transition transform hover:scale-105"
         (click)="selectRole('professional')">
      <div class="text-6xl mb-4 text-center">🛠️</div>
      <h2 class="text-2xl font-bold text-gray-800 mb-2 text-center">Ofrecer Mis Servicios</h2>
      <p class="text-gray-600 text-center">Regístrate como profesional y recibe solicitudes</p>
      <button class="btn-primary w-full mt-4">Continuar</button>
    </div>
  </div>

  <!-- Link para login -->
  <p class="text-gray-600">
    ¿Ya tienes cuenta?
    <a (click)="goToLogin()" class="text-primary-600 font-bold hover:underline cursor-pointer">
      Inicia sesión
    </a>
  </p>
</div>
```

---

## Paso 8: Actualizar App Component

### **src/app/app.component.ts**
```typescript
import { Component } from '@angular/core';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public authService: AuthService) {}
}
```

### **src/app/app.component.html**
```html
<router-outlet></router-outlet>
```

---

## Paso 9: Actualizar main.ts

### **src/main.ts**
```typescript
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

platformBrowserDynamic()
  .bootstrapModule(AppModule)
  .catch(err => console.error(err));
```

---

## Paso 10: Actualizar app.module.ts

### **src/app/app.module.ts**
```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

---

## ✅ Checklist

- [ ] Proyecto Angular creado
- [ ] Tailwind CSS instalado y configurado
- [ ] Estructura de carpetas creada
- [ ] AuthService creado
- [ ] Guards creados (AuthGuard, NotAuthGuard)
- [ ] AuthInterceptor creado
- [ ] Rutas configuradas
- [ ] Módulos creados
- [ ] Role Selector component creado
- [ ] App Component actualizado

---

## 🚀 Ejecutar la aplicación

```powershell
cd c:\dev\ProServi\frontend
ng serve --open
```

La aplicación estará disponible en: `http://localhost:4200`

---

**Guía Creada**: 28/03/2026

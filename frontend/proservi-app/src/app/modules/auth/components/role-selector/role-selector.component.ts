import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-role-selector',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="d-flex flex-column align-items-center justify-content-center min-vh-100 bg-light">
      <!-- Logo -->
      <div class="avatar-circle" style="width: 150px; height: 150px; font-size: 3rem; margin-bottom: 2rem;">
        <span>PS</span>
      </div>

      <!-- Título -->
      <h1 class="display-4 fw-bold text-dark mb-2 text-center">¡Bienvenido a ProServi!</h1>
      <p class="text-muted mb-5 fs-5 text-center">Conecta con los mejores profesionales de tu zona</p>

      <!-- Opciones -->
      <div class="row w-100" style="max-width: 900px; margin-bottom: 2rem; padding: 0 1rem;">
        <!-- Card: Buscar Profesionales -->
        <div class="col-12 col-md-6 mb-4 mb-md-0 d-flex">
          <div class="card card-custom w-100 cursor-pointer"
               (click)="selectRole('customer')">
            <div class="card-body text-center">
              <div style="font-size: 3rem; margin-bottom: 1rem;">🔍</div>
              <h2 class="h4 fw-bold text-dark mb-2">Buscar Profesionales</h2>
              <p class="text-muted">Encuentra expertos en tu área cercanos a ti</p>
              <button class="btn btn-primary-custom w-100 mt-3">Continuar</button>
            </div>
          </div>
        </div>

        <!-- Card: Ofrecer Servicios -->
        <div class="col-12 col-md-6 d-flex">
          <div class="card card-custom w-100 cursor-pointer"
               (click)="selectRole('professional')">
            <div class="card-body text-center">
              <div style="font-size: 3rem; margin-bottom: 1rem;">🛠️</div>
              <h2 class="h4 fw-bold text-dark mb-2">Ofrecer Mis Servicios</h2>
              <p class="text-muted">Regístrate como profesional y recibe solicitudes</p>
              <button class="btn btn-primary-custom w-100 mt-3">Continuar</button>
            </div>
          </div>
        </div>
      </div>

      <!-- Link para login -->
      <p class="text-muted">
        ¿Ya tienes cuenta?
        <a (click)="goToLogin()" class="fw-bold cursor-pointer">
          Inicia sesión
        </a>
      </p>
    </div>
  `,
  styles: []
})
export class RoleSelectorComponent {
  constructor(private router: Router) {}

  selectRole(role: 'customer' | 'professional'): void {
    if (role === 'customer') {
      this.router.navigate(['/auth/register-customer']);
    } else {
      this.router.navigate(['/auth/register-professional']);
    }
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}

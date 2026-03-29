import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { LoginRequest } from '../../../../shared/models/auth.models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="d-flex align-items-center justify-content-center min-vh-100 bg-light">
      <div class="w-100" style="max-width: 450px; padding: 0 1rem;">
        <!-- Card -->
        <div class="card card-custom">
          <div class="card-body">
            <div class="text-center mb-5">
              <div class="avatar-circle" style="width: 120px; height: 120px; font-size: 2.5rem; margin: 0 auto 1rem;">
                <span>PS</span>
              </div>
              <h1 class="h3 fw-bold text-dark">ProServi</h1>
              <p class="text-muted mt-2">Inicia sesión en tu cuenta</p>
            </div>

            <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
              <!-- Error Message -->
              <div *ngIf="errorMessage" class="alert alert-custom-error mb-4" role="alert">
                {{ errorMessage }}
              </div>

              <!-- Email -->
              <div class="mb-3">
                <label for="email" class="form-label fw-semibold">Email</label>
                <input
                  type="email"
                  formControlName="email"
                  class="form-control"
                  placeholder="tu@email.com"
                />
                <p *ngIf="isFieldInvalid('email')" class="text-danger small mt-1">Email requerido y válido</p>
              </div>

              <!-- Password -->
              <div class="mb-4">
                <label for="password" class="form-label fw-semibold">Contraseña</label>
                <input
                  type="password"
                  formControlName="password"
                  class="form-control"
                  placeholder="••••••••"
                />
                <p *ngIf="isFieldInvalid('password')" class="text-danger small mt-1">Contraseña requerida (mínimo 8 caracteres)</p>
              </div>

              <!-- Submit Button -->
              <button
                type="submit"
                [disabled]="!loginForm.valid || isLoading"
                class="btn btn-primary-custom w-100"
              >
                {{ isLoading ? 'Iniciando sesión...' : 'Inicia Sesión' }}
              </button>
            </form>

            <!-- Footer -->
            <div class="text-center mt-4">
              <p class="text-muted">
                ¿No tienes cuenta?
                <a (click)="goToRoleSelector()" class="fw-bold cursor-pointer">
                  Regístrate aquí
                </a>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmit(): void {
    if (!this.loginForm.valid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const credentials: LoginRequest = this.loginForm.value;

    this.authService.login(credentials).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Error al iniciar sesión. Verifica tus credenciales.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.loginForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  goToRoleSelector(): void {
    this.router.navigate(['/auth/role-selector']);
  }
}

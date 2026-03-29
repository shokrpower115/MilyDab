import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { RegisterCustomerRequest } from '../../../../shared/models/auth.models';

@Component({
  selector: 'app-register-customer',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="min-vh-100 bg-light py-5 px-3">
      <div class="w-100" style="max-width: 900px; margin: 0 auto;">
        <!-- Card -->
        <div class="card card-custom">
          <div class="card-body">
            <div class="text-center mb-4">
              <h1 class="h3 fw-bold text-dark">Regístrate como Cliente</h1>
              <p class="text-muted mt-2">Busca y conecta con los mejores profesionales</p>
            </div>

            <form [formGroup]="registerForm" (ngSubmit)="onSubmit()">
              <!-- Error Message -->
              <div *ngIf="errorMessage" class="alert alert-custom-error mb-4" role="alert">
                {{ errorMessage }}
              </div>

              <!-- Grid para 2 columnas -->
              <div class="row g-3">
              <!-- Full Name -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Nombre completo</label>
                <input
                  type="text"
                  formControlName="fullName"
                  class="form-control"
                  placeholder="Tu nombre"
                />
                <p *ngIf="isFieldInvalid('fullName')" class="text-danger small mt-1">Nombre requerido</p>
              </div>

              <!-- Email -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Email</label>
                <input
                  type="email"
                  formControlName="email"
                  class="form-control"
                  placeholder="tu@email.com"
                />
                <p *ngIf="isFieldInvalid('email')" class="text-danger small mt-1">Email válido requerido</p>
              </div>

              <!-- Phone -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Teléfono</label>
                <input
                  type="tel"
                  formControlName="phone"
                  class="form-control"
                  placeholder="+34 612345678"
                />
                <p *ngIf="isFieldInvalid('phone')" class="text-danger small mt-1">Teléfono requerido</p>
              </div>

              <!-- City -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Ciudad</label>
                <input
                  type="text"
                  formControlName="city"
                  class="form-control"
                  placeholder="Madrid"
                />
                <p *ngIf="isFieldInvalid('city')" class="text-danger small mt-1">Ciudad requerida</p>
              </div>

              <!-- Country -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">País</label>
                <input
                  type="text"
                  formControlName="country"
                  class="form-control"
                  placeholder="Spain"
                />
                <p *ngIf="isFieldInvalid('country')" class="text-danger small mt-1">País requerido</p>
              </div>

              <!-- Latitude -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Latitud</label>
                <input
                  type="number"
                  step="0.0001"
                  formControlName="latitude"
                  class="form-control"
                  placeholder="40.4168"
                />
                <p *ngIf="isFieldInvalid('latitude')" class="text-danger small mt-1">Latitud requerida</p>
              </div>

              <!-- Longitude -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Longitud</label>
                <input
                  type="number"
                  step="0.0001"
                  formControlName="longitude"
                  class="form-control"
                  placeholder="-3.7038"
                />
                <p *ngIf="isFieldInvalid('longitude')" class="text-danger small mt-1">Longitud requerida</p>
              </div>

              <!-- Password -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Contraseña</label>
                <input
                  type="password"
                  formControlName="password"
                  class="form-control"
                  placeholder="••••••••"
                />
                <p *ngIf="isFieldInvalid('password')" class="text-danger small mt-1">Mínimo 8 caracteres</p>
              </div>

              <!-- Confirm Password -->
              <div class="col-12 col-md-6">
                <label class="form-label fw-semibold">Confirmar contraseña</label>
                <input
                  type="password"
                  formControlName="confirmPassword"
                  class="form-control"
                  placeholder="••••••••"
                />
                <p *ngIf="isFieldInvalid('confirmPassword')" class="text-danger small mt-1">Confirma tu contraseña</p>
              </div>
              </div>

              <!-- Password Match Error -->
              <div *ngIf="registerForm.hasError('passwordMismatch')" class="text-danger small mt-2">
                Las contraseñas no coinciden
              </div>

              <!-- Submit Button -->
              <button
                type="submit"
                [disabled]="!registerForm.valid || isLoading"
                class="btn btn-primary-custom w-100 mt-4"
            >
              {{ isLoading ? 'Registrando...' : 'Crear Cuenta' }}
            </button>
          </form>

          <!-- Footer -->
          <div class="text-center mt-4">
            <p class="text-muted">
              ¿Ya tienes cuenta?
              <a (click)="goToLogin()" class="fw-bold cursor-pointer">
                Inicia sesión
              </a>
            </p>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class RegisterCustomerComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group(
      {
        fullName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phone: ['', Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        latitude: [null, Validators.required],
        longitude: [null, Validators.required],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required]
      },
      { validators: this.passwordMatcher }
    );
  }

  passwordMatcher(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;

    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }

    return null;
  }

  onSubmit(): void {
    if (!this.registerForm.valid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const userData: RegisterCustomerRequest = this.registerForm.value;

    this.authService.registerCustomer(userData).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Error al registrarse. Intenta de nuevo.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}

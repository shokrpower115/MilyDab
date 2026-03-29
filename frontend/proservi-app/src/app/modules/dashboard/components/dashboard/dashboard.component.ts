import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { UserData } from '../../../../shared/models/auth.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="min-vh-100 bg-light">
      <!-- Navbar -->
      <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm mb-4">
        <div class="container-fluid">
          <a class="navbar-brand fw-bold" [routerLink]="['/dashboard']">
            <div class="avatar-circle d-inline-block me-2" style="width: 40px; height: 40px; font-size: 1.2rem;">
              PS
            </div>
            ProServi
          </a>
          <div class="d-flex align-items-center ms-auto">
            <span class="me-3 text-dark fw-semibold" *ngIf="currentUser">
              {{ currentUser.fullName }}
            </span>
            <span class="badge bg-primary-custom me-3" *ngIf="currentUser">
              {{ currentUser.role }}
            </span>
            <button (click)="logout()" class="btn btn-primary-custom btn-sm">
              Cerrar Sesión
            </button>
          </div>
        </div>
      </nav>

      <!-- Main Content -->
      <div class="container-fluid py-4">
        <div class="row">
          <!-- Welcome Card -->
          <div class="col-12 mb-4">
            <div class="card card-custom">
              <div class="card-body">
                <h2 class="h4 fw-bold mb-2">¡Bienvenido, {{ currentUser?.fullName }}!</h2>
                <p class="text-muted mb-0">
                  Eres un <strong>{{ currentUser?.role }}</strong>
                  <span *ngIf="currentUser?.role === 'Professional'"> y tus servicios están disponibles para los clientes</span>
                  <span *ngIf="currentUser?.role === 'Customer'"> y puedes buscar profesionales</span>
                </p>
              </div>
            </div>
          </div>

          <!-- Quick Actions -->
          <div class="col-12 mb-4">
            <h3 class="h5 fw-bold mb-3">Acciones Rápidas</h3>
            <div class="row g-3">
              <div class="col-12 col-md-6 col-lg-3">
                <div class="card card-custom">
                  <div class="card-body text-center">
                    <div style="font-size: 2rem; margin-bottom: 1rem;">🔍</div>
                    <h5 class="card-title fw-bold">Buscar Profesionales</h5>
                    <p class="text-muted small mb-3">Encuentra expertos en tu área</p>
                    <button class="btn btn-sm btn-primary-custom" *ngIf="currentUser?.role === 'Customer'">
                      Ver Disponibles
                    </button>
                    <span class="text-muted small" *ngIf="currentUser?.role === 'Professional'">
                      (No disponible para profesionales)
                    </span>
                  </div>
                </div>
              </div>

              <div class="col-12 col-md-6 col-lg-3">
                <div class="card card-custom">
                  <div class="card-body text-center">
                    <div style="font-size: 2rem; margin-bottom: 1rem;">📋</div>
                    <h5 class="card-title fw-bold">Mis Solicitudes</h5>
                    <p class="text-muted small mb-3">
                      <span *ngIf="currentUser?.role === 'Customer'">Solicitudes enviadas</span>
                      <span *ngIf="currentUser?.role === 'Professional'">Solicitudes recibidas</span>
                    </p>
                    <button class="btn btn-sm btn-primary-custom">Ver Listado</button>
                  </div>
                </div>
              </div>

              <div class="col-12 col-md-6 col-lg-3">
                <div class="card card-custom">
                  <div class="card-body text-center">
                    <div style="font-size: 2rem; margin-bottom: 1rem;">💰</div>
                    <h5 class="card-title fw-bold">Pagos</h5>
                    <p class="text-muted small mb-3">Gestiona tus transacciones</p>
                    <button class="btn btn-sm btn-primary-custom">Ver Historial</button>
                  </div>
                </div>
              </div>

              <div class="col-12 col-md-6 col-lg-3">
                <div class="card card-custom">
                  <div class="card-body text-center">
                    <div style="font-size: 2rem; margin-bottom: 1rem;">⚙️</div>
                    <h5 class="card-title fw-bold">Perfil</h5>
                    <p class="text-muted small mb-3">Edita tu información</p>
                    <button class="btn btn-sm btn-primary-custom">Ir a Perfil</button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Info Section -->
          <div class="col-12">
            <div class="alert alert-info d-flex align-items-center" role="alert">
              <svg class="bi flex-shrink-0 me-2" width="24" height="24" viewBox="0 0 16 16" fill="currentColor">
                <path d="m8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533L8.93 6.588zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0z"/>
              </svg>
              <div>
                Esta es una versión beta de ProServi. Estamos continuamente mejorando la plataforma.
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class DashboardComponent implements OnInit {
  currentUser: UserData | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/role-selector']);
  }
}

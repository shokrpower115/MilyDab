import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { UserData } from '../../shared/models/auth.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="min-h-screen bg-gray-50">
      <!-- Navbar -->
      <nav class="bg-white shadow">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex justify-between h-16">
            <!-- Logo -->
            <div class="flex items-center">
              <div class="flex-shrink-0 flex items-center">
                <div class="w-10 h-10 bg-primary-500 rounded-full flex items-center justify-center">
                  <span class="text-white font-bold">PS</span>
                </div>
                <span class="ml-2 text-xl font-bold text-gray-800">ProServi</span>
              </div>
            </div>

            <!-- User Menu -->
            <div class="flex items-center">
              <span class="text-gray-700 mr-4" *ngIf="currentUser">
                {{ currentUser.fullName }}
              </span>
              <button
                (click)="logout()"
                class="bg-danger-500 hover:bg-danger-700 text-white font-bold py-2 px-4 rounded"
              >
                Cerrar sesión
              </button>
            </div>
          </div>
        </div>
      </nav>

      <!-- Content -->
      <div class="max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold text-gray-900">Bienvenido, {{ currentUser?.fullName }}!</h1>
        <p class="text-gray-600 mt-2">Tu rol: <span class="font-semibold">{{ currentUser?.role }}</span></p>

        <div class="mt-12 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Card Template -->
          <div class="card">
            <h3 class="text-xl font-bold text-gray-800 mb-2">🔍 Buscar Profesionales</h3>
            <p class="text-gray-600">Encuentra expertos en tu área</p>
          </div>

          <div class="card">
            <h3 class="text-xl font-bold text-gray-800 mb-2">📋 Mis Proyectos</h3>
            <p class="text-gray-600">Gestiona tus proyectos activos</p>
          </div>

          <div class="card">
            <h3 class="text-xl font-bold text-gray-800 mb-2">💰 Pagos</h3>
            <p class="text-gray-600">Historial de transacciones</p>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class DashboardComponent implements OnInit {
  currentUser: UserData | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    // Subscribe al usuario actual
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/role-selector']);
  }
}

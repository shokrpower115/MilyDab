import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotAuthGuard } from './core/guards/not-auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    canActivate: [NotAuthGuard],
    children: [
      {
        path: 'role-selector',
        loadComponent: () => import('./modules/auth/components/role-selector/role-selector.component').then(m => m.RoleSelectorComponent)
      },
      {
        path: 'login',
        loadComponent: () => import('./modules/auth/components/login/login.component').then(m => m.LoginComponent)
      },
      {
        path: 'register-customer',
        loadComponent: () => import('./modules/auth/components/register-customer/register-customer.component').then(m => m.RegisterCustomerComponent)
      },
      {
        path: 'register-professional',
        loadComponent: () => import('./modules/auth/components/register-professional/register-professional.component').then(m => m.RegisterProfessionalComponent)
      }
    ]
  },
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    loadComponent: () => import('./modules/dashboard/dashboard.component').then(m => m.DashboardComponent)
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

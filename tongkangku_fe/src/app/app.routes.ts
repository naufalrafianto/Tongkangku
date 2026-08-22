import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

import { VesselComponent } from './pages/vessel/vessel.component';
import { RentalRequestsComponent } from './pages/rental-requests/rental-requests.component';
import { VesselDetailComponent } from './pages/vessel-detail/vessel-detail.component';
import { VesselCreateComponent } from './pages/vessel-create/vessel-create.component';

import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './shared/components/main-layout/main-layout.component';
import { RentalRequestDetailComponent } from './pages/rental-request-detail/rental-request-detail.component';
import { RentalRequestListComponent } from './pages/rental-request-list/rental-request-list.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full',
  },

  // =========================
  // Public / Auth Routes
  // =========================
  {
    path: 'auth',
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'register',
        component: RegisterComponent,
      },
    ],
  },

  // =========================
  // Authenticated Routes
  // =========================
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'vessels',
        children: [
          {
            path: '',
            component: VesselComponent,
          },
          {
            path: ':id/rental-requests',
            component: RentalRequestsComponent,
          },
          {
            path: ':id/detail',
            component: VesselDetailComponent
          },
          {
            path: 'create',
            component: VesselCreateComponent
          }
        ],
      },
    ],
  },

  
  // =========================
  // Rental Requests
  // =========================
  {
    path: 'rental-requests',
    children: [
      {
        path: '',
        component: RentalRequestListComponent,
      },
      {
        path: ':id',
        component: RentalRequestDetailComponent,
      },
    ],
  },

  {
    path: '**',
    redirectTo: 'vessels',
  },
];

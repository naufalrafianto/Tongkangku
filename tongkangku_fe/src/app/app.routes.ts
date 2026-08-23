import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

import { VesselComponent } from './pages/vessel/vessel.component';
import { VesselDetailComponent } from './pages/vessel-detail/vessel-detail.component';
import { VesselCreateComponent } from './pages/vessel-create/vessel-create.component';

import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './shared/components/main-layout/main-layout.component';
import { RentalRequestDetailComponent } from './pages/rental-request-detail/rental-request-detail.component';
import { RentalRequestListComponent } from './pages/rental-request-list/rental-request-list.component';
import { CreateRentalOfferComponent } from './pages/rental-offers/create-rental-offer/create-rental-offer.component';
import { ownerGuard } from './core/guards/owner.guard';
import { RentalRequestsComponent } from './pages/rental-requests/rental-requests.component';
import { VesselOwnComponent } from './pages/vessel-own/vessel-own.component';

export const routes: Routes = [
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
  // Authenticated Routes (With Navbar)
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
            path: 'own',
            component: VesselOwnComponent,
          },
          {
            path: ':id/rental-request',
            component: RentalRequestsComponent,
          },
          {
            path: ':id/detail',
            component: VesselDetailComponent,
          },
          {
            path: 'create',
            component: VesselCreateComponent,
          },
        ],
      },
      {
        path: 'rental-request',
        children: [
          {
            path: '',
            component: RentalRequestListComponent,
          },
          {
            path: ':id',
            component: RentalRequestDetailComponent,
          },
          {
            path: ':id/offer',
            component: CreateRentalOfferComponent,
            canActivate: [ownerGuard],
          },
        ],
      },
      {
        path: 'rental-contract',
        loadComponent: () =>
          import('./pages/rental-contract/rental-contract-list/rental-contract-list.component').then(
            (m) => m.RentalContractListComponent,
          ),
      },
    ],
  },

  {
    path: '**',
    redirectTo: 'auth/login',
  },
];

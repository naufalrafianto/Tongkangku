import { Routes } from '@angular/router';

import { RentalRequestsComponent } from './rental-requests/rental-requests.component';

export const routes: Routes = [
  {
    path: 'vessels',
    children: [
      {
        path: ':id/rental-request',
        component: RentalRequestsComponent,
      },
    ],
  },
];

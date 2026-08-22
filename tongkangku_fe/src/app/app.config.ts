import {
  ApplicationConfig,
  inject,
  APP_INITIALIZER,
  provideZoneChangeDetection,
} from '@angular/core';

import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { firstValueFrom } from 'rxjs';

import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { AuthService } from './core/services/auth.service';

export function initializeAuth(authService: AuthService) {
  return () => {
    if (authService.isLoggedIn() && !authService.getCurrentUserValue()) {
      return firstValueFrom(authService.getMe()).catch(() => {});
    }

    return Promise.resolve();
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({
      eventCoalescing: true,
    }),

    provideRouter(routes),

    provideHttpClient(withInterceptors([authInterceptor])),

    {
      provide: APP_INITIALIZER,
      useFactory: initializeAuth,
      deps: [AuthService],
      multi: true,
    },
  ],
};

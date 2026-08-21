import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { RegisterPayload } from '../../shared/types/auth/register-payload.type';
import { LoadingPayload } from '../../shared/types/auth/login-payload.type';
import { LoginApiResponse } from '../../shared/types/auth/login-response.type';
import { environment } from '../../../environments/environment';
import { UserRole } from '../../shared/interface/InterfaceVessel';
import { EnumHelper } from '../helper/role.helper';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  register(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, payload);
  }

  login(payload: LoadingPayload): Observable<LoginApiResponse> {
    return this.http
      .post<LoginApiResponse>(`${this.apiUrl}/auth/login`, payload)
      .pipe(
        tap((res) => {
          if (res.success && res.data) {
            localStorage.setItem('access_token', res.data.token);
          }
        }),
      );
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_role');
  }

  // --- Manajemen Role ---

  setRole(role: number | UserRole): void {
    localStorage.setItem('user_role', role.toString());
  }

  getRole(): UserRole {
    const role = localStorage.getItem('user_role');
    return role !== null ? Number(role) : UserRole.Charterer;
  }

  getRoleName(): string {
    return EnumHelper.getRoleName(this.getRole());
  }

  isOwner(): boolean {
    return EnumHelper.isOwner(this.getRole());
  }

  isCharterer(): boolean {
    return EnumHelper.isCharterer(this.getRole());
  }
}

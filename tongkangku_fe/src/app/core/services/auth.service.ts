import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { LoadingPayload } from '../../shared/types/auth/login-payload.type';
import { LoginApiResponse } from '../../shared/types/auth/login-response.type';
import { environment } from '../../../environments/environment';
import { EnumHelper } from '../helper/role.helper';
import {
  CurrentUser,
  CurrentUserResponse,
} from '../../shared/types/auth/current-user.type';
import { UserRole } from '../../shared/types/enum/user.enum';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private currentUserSubject = new BehaviorSubject<CurrentUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  register(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, payload);
  }

  login(payload: LoadingPayload): Observable<CurrentUserResponse> {
    return this.http
      .post<LoginApiResponse>(`${this.apiUrl}/auth/login`, payload)
      .pipe(
        switchMap((res) => {
          if (!res.success || !res.data) {
            throw new Error(res.message || 'Login gagal.');
          }

          localStorage.setItem('access_token', res.data.token);

          return this.getMe();
        }),
      );
  }

  getMe(): Observable<CurrentUserResponse> {
    return this.http.get<CurrentUserResponse>(`${this.apiUrl}/auth/me`).pipe(
      tap((res) => {
        if (res.success && res.data) {
          this.currentUserSubject.next(res.data);
          this.setRole(res.data.role);
        }
      }),
    );
  }

  getCurrentUserValue(): CurrentUser | null {
    return this.currentUserSubject.value;
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

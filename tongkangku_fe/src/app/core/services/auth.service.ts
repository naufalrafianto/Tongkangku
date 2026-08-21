import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';

import { LoadingPayload } from '../../shared/types/auth/login-payload.type';
import { LoginApiResponse } from '../../shared/types/auth/login-response.type';
import {
  CurrentUser,
  CurrentUserResponse,
} from '../../shared/types/auth/current-user.type';
import { UserRole } from '../../shared/types/enum/user.enum';

import { environment } from '../../../environments/environment';
import { EnumHelper } from '../helper/role.helper';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  private currentUserSubject = new BehaviorSubject<CurrentUser | null>(null);

  public currentUser$ = this.currentUserSubject.asObservable();

  private userRole: UserRole | null = null;

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

  setRole(role: UserRole): void {
    this.userRole = role;

    localStorage.setItem('user_role', role.toString());
  }

  getRole(): UserRole | null {
    if (this.userRole !== null) {
      return this.userRole;
    }

    const storedRole = localStorage.getItem('user_role');

    if (storedRole === null) {
      return null;
    }

    return Number(storedRole) as UserRole;
  }

  getRoleName(): string {
    const role = this.getRole();

    if (role === null) {
      return '';
    }

    return EnumHelper.getRoleName(role);
  }

  isOwner(): boolean {
    const role = this.getRole();

    return role !== null && EnumHelper.isOwner(role);
  }

  isCharterer(): boolean {
    const role = this.getRole();

    return role !== null && EnumHelper.isCharterer(role);
  }

  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_role');

    this.userRole = null;
    this.currentUserSubject.next(null);
  }
}

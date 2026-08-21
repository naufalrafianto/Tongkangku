import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { UserRole } from '../../shared/interface/InterfaceVessel';
import { EnumHelper } from '../helper/role.helper';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5168/api/auth'; // 1. Spasi sudah dihapus

  register(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, payload);
  }

  login(payload: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, payload).pipe(
      tap((res) => {
        if (res.token) {
          localStorage.setItem('access_token', res.token);
        }
        // 2. Simpan role otomatis jika dikirim dari backend (misal res.role = 1 atau 2)
        if (res.role !== undefined) {
          this.setRole(res.role);
        }
      })
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
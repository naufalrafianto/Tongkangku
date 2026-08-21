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
  private apiUrl = 'http://localhost:5168/api/auth'; 
  private userRole: string = '';
  private userId: string = '';

  register(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, payload);
  }

  login(payload: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, payload).pipe(
      tap((res) => {
        console.log('Response Asli Backend:', res);

        if (res.token) {
          localStorage.setItem('access_token', res.token);
        }
        if (res.user?.role) {
          this.setRole(res.user.role);
        } else if (res.role) {
          this.setRole(res.role);
        }

        if (res.user?.id) {
          this.setUserId(res.user.id);
        } else if (res.id) {
          this.setUserId(res.id);
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
    localStorage.removeItem('role');
    localStorage.removeItem('id');
    this.userRole = '';
    this.userId = '';
  }


  setUserId(id: string): void {
    this.userId = id;
    localStorage.setItem('id', id);
  }

  getUserId(): string {
    return this.userId || localStorage.getItem('id') || '';
  }

  setRole(role: string): void {
    this.userRole = role;
    localStorage.setItem('role', role);
  }

  getRole(): string {
    return this.userRole || localStorage.getItem('role') || '';
  }
}
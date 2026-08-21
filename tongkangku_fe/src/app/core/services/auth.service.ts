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
      if (res.user && res.user.role !== undefined) {
        this.setRole(res.user.role);
        
      } else {
        console.warn('Field user.role tidak ditemukan!');
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


  setRole(role: string){
    this.userRole = role;
    localStorage.setItem('role', role);
  }

  getRole(): string {
  return this.userRole || localStorage.getItem('role') || '';
  }

}
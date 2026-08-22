import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Port } from '../../shared/types/port/port.types';
import { ApiResponse } from '../../shared/types/api/response.type';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PortService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAll(): Observable<Port[]> {
    return this.http.get<Port[]>(`${this.apiUrl}/ports`);
  }
}

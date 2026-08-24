import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Port } from '../../shared/types/port/port.types';
import { ApiResponse } from '../../shared/types/api/response.type';
import { Observable } from 'rxjs';
import { PortRequestDto } from '../../shared/interface/PortIntervace';

@Injectable({
  providedIn: 'root',
})
export class PortService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAll(): Observable<ApiResponse<Port[]>> {
    return this.http.get<ApiResponse<Port[]>>(`${this.apiUrl}/ports`);

  }
  createPort(dto: PortRequestDto): Observable<ApiResponse<Port>> {
    return this.http.post<ApiResponse<Port>>(`${this.apiUrl}/ports`, dto);
  }
}

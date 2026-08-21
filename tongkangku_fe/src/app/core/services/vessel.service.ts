import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../shared/types/api/response.type';
import { Vessel } from '../../shared/types/vessel/vessel.type';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class VesselService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAll(): Observable<ApiResponse<Vessel[]>> {
    return this.http.get<ApiResponse<Vessel[]>>(`${this.apiUrl}/vessels`);
  }

  getById(vesselId: string): Observable<ApiResponse<Vessel>> {
    return this.http.get<ApiResponse<Vessel>>(
      `${this.apiUrl}/vessels/${vesselId}`,
    );
  }

  CreateVessel(payload: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/vessels`, payload);
  }
}

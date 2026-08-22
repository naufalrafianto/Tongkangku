import { HttpClient, HttpParams } from '@angular/common/http';
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

  getAll(search: string = '', limit: number = 10, page: number = 1): Observable<ApiResponse<Vessel[]>> {
    let params = new HttpParams()
    .set('search', search)
    .set('limit', limit)
    .set('page', page)



    return this.http.get<ApiResponse<Vessel[]>>(`${this.apiUrl}/vessels`, {params});
  }

  getById(vesselId: string): Observable<ApiResponse<Vessel>> {
    return this.http.get<ApiResponse<Vessel>>(
      `${this.apiUrl}/vessels/${vesselId}`,
    );
  }

  CreateVessel(payload: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/vessels`, payload);
  }

GetByid(id: string): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/vessels/${id}`); 
}


 
}

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VesselService {

  private http = inject(HttpClient)
  private apiUrl = 'http://localhost:5168/api/vessel'

 getAll(): Observable<any> { 
    return this.http.get<any>(`${this.apiUrl}/get-all`);
  }

  CreateVessel(payload: any): Observable<any>
  {
    return this.http.post<any>(`${this.apiUrl}/create`,payload);
  }

GetByid(id: string): Observable<any> {
  // Gunakan this.apiUrl + id jika apiUrl sudah ada slash di belakang, 
  // atau hapus slash di apiUrl.
  return this.http.get<any>(`${this.apiUrl}/${id}`); 
}
}

import { inject, Injectable } from '@angular/core';
import { portInterface } from '../../shared/interface/port';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class PortService {
   private http = inject(HttpClient);
   private apiUrl = ' http://localhost:5168/api/port';

   GetAllPort(): Observable<portInterface[]>{
    return this.http.get<portInterface[]>(`${this.apiUrl}/port`);
   }
}

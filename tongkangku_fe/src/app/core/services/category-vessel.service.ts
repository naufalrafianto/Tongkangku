import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { categoryVessel } from '../../shared/interface/category-vessel';
@Injectable({
  providedIn: 'root'
})
export class CategoryVesselService {
private http = inject(HttpClient);
private apiUrl = ' http://localhost:5168/api/vessel-category';

GetAllCategory(): Observable<categoryVessel[]> {
    return this.http.get<categoryVessel[]>(`${this.apiUrl}/get-all`);
  }
}

import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../shared/types/api/response.type';
import { CargoType } from '../../shared/types/cargo-type/cargo-type.type';

@Injectable({
  providedIn: 'root',
})
export class CargoTypeService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAll(): Observable<ApiResponse<CargoType[]>> {
    return this.http.get<ApiResponse<CargoType[]>>(
      `${this.apiUrl}/cargo-types`,
    );
  }
}

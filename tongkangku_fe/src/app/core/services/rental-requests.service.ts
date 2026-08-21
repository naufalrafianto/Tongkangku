import { inject, Injectable } from '@angular/core';
import {
  EstimateRentalPayload,
  EstimateRentalResponse,
} from '../../shared/types/rental-request/estimated.type';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/types/api/response.type';
import {
  CreateRentalRequestPayload,
  RentalResponse,
} from '../../shared/types/rental-request/rental-request.type';

@Injectable({
  providedIn: 'root',
})
export class RentalRequestsService {
  private readonly http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAll(): Observable<ApiResponse<RentalResponse[]>> {
    return this.http.get<ApiResponse<RentalResponse[]>>(
      `${this.apiUrl}/rental-request`,
    );
  }

  estimate(payload: EstimateRentalPayload): Observable<EstimateRentalResponse> {
    return this.http.post<EstimateRentalResponse>(
      `${this.apiUrl}/rental-request/estimate`,
      payload,
    );
  }

  getById(rentalId: string): Observable<ApiResponse<RentalResponse>> {
    return this.http.get<ApiResponse<RentalResponse>>(
      `${this.apiUrl}/rental-request/${rentalId}`,
    );
  }

  create(payload: CreateRentalRequestPayload): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/rental-request`,
      payload,
    );
  }

  cancel(rentalId: string): Observable<ApiResponse<any>> {
    return this.http.patch<ApiResponse<any>>(
      `${this.apiUrl}/rental-request/${rentalId}/cancel`,
      {},
    );
  }
}

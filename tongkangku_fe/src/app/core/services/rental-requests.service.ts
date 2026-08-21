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
  RentalStatusResponse,
} from '../../shared/types/rental-request/rental-request.type';

@Injectable({
  providedIn: 'root',
})
export class RentalRequestsService {
  private readonly http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  estimate(payload: EstimateRentalPayload): Observable<EstimateRentalResponse> {
    return this.http.post<EstimateRentalResponse>(
      `${this.apiUrl}/rental-request/estimate`,
      payload,
    );
  }

  create(
    payload: CreateRentalRequestPayload,
  ): Observable<ApiResponse<RentalStatusResponse>> {
    return this.http.post<ApiResponse<RentalStatusResponse>>(
      `${this.apiUrl}/rental-request`,
      payload,
    );
  }
}

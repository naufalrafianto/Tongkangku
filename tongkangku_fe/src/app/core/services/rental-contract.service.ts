import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../shared/types/api/response.type';
import { RentalContract } from '../../shared/types/rental-contract/rentral-contract.type';

@Injectable({
  providedIn: 'root'
})
export class RentalContractService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getById(id: string): Observable<ApiResponse<RentalContract>> {
    return this.http.get<ApiResponse<RentalContract>>(
      `${this.apiUrl}/rental-contracts/${id}`,
    );
  }

  getByRentalRequestId(
    rentalRequestId: string,
  ): Observable<ApiResponse<RentalContract>> {
    return this.http.get<ApiResponse<RentalContract>>(
      `${this.apiUrl}/rental-contracts/rental-request/${rentalRequestId}`,
    );
  }

  complete(
    id: string,
  ): Observable<ApiResponse<RentalContract>> {
    return this.http.patch<ApiResponse<RentalContract>>(
      `${this.apiUrl}/rental-contracts/${id}/complete`,
      {},
    );
  }

  cancel(
    id: string,
  ): Observable<ApiResponse<RentalContract>> {
    return this.http.patch<ApiResponse<RentalContract>>(
      `${this.apiUrl}/rental-contracts/${id}/cancel`,
      {},
    );
  }

}

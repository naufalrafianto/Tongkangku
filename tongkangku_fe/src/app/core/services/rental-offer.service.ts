import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../shared/types/api/response.type';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RentalOffer } from '../../shared/types/rental-offer/rental-offer.type';

@Injectable({
  providedIn: 'root',
})
export class RentalOfferService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getById(rentalId: string): Observable<ApiResponse<RentalOffer>> {
    return this.http.get<ApiResponse<RentalOffer>>(
      `${this.apiUrl}/rental-offers/${rentalId}`,
    );
  }

  getByRentalRequestId(
    rentalRequestId: string,
  ): Observable<ApiResponse<RentalOffer[]>> {
    return this.http.get<ApiResponse<RentalOffer[]>>(
      `${this.apiUrl}/rental-offers/rental-request/${rentalRequestId}`,
    );
  }

  acceptOffer(offerId: string): Observable<ApiResponse<void>> {
    return this.http.patch<ApiResponse<void>>(
      `${this.apiUrl}/rental-offers/${offerId}/accept`,
      {},
    );
  }

  rejectOffer(offerId: string, reason: string): Observable<ApiResponse<void>> {
    return this.http.patch<ApiResponse<void>>(
      `${this.apiUrl}/rental-offers/${offerId}/reject`,
      { reason },
    );
  }
}

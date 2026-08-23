import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateLaytimeRecordDto, LaytimeRecord } from '../../shared/types/laytime/laytime-record.type';


@Injectable({
  providedIn: 'root',
})
export class LaytimeRecordService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getById(id: string): Observable<LaytimeRecord> {
    return this.http.get<LaytimeRecord>(
      `${this.apiUrl}/laytime-records/${id}`,
    );
  }

  getByContractId(contractId: string): Observable<LaytimeRecord[]> {
    return this.http.get<LaytimeRecord[]>(
      `${this.apiUrl}/laytime-records/contract/${contractId}`,
    );
  }

  create(dto: CreateLaytimeRecordDto): Observable<LaytimeRecord> {
    return this.http.post<LaytimeRecord>(
      `${this.apiUrl}/laytime-records`,
      dto,
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/laytime-records/${id}`);
  }
}

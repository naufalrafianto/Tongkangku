
export enum LaytimeOperationType {
  Loading = 0,
  Discharging = 1,
  Other = 2,
}

export interface LaytimeRecord {
  id: string;
  contractId: string;
  operationType: LaytimeOperationType;
  startTime: string;
  endTime: string;
  laytimeHours: number;
  actualDurationHours: number;
  overtimeHours: number;
  savedHours: number;
  demurrageRate: number;
  demurrageAmount: number;
  despatchRate: number;
  despatchAmount: number;
  netLaytimeAmount: number;
  notes: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateLaytimeRecordDto {
  contractId: string;
  operationType: LaytimeOperationType;
  startTime: string;
  endTime: string;
  laytimeHours: number;
  notes?: string;
}


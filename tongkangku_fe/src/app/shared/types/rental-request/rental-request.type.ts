import { RentalStatus } from '../enum/rental-status.enum';

export interface CargoPayload {
  cargoTypeId: string | null;
  quantity: number | null;
  unit: string;
}

export interface CreateRentalRequestPayload {
  vesselId: string;
  charterType: number | null;
  loadingPortId: string | null;
  dischargingPortId: string | null;
  startDate: string | null;
  planDay: number | null;
  notes: string;
  cargos: CargoPayload[];
}

export interface RentalResponse {
  id: string;
  vesselId: string;
  vesselName: string;
  chartererId: string;
  chartererName: string;
  startDate: string;
  planDay: number;
  totalEstimatedPrice: number;
  status: RentalStatus;
  rejectionReason?: string | null;
  notes?: string | null;
  createdAt: string;
  updateAt: string;
}

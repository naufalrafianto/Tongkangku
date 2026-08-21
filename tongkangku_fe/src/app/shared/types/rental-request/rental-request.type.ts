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

export interface RentalStatusResponse {
  id: string;
  status: number;
  updatedAt: string;
}

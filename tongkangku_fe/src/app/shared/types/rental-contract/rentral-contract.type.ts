export enum RentalContractStatus {
  Draft = 0,
  Active = 1,
  Complete = 2,
  Cancelled = 3
}

export interface ContractCargo {
  id: string;
  cargoTypeId: string;
  cargoName: string;
  quantity: number;
  unit: string;
  freightRatePerTon: number | null;
}

export interface RentalContract {
  id: string;
  contractNum: string;

  rentalRequestId: string;
  ownerId: string;
  ownerName: string;

  startDate: string;
  endDate: string;

  demurrageRate: number;
  despatchRate: number;

  agreedRatePerDay: number;
  agreedHireAmount: number;
  agreedBunkerAmount: number;
  agreedOtherCharges: number;
  agreedTotalPrice: number | null;

  status: number;
  cargos: ContractCargo[];

  totalLaytimeAdjustment: number;
  finalSettlementAmount: number | null;
  completedAt: string | null;

  createdAt: string;
  updatedAt: string;
}


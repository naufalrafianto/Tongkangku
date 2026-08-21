import { ApiResponse } from '../api/response.type';

export type EstimateRental = {
  vesselId: string;
  vesselName: string;
  isVesselAvailable: boolean;
  ratePerDay: number;
  planDay: number;
  durationMultiplier: number;
  baseHirePrice: number;
  adjustedHirePrice: number;
  operationalCost: number;
  contingencyCost: number;
  estimatedCost: number;
  targetMargin: number;
  totalEstimatedPrice: number;
  taxRate: number;
  taxAmount: number;
  grandTotal: number;
};

export type EstimateRentalPayload = {
  vesselId: string;
  startDate: string;
  planDay: number;
};
export type EstimateRentalResponse = ApiResponse<EstimateRental>;

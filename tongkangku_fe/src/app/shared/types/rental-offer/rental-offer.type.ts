export interface RentalOffer {
  id: string;
  rentalRequestId: string;
  ownerId: string;
  ownerName: string;

  ratePerDay: number;
  hireAmount: number;
  bunkerAmount: number;
  otherCharges: number;
  totalPrice: number;

  validUntil: string;

  status: number;

  notes: string;
  rejectionReason: string | null;

  createdAt: string;
  updatedAt: string;
}

export interface CreateRentalOfferPayload {
  rentalRequestId: string;
  ratePerDay: number;
  bunkerAmount: number;
  otherCharges: number;
  validUntil: string;
  notes: string;
}

export enum RentalOfferStatus {
  Pending = 0,
  Accepted = 1,
  Rejected = 2,
  Withdrawn = 3,
  Expired = 4
}
export interface RentalOfferPreview {
  ratePerDay: number;
  planDay: number;
  durationMultiplier: number;
  baseHirePrice: number;
  hireAmount: number;
  operationalCost: number;
  contingencyCost: number;
  taxAmount: number;
  bunkerAmount: number;
  otherCharges: number;
  totalPrice: number;
}
export interface RentalOfferStatusResponse {
  id: string;
  status: RentalOfferStatus;
  updatedAt: string;
}

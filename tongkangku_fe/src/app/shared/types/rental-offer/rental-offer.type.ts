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

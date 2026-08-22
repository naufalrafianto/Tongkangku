export enum VesselStatus {
  Unavailable = 0,
  Available = 1
}

export enum UserRole {
  Charterer = 1,
  Owner = 2
}

export interface VesselResponseDto {
  id: string;
  name: string;
  portId: number,
  ownerId: number,
  categoryId : number,
  dwtCapacity: number;
  capacityFeed: number;
  year: number;
  status: VesselStatus; 
  ratePerDay?: number;
}

export interface vesselcreate{
   name: string;
  portId: number,
  ownerId: number,
  categoryId : number,
  dwtCapacity: number;
  capacityFeed: number;
  year: number;
  status: VesselStatus; 
  ratePerDay?: number;
}
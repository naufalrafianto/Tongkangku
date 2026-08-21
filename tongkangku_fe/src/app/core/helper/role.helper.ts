import { UserRole,VesselStatus } from "../../shared/interface/InterfaceVessel";

export class EnumHelper {

  static getRoleName(role: number | UserRole): string {
    return Number(role) === UserRole.Owner ? 'Owner' : 'Charterer';
  }

  static getVesselStatusLabel(status: number | VesselStatus): string {
    return Number(status) === VesselStatus.Available ? 'Tersedia' : 'Tidak Tersedia';
  }

  static isOwner(role: number | UserRole): boolean {
    return Number(role) === UserRole.Owner;
  }

  static isCharterer(role: number | UserRole): boolean {
    return Number(role) === UserRole.Charterer;
  }
}
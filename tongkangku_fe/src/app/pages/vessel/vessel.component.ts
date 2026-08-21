import { Component, inject, OnInit } from '@angular/core';
import { VesselService } from '../../core/services/vessel.service';
import { Router, RouterLink } from '@angular/router';
import { VesselStatus } from '../../shared/interface/InterfaceVessel';
import { CommonModule } from '@angular/common';
import { VesselResponseDto } from '../../shared/interface/InterfaceVessel';
import { EnumHelper } from '../../core/helper/role.helper';
@Component({
  selector: 'app-vessel',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './vessel.component.html',
  styleUrl: './vessel.component.css'
})
export class VesselComponent implements OnInit{
private vesselService = inject(VesselService)
private router = inject(Router);
vesselData : VesselResponseDto[] | null = null
errorMessage = "";
isLoading: boolean = true;
VesselStatus = VesselStatus;
EnumHelper = EnumHelper;

ngOnInit(): void {
  this.fetchVessel();
}
fetchVessel(): void{
  this.isLoading = true;
 this.vesselService.getAll().subscribe({
  next: (response) => {
     this.vesselData = response;
     this.isLoading = false;
  },
  error: (err) => {
    this.errorMessage = err.error?.message || 'Gagal Mengambil Data';
  }
 })
}
}

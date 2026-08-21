import { Component, inject, OnInit } from '@angular/core';
import { VesselService } from '../../core/services/vessel.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EnumHelper } from '../../core/helper/role.helper';
import { Vessel } from '../../shared/types/vessel/vessel.type';
import { VesselStatus } from '../../shared/types/enum/vessel.enum';
@Component({
  selector: 'app-vessel',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './vessel.component.html',
  styleUrl: './vessel.component.css',
})
export class VesselComponent implements OnInit {
  private vesselService = inject(VesselService);
  vesselData: Vessel[] | null = null;
  errorMessage = '';
  isLoading: boolean = true;
  VesselStatus = VesselStatus;
  EnumHelper = EnumHelper;

  ngOnInit(): void {
    this.fetchVessel();
  }
  fetchVessel(): void {
    this.isLoading = true;

    this.vesselService.getAll().subscribe({
      next: (response) => {
        this.vesselData = response.data ?? [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Gagal Mengambil Data';

        this.vesselData = [];
        this.isLoading = false;
      },
    });
  }
}

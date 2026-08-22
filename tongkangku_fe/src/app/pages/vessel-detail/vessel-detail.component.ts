import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';

import { VesselService } from '../../core/services/vessel.service';
import {
  VesselResponseDto,
  VesselStatus
} from '../../shared/interface/InterfaceVessel';

@Component({
  selector: 'app-vessel-detail',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  templateUrl: './vessel-detail.component.html',
  styleUrl: './vessel-detail.component.css'
})
export class VesselDetailComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private vesselSvc = inject(VesselService);

  vesselStatus = VesselStatus;

  id: string | null = null;
  vesselDetail: VesselResponseDto | null = null;
  errorMessage = '';

  ngOnInit(): void {
    this.getDetail();
  }

  getDetail(): void {

    this.id = this.route.snapshot.paramMap.get('id');

    console.log('ID:', this.id);

    if (!this.id) {
      this.errorMessage = 'ID Kapal tidak ditemukan';
      return;
    }

    this.vesselSvc.GetByid(this.id).subscribe({

      next: (response: any) => {

        console.log('Response dari Backend:', response);

        this.vesselDetail = response.data ?? response;

        console.log('Detail Vessel:', this.vesselDetail);
        console.log('Status:', this.vesselDetail?.status);
      },

      error: (err) => {

        console.error('Error dari Backend:', err);

        this.errorMessage =
          err.error?.message || 'Gagal memuat detail kapal';
      }

    });
  }
}
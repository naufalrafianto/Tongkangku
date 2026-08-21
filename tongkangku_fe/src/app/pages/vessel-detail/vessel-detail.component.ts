import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { VesselService } from '../../core/services/vessel.service';
import { VesselResponseDto } from '../../shared/interface/InterfaceVessel';
import { VesselStatus } from '../../shared/interface/InterfaceVessel';
import { DecimalPipe } from '@angular/common';
@Component({
  selector: 'app-vessel-detail',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  templateUrl: './vessel-detail.component.html',
  styleUrl: './vessel-detail.component.css'
})
export class VesselDetailComponent implements OnInit {
  private rouet = inject(ActivatedRoute)
  private vesselSvc = inject(VesselService)
  vesselStatus = VesselStatus;
  id: string | null = null;
  vesselDetail : VesselResponseDto | null = null;
  errorMessage = "";

  ngOnInit()
  {
    
    this.getDetail();

  }

  getDetail(): void {
    this.id = this.rouet.snapshot.paramMap.get('id');
    console.log(this.id);
    if (!this.id) {
      this.errorMessage = 'ID Kapal tidak ditemukan';
      return;
    }
  this.vesselSvc.GetByid(this.id).subscribe({
    next: (response: any) => {
      console.log('Response dari Backend:', response); 
      this.vesselDetail = response.data ? response.data : response; 
    },
    error: (err) => {
      console.error('Error dari Backend:', err);
      this.errorMessage = err.error?.message || 'Gagal memuat detail kapal';
    }
  });
}

}

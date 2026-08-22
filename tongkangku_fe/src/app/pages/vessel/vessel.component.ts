import { Component, inject, OnInit } from '@angular/core';
import { VesselService } from '../../core/services/vessel.service';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VesselResponseDto, VesselStatus } from '../../shared/interface/InterfaceVessel';

@Component({
  selector: 'app-vessel',
  standalone: true,
  imports: [RouterLink, DecimalPipe, FormsModule],
  templateUrl: './vessel.component.html',
  styleUrl: './vessel.component.css',
})
export class VesselComponent implements OnInit {
  private vesselService = inject(VesselService);

  vesselData: VesselResponseDto[] = [];
  errorMessage = '';
  isLoading: boolean = true;
  
  vesselStatus = VesselStatus; // Menyambungkan enum ke template

  page: number = 1;
  limit: number = 6; 
  search: string = '';
  hasMoreData: boolean = true;

  ngOnInit(): void {
    this.fetchVessel();
  }

  fetchVessel(): void {
    this.isLoading = true;

    this.vesselService.getAll(this.search, this.limit, this.page).subscribe({
      next: (response: any) => {
        this.vesselData = response.data ?? [];
        this.hasMoreData = this.vesselData.length === this.limit;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Gagal Mengambil Data';
        this.vesselData = [];
        this.isLoading = false;
      },
    });
  }

  onSearch(): void {
    this.page = 1; 
    this.fetchVessel();
  }

  nextPage(): void {
    if (this.hasMoreData) {
      this.page++;
      this.fetchVessel();
    }
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.fetchVessel();
    }
  }
}
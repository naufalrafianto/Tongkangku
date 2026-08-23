import { Component, inject } from '@angular/core';
import { VesselService } from '../../core/services/vessel.service';
import { Vessel } from '../../shared/types/vessel/vessel.type';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vessel-own',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './vessel-own.component.html',
  styleUrl: './vessel-own.component.css'
})
export class VesselOwnComponent {
  private readonly vesselService = inject(VesselService);

  vessels: Vessel[] = [];
  isLoading = false;
  errorMessage = '';

  search = '';
  page = 1;
  limit = 10;

  ngOnInit(): void {
    this.loadVessels();
  }

  loadVessels(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.vesselService.getAllByOwner(this.search, this.limit, this.page).subscribe({
      next: (res) => {
        this.vessels = res.data ?? [];
        this.isLoading = false;
      },
      error: (err) => {
        this.vessels = [];
        this.errorMessage =
          err?.error?.message || 'Kapal tidak ditemukan atau terjadi kesalahan.';
        this.isLoading = false;
      },
    });
  }

  onSearchChange(): void {
    this.page = 1;
    this.loadVessels();
  }

  nextPage(): void {
    this.page++;
    this.loadVessels();
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadVessels();
    }
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Disewa';
      case 1:
        return 'Tersedia';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 0:
        return 'bg-red-100 text-red-700';
      case 1:
        return 'bg-blue-100 text-blue-700';
      default:
        return 'bg-slate-100 text-slate-600';
    }
  }
}

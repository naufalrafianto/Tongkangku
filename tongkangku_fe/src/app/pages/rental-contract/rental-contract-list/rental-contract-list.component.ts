import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RentalContract } from '../../../shared/types/rental-contract/rentral-contract.type';
import { RentalContractService } from '../../../core/services/rental-contract.service';

@Component({
  selector: 'app-rental-contract-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './rental-contract-list.component.html',
  styleUrl: './rental-contract-list.component.css'
})
export class RentalContractListComponent implements OnInit {
  private readonly rentalContractService = inject(RentalContractService);

  contracts: RentalContract[] = [];
  isLoading = false;
  errorMessage = '';
  processingId: string | null = null;

  ngOnInit(): void {
    this.loadContracts();
  }

  loadContracts(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.rentalContractService.getAll().subscribe({
      next: (res) => {
        this.contracts = res.data ?? [];
        this.isLoading = false;
      },
      error: (err) => {
        this.contracts = [];
        this.errorMessage =
          err?.error?.message || 'Kontrak tidak ditemukan atau terjadi kesalahan.';
        this.isLoading = false;
      },
    });
  }

  complete(contract: RentalContract): void {
    if (this.processingId) return;
    this.processingId = contract.id;

    this.rentalContractService.complete(contract.id).subscribe({
      next: () => {
        this.processingId = null;
        this.loadContracts();
      },
      error: (err) => {
        this.processingId = null;
        this.errorMessage = err?.error?.message || 'Gagal menyelesaikan kontrak.';
      },
    });
  }

  cancel(contract: RentalContract): void {
    if (this.processingId) return;
    this.processingId = contract.id;

    this.rentalContractService.cancel(contract.id).subscribe({
      next: () => {
        this.processingId = null;
        this.loadContracts();
      },
      error: (err) => {
        this.processingId = null;
        this.errorMessage = err?.error?.message || 'Gagal membatalkan kontrak.';
      },
    });
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Draft';
      case 1:
        return 'Active';
      case 2:
        return 'Complete';
      case 3:
        return 'Cancelled';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 0:
        return 'bg-yellow-100 text-yellow-700';
      case 1:
        return 'bg-blue-100 text-blue-700';
      case 2:
        return 'bg-green-100 text-green-700';
      case 3:
        return 'bg-red-100 text-red-700';
      default:
        return 'bg-slate-100 text-slate-600';
    }
  }

  isActive(status: number): boolean {
    return status === 0;
  }

}

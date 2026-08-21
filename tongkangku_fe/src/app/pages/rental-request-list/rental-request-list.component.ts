import { Component, inject, OnInit, signal } from '@angular/core';
import { RentalRequestsService } from '../../core/services/rental-requests.service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-rental-request-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './rental-request-list.component.html',
  styleUrl: './rental-request-list.component.css',
})
export class RentalRequestListComponent implements OnInit {
  private readonly rentalService = inject(RentalRequestsService);
  private readonly router = inject(Router);

  readonly requests = signal<any[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadRequests();
  }

  private loadRequests(): void {
    this.loading.set(true);
    this.error.set(null);

    this.rentalService
      .getAll()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.requests.set(res.data ?? []);
          } else {
            this.error.set(res.message || 'Failed to load rental requests.');
          }
        },
        error: (error) => {
          this.error.set(
            error?.error?.message || 'Failed to load rental requests.',
          );
        },
      });
  }

  viewDetail(id: string): void {
    this.router.navigate(['/rental-requests', id]);
  }
}

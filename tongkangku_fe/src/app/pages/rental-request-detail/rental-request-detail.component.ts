import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { RentalRequestsService } from '../../core/services/rental-requests.service';
import { AuthService } from '../../core/services/auth.service';
import { RentalResponse } from '../../shared/types/rental-request/rental-request.type';
import { RentalOfferService } from '../../core/services/rental-offer.service';
import { RentalOffer } from '../../shared/types/rental-offer/rental-offer.type';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-rental-request-detail',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './rental-request-detail.component.html',
})
export class RentalRequestDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private rentalService = inject(RentalRequestsService);
  private offerService = inject(RentalOfferService);
  private authService = inject(AuthService);

  private id = this.route.snapshot.paramMap.get('id')!;

  detail = signal<RentalResponse | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  offers = signal<RentalOffer[]>([]);
  offersLoading = signal(false);

  cancelLoading = signal(false);
  actionLoading = signal<string | null>(null); // Menyimpan ID offer yang sedang diproses

  isOwnerOfRequest = computed(() => {
    const user = this.authService.getCurrentUserValue();
    return user?.id === this.detail()?.chartererId;
  });

  canCancel = computed(
    () => this.detail()?.status === 0 && this.isOwnerOfRequest(),
  );

  ngOnInit(): void {
    this.loadDetail();
  }

  private loadDetail(): void {
    this.loading.set(true);
    this.rentalService.getById(this.id).subscribe({
      next: (res) => {
        this.detail.set(res.data);
        this.loading.set(false);

        if (res.data?.status === 1) {
          this.loadOffers();
        }
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? 'Failed to load rental request.');
        this.loading.set(false);
      },
    });
  }

  private loadOffers(): void {
    this.offersLoading.set(true);
    this.offerService
      .getByRentalRequestId(this.id)
      .pipe(finalize(() => this.offersLoading.set(false)))
      .subscribe({
        next: (res) => this.offers.set(res.data ?? []),
      });
  }

  cancel(): void {
    this.cancelLoading.set(true);
    this.rentalService
      .cancel(this.id)
      .pipe(finalize(() => this.cancelLoading.set(false)))
      .subscribe({
        next: () => this.loadDetail(),
      });
  }

  acceptOffer(offerId: string): void {
    if (!confirm('Apakah Anda yakin ingin menerima penawaran ini?')) return;

    this.actionLoading.set(offerId);
    this.offerService
      .acceptOffer(offerId)
      .pipe(finalize(() => this.actionLoading.set(null)))
      .subscribe({
        next: () => {
          this.loadDetail();
        },
        error: (err) => {
          alert(err?.error?.message ?? 'Gagal menerima penawaran.');
        },
      });
  }

  rejectOffer(offerId: string): void {
    const reason = prompt('Masukkan alasan penolakan penawaran:');
    if (reason === null) return; // User membatalkan prompt

    if (!reason.trim()) {
      alert('Alasan penolakan harus diisi.');
      return;
    }

    this.actionLoading.set(offerId);
    this.offerService
      .rejectOffer(offerId, reason.trim())
      .pipe(finalize(() => this.actionLoading.set(null)))
      .subscribe({
        next: () => {
          this.loadOffers();
        },
        error: (err) => {
          alert(err?.error?.message ?? 'Gagal menolak penawaran.');
        },
      });
  }

  refresh(): void {
    this.loadDetail();
  }
}

// rental-request-detail.component.ts
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { RentalRequestsService } from '../../core/services/rental-requests.service';
import { AuthService } from '../../core/services/auth.service';
import { RentalResponse } from '../../shared/types/rental-request/rental-request.type';
import { RentalOfferService } from '../../core/services/rental-offer.service';
import { RentalOffer } from '../../shared/types/rental-offer/rental-offer.type';
import { CurrencyPipe, DatePipe, JsonPipe } from '@angular/common';

@Component({
  selector: 'app-rental-request-detail',
  standalone: true,
  imports: [JsonPipe, CurrencyPipe, DatePipe],
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

  refresh(): void {
    this.loadDetail();
  }
}

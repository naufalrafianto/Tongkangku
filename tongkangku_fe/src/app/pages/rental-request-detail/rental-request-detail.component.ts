import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { RentalRequestsService } from '../../core/services/rental-requests.service';
import { AuthService } from '../../core/services/auth.service';
import { RentalResponse } from '../../shared/types/rental-request/rental-request.type';
import { RentalOfferService } from '../../core/services/rental-offer.service';
import { RentalOffer, RentalOfferStatus } from '../../shared/types/rental-offer/rental-offer.type';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RentalContractService } from '../../core/services/rental-contract.service';
import { RentalStatus } from '../../shared/types/enum/rental-status.enum';

@Component({
  selector: 'app-rental-request-detail',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './rental-request-detail.component.html',
})
export class RentalRequestDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private rentalService = inject(RentalRequestsService);
  private offerService = inject(RentalOfferService);
  private authService = inject(AuthService);
  private contractService = inject(RentalContractService);
  readonly RentalStatus = RentalStatus;
  readonly RentalOfferStatus = RentalOfferStatus;

  private id = this.route.snapshot.paramMap.get('id')!;

  acceptingOfferId = signal<string | null>(null);
  offerActionError = signal<string | null>(null);

  detail = signal<RentalResponse | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  offers = signal<RentalOffer[]>([]);
  offersLoading = signal(false);
  contractLoading = signal(false);

  cancelLoading = signal(false);
  actionLoading = signal<string | null>(null);
  currentUser = computed(() => this.authService.getCurrentUserValue());

  isOwnerOfRequest = computed(() => {
    const user = this.authService.getCurrentUserValue();
    return user?.id === this.detail()?.chartererId;
  });

  isOwner = computed(() => {
    const user = this.currentUser();
    return user?.role === 2;
  });

  isCharterer = computed(() => {
    const user = this.currentUser();
    const rental = this.detail();
    return !!user && !!rental && user.id === rental.chartererId;
  });

  canCancel = computed(
    () => this.detail()?.status === 0 && this.isOwnerOfRequest(),
  );

  ngOnInit(): void {
    this.loadDetail();
  }

  createOffer(): void {
    this.router.navigate(['/rental-requests', this.id, 'offer']);
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

    this.acceptingOfferId.set(offerId);
    this.offerActionError.set(null);

    this.offerService
      .acceptOffer(offerId)
      .pipe(finalize(() => this.actionLoading.set(null)))
      .subscribe({
        next: () => {
          this.contractService.getByRentalRequestId(this.id).subscribe({
            next: (res) => {
              if (res.success && res.data) {
                this.router.navigate(['/rental-contracts', res.data.id]);
              } else {
                this.loadDetail();
              }
            },
            error: () => this.loadDetail(),
          });
        },
        error: (err) => {
          this.offerActionError.set(
            err?.error?.message ?? 'Failed to accept offer.',
          );
        },
      });
  }

  rejectingOfferId = signal<string | null>(null);

  rejectOffer(offerId: string, reason: string): void {
    this.rejectingOfferId.set(offerId);
    this.offerActionError.set(null);

    this.offerService
      .rejectOffer(offerId, reason)
      .pipe(finalize(() => this.rejectingOfferId.set(null)))
      .subscribe({
        next: () => this.loadOffers(),
        error: (err) => {
          this.offerActionError.set(
            err?.error?.message ?? 'Failed to reject offer.',
          );
        },
      });
  }

  rejectOfferWithPrompt(offerId: string): void {
    const reason = window.prompt('Masukkan alasan penolakan penawaran:');
    if (reason !== null && reason.trim() !== '') {
      this.rejectOffer(offerId, reason);
    }
  }

  viewContract(): void {
    this.contractLoading.set(true);
    this.contractService.getByRentalRequestId(this.id).subscribe({
      next: (res) => {
        this.contractLoading.set(false);
        if (res.success && res.data) {
          this.router.navigate(['/rental-contracts', res.data.id]);
        }
      },
      error: () => this.contractLoading.set(false),
    });
  }

  refresh(): void {
    this.loadDetail();
  }

}

import { Component, inject, OnInit, signal } from '@angular/core';
import { NonNullableFormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RentalRequestsService } from '../../../core/services/rental-requests.service';
import { RentalOfferService } from '../../../core/services/rental-offer.service';
import { VesselService } from '../../../core/services/vessel.service';
import { RentalResponse } from '../../../shared/types/rental-request/rental-request.type';
import { Vessel } from '../../../shared/types/vessel/vessel.type';
import {
  finalize,
  startWith,
  switchMap,
  of,
  catchError,
  debounceTime,
  distinctUntilChanged,
} from 'rxjs';
import { CurrencyPipe, DatePipe } from '@angular/common';
import {
  FormDateComponent,
  FormFieldComponent,
  FormInputComponent,
  FormTextareaComponent,
} from '../../../shared/components/form';
import { RentalOfferPreview } from '../../../shared/types/rental-offer/rental-offer.type';

@Component({
  selector: 'app-create-rental-offer',
  standalone: true,
  imports: [
    CurrencyPipe,
    DatePipe,
    ReactiveFormsModule,
    FormFieldComponent,
    FormInputComponent,
    FormDateComponent,
    FormTextareaComponent,
  ],
  templateUrl: './create-rental-offer.component.html',
  styleUrl: './create-rental-offer.component.css',
})
export class CreateRentalOfferComponent implements OnInit {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly rentalRequestsService = inject(RentalRequestsService);
  private readonly offerService = inject(RentalOfferService);
  private readonly vesselService = inject(VesselService);

  readonly rentalRequestId = this.route.snapshot.paramMap.get('id')!;

  readonly rentalRequest = signal<RentalResponse | null>(null);
  readonly vessel = signal<Vessel | null>(null);

  readonly rentalRequestLoading = signal(true);
  readonly rentalRequestError = signal<string | null>(null);

  readonly preview = signal<RentalOfferPreview | null>(null);
  readonly previewLoading = signal(false);
  readonly previewError = signal<string | null>(null);

  ngOnInit(): void {
    this.loadRentalRequest();
  }

  private loadRentalRequest(): void {
    this.rentalRequestLoading.set(true);
    this.rentalRequestError.set(null);

    this.rentalRequestsService
      .getById(this.rentalRequestId)
      .pipe(
        switchMap((res) => {
          if (!res.success || !res.data) {
            this.rentalRequestError.set(res.message || 'Rental request not found');
            return of(null);
          }

          if (res.data.status !== 0 && res.data.status !== 1) {
            this.rentalRequestError.set(
              'Offers can only be submitted for pending or offered rental requests.',
            );
          }

          this.rentalRequest.set(res.data);

          return this.vesselService.getById(res.data.vesselId).pipe(
            catchError(() => of(null)),
          );
        }),
      )
      .subscribe({
        next: (vesselRes) => {
          if (vesselRes?.success && vesselRes.data) {
            this.vessel.set(vesselRes.data);
            this.form.controls.ratePerDay.setValue(vesselRes.data.ratePerDay);
          }
          this.rentalRequestLoading.set(false);
          this.setupPreviewListener();
        },
        error: (err) => {
          this.rentalRequestError.set(
            err?.error?.message || 'Failed to load rental request.',
          );
          this.rentalRequestLoading.set(false);
        },
      });
  }

  readonly form = this.fb.group({
    ratePerDay: this.fb.control<number | null>(
      { value: null, disabled: true },
      [Validators.required, Validators.min(1)],
    ),
    bunkerAmount: this.fb.control<number | null>(0, [
      Validators.required,
      Validators.min(0),
    ]),
    otherCharges: this.fb.control<number | null>(0, [
      Validators.required,
      Validators.min(0),
    ]),
    validUntil: this.fb.control<string | null>(null, Validators.required),
    notes: this.fb.control(''),
  });

  private setupPreviewListener(): void {
    this.form.valueChanges
      .pipe(
        startWith(this.form.getRawValue()),
        debounceTime(300),
        distinctUntilChanged(
          (a, b) =>
            a.bunkerAmount === b.bunkerAmount &&
            a.otherCharges === b.otherCharges,
        ),
        switchMap(() => {
          const raw = this.form.getRawValue();

          if (!raw.ratePerDay || raw.ratePerDay <= 0) {
            return of(null);
          }

          this.previewLoading.set(true);
          this.previewError.set(null);

          return this.offerService
            .preview(
              this.rentalRequestId,
              raw.ratePerDay,
              raw.bunkerAmount ?? 0,
              raw.otherCharges ?? 0,
            )
            .pipe(
              catchError((err) => {
                this.previewError.set(
                  err?.error?.message || 'Gagal menghitung estimasi harga.',
                );
                return of(null);
              }),
              finalize(() => this.previewLoading.set(false)),
            );
        }),
      )
      .subscribe((res) => {
        if (res?.success && res.data) {
          this.preview.set(res.data);
        }
      });
  }

  readonly submitLoading = signal(false);
  readonly submitError = signal<string | null>(null);

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const payload = {
      rentalRequestId: this.rentalRequestId,
      ratePerDay: value.ratePerDay!,
      bunkerAmount: value.bunkerAmount!,
      otherCharges: value.otherCharges!,
      validUntil: value.validUntil!,
      notes: value.notes,
    };

    this.submitLoading.set(true);
    this.submitError.set(null);

    this.offerService
      .create(payload)
      .pipe(finalize(() => this.submitLoading.set(false)))
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.router.navigate(['/rental-request', this.rentalRequestId]);
          } else {
            this.submitError.set(res.message || 'Failed to submit offer.');
          }
        },
        error: (err) => {
          this.submitError.set(err?.error?.message || 'Failed to submit offer.');
        },
      });
  }

  cancel(): void {
    this.router.navigate(['/rental-request', this.rentalRequestId]);
  }
}

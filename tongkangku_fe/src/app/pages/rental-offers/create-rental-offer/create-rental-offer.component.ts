import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { NonNullableFormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RentalRequestsService } from '../../../core/services/rental-requests.service';
import { RentalOfferService } from '../../../core/services/rental-offer.service';
import { RentalResponse } from '../../../shared/types/rental-request/rental-request.type';
import { toSignal } from '@angular/core/rxjs-interop';
import { finalize, startWith } from 'rxjs';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormDateComponent, FormFieldComponent, FormInputComponent, FormTextareaComponent } from '../../../shared/components/form';

@Component({
  selector: 'app-create-rental-offer',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, ReactiveFormsModule, FormFieldComponent, FormInputComponent, FormDateComponent, FormTextareaComponent],
  templateUrl: './create-rental-offer.component.html',
  styleUrl: './create-rental-offer.component.css'
})
export class CreateRentalOfferComponent implements OnInit {
  private readonly fb = inject(NonNullableFormBuilder)
  private readonly route = inject(ActivatedRoute)
  private readonly router = inject(Router)
  private readonly rentalRequestsService = inject(RentalRequestsService)
  private readonly offerService = inject(RentalOfferService)
  readonly rentalRequestId = this.route.snapshot.paramMap.get('id')!;


  readonly rentalRequest = signal<RentalResponse | null>(null)
  readonly rentalRequestLoading = signal(true)
  readonly rentalRequestError = signal<string | null>(null)

  ngOnInit(): void {
    this.loadRentalRequest()
  }

  private loadRentalRequest(): void {
    this.rentalRequestLoading.set(true)
    this.rentalRequestError.set(null)

    this.rentalRequestsService.getById(this.rentalRequestId).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          if (res.data.status !== 0) {
            this.rentalRequestError.set(
              'Offers can only be submitted for pending rental requests.',
            );
          }
          this.rentalRequest.set(res.data)
        } else {
          this.rentalRequestError.set(
            res.message || 'Rental request not found'
          )
        }
        this.rentalRequestLoading.set(false)
      },
      error: (err) => {
        this.rentalRequestError.set(
          err?.error?.message || 'Failed to load rental request.',
        );
        this.rentalRequestLoading.set(false);
      },
    })
  }

  readonly form = this.fb.group({
    ratePerDay: this.fb.control<number | null>(null, [
      Validators.required,
      Validators.min(1),
    ]),
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
  })

  private readonly formValues = toSignal(this.form.valueChanges.pipe(
    startWith(this.form.value)
  ), { initialValue: this.form.value })

  readonly planDay = computed(() => this.rentalRequest()?.planDay ?? 0)

  readonly hireAmount = computed(() => {
    const rate = this.formValues()?.ratePerDay ?? 0
    return rate * this.planDay()
  })

  readonly totalPrice = computed(() => {
    const bunker = this.formValues()?.bunkerAmount ?? 0
    const other = this.formValues()?.otherCharges ?? 0
    return this.hireAmount() + bunker + other
  })


  readonly submitLoading = signal(false);
  readonly submitError = signal<string | null>(null);

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched()
      return
    }

    const value = this.form.getRawValue()
    const payload = {
      rentalRequestId: this.rentalRequestId,
      ratePerDay: value.ratePerDay!,
      bunkerAmount: value.bunkerAmount!,
      otherCharges: value.otherCharges!,
      validUntil: value.validUntil!,
      notes: value.notes,
    }

    this.submitLoading.set(true)
    this.submitError.set(null)

    this.offerService.create(payload)
      .pipe(finalize(() => this.submitLoading.set(false)))
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.router.navigate(['/rental-request', this.rentalRequestId]);
          } else {
            this.submitError.set(
              res.message || 'Failed to submit offer.',
            );
          }
        },
        error: (err) => {
          this.submitError.set(
            err?.error?.message || 'Failed to submit offer.',
          );
        },
      });

  }
  cancel(): void {
    this.router.navigate(['/rental-request', this.rentalRequestId]);
  }
}

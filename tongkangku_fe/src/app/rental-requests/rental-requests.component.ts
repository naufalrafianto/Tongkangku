import { Component, inject, OnInit, signal } from '@angular/core';

import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { ActivatedRoute, Router } from '@angular/router';

import {
  catchError,
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  finalize,
  forkJoin,
  of,
  startWith,
  switchMap,
} from 'rxjs';

import { toSignal } from '@angular/core/rxjs-interop';

import { FormFieldComponent } from '../shared/components/form/form-field/form-field.component';

import {
  FormDateComponent,
  FormInputComponent,
  FormSelectComponent,
  FormTextareaComponent,
  SelectOption,
} from '../shared/components/form';

import { Vessel } from '../shared/types/vessel/vessel.type';
import { RentalRequestsService } from '../core/services/rental-requests.service';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { AuthService } from '../core/services/auth.service';
import { VesselService } from '../core/services/vessel.service';
import { PortService } from '../core/services/port.service';
import { CargoTypeService } from '../core/services/cargo-type.service';

@Component({
  selector: 'app-rental-requests',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormFieldComponent,
    FormInputComponent,
    FormDateComponent,
    FormTextareaComponent,
    FormSelectComponent,
    CurrencyPipe,
    DecimalPipe,
  ],
  templateUrl: './rental-requests.component.html',
  styleUrl: './rental-requests.component.css',
})
export class RentalRequestsComponent implements OnInit {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly rentalService = inject(RentalRequestsService);
  private readonly authService = inject(AuthService);
  private readonly vesselService = inject(VesselService);
  private readonly portService = inject(PortService);
  private readonly cargoTypeService = inject(CargoTypeService);

  readonly vessel = signal<Vessel | null>(null);
  readonly vesselLoading = signal(true);
  readonly vesselError = signal<string | null>(null);
  readonly vesselId = this.route.snapshot.paramMap.get('id');

  // =========================
  // Options untuk dropdown
  // =========================

  readonly charterTypeOptions: SelectOption<number>[] = [
    { value: 1, label: 'Voyage Charter' },
    { value: 2, label: 'Time Charter' },
    { value: 3, label: 'Bareboat Charter' },
  ];

  readonly portOptions = signal<SelectOption<string>[]>([]);
  readonly cargoTypeOptions = signal<SelectOption<string>[]>([]);
  readonly referenceDataLoading = signal(true);
  readonly referenceDataError = signal<string | null>(null);

  // =========================
  // Submit state
  // =========================

  readonly submitLoading = signal(false);
  readonly submitError = signal<string | null>(null);

  ngOnInit(): void {
    this.loadVessel();
    this.loadReferenceData();
  }

  readonly estimateLoading = signal(false);
  readonly estimateError = signal<string | null>(null);

  readonly currentUser = toSignal(this.authService.currentUser$, {
    initialValue: this.authService.getCurrentUserValue(),
  });

  readonly form = this.fb.group({
    charterType: this.fb.control<number | null>(null, Validators.required),

    loadingPortId: this.fb.control<string | null>(null, Validators.required),

    dischargingPortId: this.fb.control<string | null>(
      null,
      Validators.required,
    ),

    startDate: this.fb.control<string | null>(null, Validators.required),

    planDay: this.fb.control<number | null>(null, [
      Validators.required,
      Validators.min(1),
    ]),

    notes: this.fb.control(''),

    cargos: this.fb.array([this.createCargoGroup()]),
  });

  // =========================
  // Reference data (ports & cargo types)
  // =========================

  private loadReferenceData(): void {
    this.referenceDataLoading.set(true);
    this.referenceDataError.set(null);

    forkJoin({
      ports: this.portService.getAll(),
      cargoTypes: this.cargoTypeService.getAll(),
    }).subscribe({
      next: ({ ports, cargoTypes }) => {
        if (ports.success && ports.data) {
          this.portOptions.set(
            ports.data.map((port) => ({
              value: port.id,
              label: port.city ? `${port.name} — ${port.city}` : port.name,
            })),
          );
        }

        if (cargoTypes.success && cargoTypes.data) {
          this.cargoTypeOptions.set(
            cargoTypes.data.map((cargoType) => ({
              value: cargoType.id,
              label: cargoType.name,
            })),
          );
        }

        this.referenceDataLoading.set(false);
      },
      error: (error) => {
        this.referenceDataError.set(
          error?.error?.message ?? 'Failed to load ports or cargo types.',
        );
        this.referenceDataLoading.set(false);
      },
    });
  }

  // =========================
  // Cargo
  // =========================

  private createCargoGroup() {
    return this.fb.group({
      cargoTypeId: this.fb.control<string | null>(null, Validators.required),

      quantity: this.fb.control<number | null>(null, [
        Validators.required,
        Validators.min(1),
      ]),

      unit: this.fb.control('', Validators.required),
    });
  }

  get cargos() {
    return this.form.controls.cargos;
  }

  addCargo(): void {
    this.cargos.push(this.createCargoGroup());
  }

  removeCargo(index: number): void {
    if (this.cargos.length <= 1) {
      return;
    }

    this.cargos.removeAt(index);
  }

  // =========================
  // Estimate
  // =========================

  private readonly planDayChanges =
    this.form.controls.planDay.valueChanges.pipe(
      startWith(this.form.controls.planDay.value),
    );

  private readonly startDateChanges =
    this.form.controls.startDate.valueChanges.pipe(
      startWith(this.form.controls.startDate.value),
    );

  readonly estimate = toSignal(
    combineLatest([this.planDayChanges, this.startDateChanges]).pipe(
      debounceTime(400),

      distinctUntilChanged(
        (
          [previousPlanDay, previousStartDate],
          [currentPlanDay, currentStartDate],
        ) =>
          previousPlanDay === currentPlanDay &&
          previousStartDate === currentStartDate,
      ),

      switchMap(([planDay, startDate]) => {
        if (!planDay || planDay <= 0 || !startDate || !this.vesselId) {
          this.estimateError.set(null);
          this.estimateLoading.set(false);

          return of(null);
        }

        this.estimateLoading.set(true);
        this.estimateError.set(null);

        return this.rentalService
          .estimate({
            vesselId: this.vesselId,
            planDay,
            startDate,
          })
          .pipe(
            catchError((error) => {
              this.estimateError.set(
                error?.error?.message ?? 'Failed to calculate estimate.',
              );

              return of(null);
            }),

            finalize(() => {
              this.estimateLoading.set(false);
            }),
          );
      }),
    ),
    {
      initialValue: null,
    },
  );

  private loadVessel(): void {
    if (!this.vesselId) {
      this.vesselLoading.set(false);
      this.vesselError.set('Vessel ID is required.');
      return;
    }

    this.vesselLoading.set(true);
    this.vesselError.set(null);

    this.vesselService.getById(this.vesselId).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.vessel.set(response.data);
        } else {
          this.vesselError.set(response.message || 'Vessel tidak ditemukan.');
        }

        this.vesselLoading.set(false);
      },

      error: (error) => {
        this.vesselError.set(
          error?.error?.message || 'Gagal mengambil detail vessel.',
        );

        this.vesselLoading.set(false);
      },
    });
  }

  // =========================
  // Submit
  // =========================

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    if (!this.vesselId) {
      this.submitError.set('Vessel ID is required.');
      return;
    }

    const value = this.form.getRawValue();

    const dto = {
      vesselId: this.vesselId,
      charterType: value.charterType,
      loadingPortId: value.loadingPortId,
      dischargingPortId: value.dischargingPortId,
      startDate: value.startDate,
      planDay: value.planDay,
      notes: value.notes,
      cargos: value.cargos,
    };

    this.submitLoading.set(true);
    this.submitError.set(null);

    this.rentalService
      .create(dto)
      .pipe(finalize(() => this.submitLoading.set(false)))
      .subscribe({
        next: (response) => {
          if (response.success && response.data) {
            this.router.navigate(['/rental-requests', response.data.id]);
          } else {
            this.submitError.set(
              response.message || 'Failed to create rental request.',
            );
          }
        },
        error: (error) => {
          this.submitError.set(
            error?.error?.message || 'Failed to create rental request.',
          );
        },
      });
  }

  cancel(): void {
    this.router.navigate(['/vessels', this.vesselId]);
  }
}

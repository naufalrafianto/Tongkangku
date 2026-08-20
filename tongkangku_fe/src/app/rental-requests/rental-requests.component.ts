import { Component, inject } from '@angular/core';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { FormFieldComponent } from '../shared/components/form/form-field/form-field.component';

import { ActivatedRoute } from '@angular/router';
import {
  FormDateComponent,
  FormInputComponent,
  FormSelectComponent,
  FormTextareaComponent,
} from '../shared/components/form';

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
  ],
  templateUrl: './rental-requests.component.html',
  styleUrl: './rental-requests.component.css',
})
export class RentalRequestsComponent {
  private fb = inject(NonNullableFormBuilder);
  private route = inject(ActivatedRoute);

  private createCargoForm() {
    return this.fb.group({
      cargoId: this.fb.control<string | null>(null, Validators.required),

      quantity: this.fb.control<number | null>(null, [
        Validators.required,
        Validators.min(1),
      ]),

      notes: this.fb.control(''),
    });
  }
  readonly vesselId = this.route.snapshot.paramMap.get('id');

  readonly form = this.fb.group({
    chartererId: this.fb.control<string | null>(null, Validators.required),
    charterType: this.fb.control<string | null>(null, Validators.required),

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

    cargos: this.fb.array<ReturnType<typeof this.createCargoForm>>([]),
  });

  get cargos() {
    return this.form.controls.cargos;
  }

  addCargo() {
    this.cargos.push(
      this.fb.group({
        cargoId: this.fb.control<string | null>(null, Validators.required),

        quantity: this.fb.control<number | null>(null, [
          Validators.required,
          Validators.min(1),
        ]),

        notes: this.fb.control(''),
      }),
    );
  }

  removeCargo(index: number) {
    this.cargos.removeAt(index);
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    const dto = {
      vesselId: this.vesselId,
      ...value,
    };

    console.log(dto);
  }
}

import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import '@angular/compiler';

export interface SelectOption<T = string | number> {
  value: T;
  label: string;
}
@Component({
  selector: 'app-form-select',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form-select.component.html',
})
export class FormSelectComponent {
  control = input.required<FormControl>();

  options = input<SelectOption[]>([]);

  placeholder = input('Select...');
}

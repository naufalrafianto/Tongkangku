import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

export interface SelectOption {
  value: string | number;
  label: string;
}
@Component({
  selector: 'app-form-select',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form-select.component.html',
  styleUrl: './form-select.component.css',
})
export class FormSelectComponent {
  control = input.required<FormControl>();

  options = input<SelectOption[]>([]);

  placeholder = input('Select...');
}

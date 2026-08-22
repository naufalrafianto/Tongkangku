import { Component, input } from '@angular/core';

@Component({
  selector: 'app-form-field',
  standalone: true,
  imports: [],
  templateUrl: './form-field.component.html',
})
export class FormFieldComponent {
  label = input.required<string>();
  required = input(false);
  error = input<string | null>(null);
}

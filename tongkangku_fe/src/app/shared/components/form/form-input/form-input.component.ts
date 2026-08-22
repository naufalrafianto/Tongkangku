import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-input',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form-input.component.html',
})
export class FormInputComponent {
  control = input.required<FormControl>();

  type = input<'text' | 'number' | 'email'>('text');

  placeholder = input('');
}

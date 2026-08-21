import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-textarea',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form-textarea.component.html',
})
export class FormTextareaComponent {
  control = input.required<FormControl>();

  rows = input(4);

  placeholder = input('');
}

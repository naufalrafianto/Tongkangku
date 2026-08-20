import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-date',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form-date.component.html',
  styleUrl: './form-date.component.css',
})
export class FormDateComponent {
  control = input.required<FormControl>();
}

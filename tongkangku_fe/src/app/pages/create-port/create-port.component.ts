import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { PortService } from '../../core/services/port.service';
import { PortRequestDto } from '../../shared/interface/PortIntervace';

@Component({
  selector: 'app-create-port',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './create-port.component.html',
  styleUrl: './create-port.component.css'
})
export class CreatePortComponent {
  private fb = inject(FormBuilder);
  private portService = inject(PortService);
  private router = inject(Router);

  isSubmitting = false;
  errorMessage: string | null = null;

  portForm: FormGroup = this.fb.group({
    name: ['', [Validators.minLength(3)]],
    city: [''],
    province: ['', [Validators.required, Validators.minLength(3)]],
  });

  onSubmit(): void {
    if (this.portForm.invalid) {
      this.portForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;

    const payload: PortRequestDto = this.portForm.value;

    this.portService.createPort(payload).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        alert("pelabuhan berhasil ditambahkan!");
        this.router.navigate(['/vessels/own']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Gagal menambahkan pelabuhan.';
      }
    });
  }
}
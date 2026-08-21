import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { UserRole } from '../../shared/interface/InterfaceVessel';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  fb = inject(FormBuilder);

  registerData = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    role: [null as number | null, [Validators.required]],
  });

  errorMessage = '';

  onRegister() {
    if (this.registerData.invalid) {
      this.errorMessage = 'Harap isi semua kolom dengan benar.';
      return;
    }

    const payload = {
      ...this.registerData.value,
      role: Number(this.registerData.value.role),
    };
    this.authService.register(payload).subscribe({
      next: () => {
        alert('Registrasi berhasil! Silakan login.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Registrasi gagal!';
      },
    });
  }
}

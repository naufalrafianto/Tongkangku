import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  loginData = { email: '', password: '' };
  errorMessage = '';

  onLogin(): void {
    this.errorMessage = '';

    this.authService.login(this.loginData).subscribe({
      next: (res) => {
        if (res.success) {
          this.router.navigate(['/vessels']);
        } else {
          this.errorMessage = res.message || 'Login gagal.';
        }
      },
      error: (err) => {
        console.error('Login error:', err);

        this.errorMessage = err?.error?.message || 'Email atau password salah.';
      },
    });
  }
}

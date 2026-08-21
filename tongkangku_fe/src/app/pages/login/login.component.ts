import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  loginData = { email: '', password: '' };
  errorMessage = '';
  
 onLogin() {
  this.authService.login(this.loginData).subscribe({
    next: () => {
      const userRole = this.authService.getRole();
      console.log("role saat ini:", userRole);

      if (userRole === 'Owner') {
        this.router.navigate(['/register']); 
      } else if (userRole === 'Charterer') { 
        this.router.navigate(['/vessel']);
      } 
    },
    error: (err) => {
      this.errorMessage = err.error?.message || 'Email atau password salah!';
    }
  });
}
}
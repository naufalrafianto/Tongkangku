import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { CurrentUser } from '../../types/auth/current-user.type';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
})
export class NavbarComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  currentUser: CurrentUser | null = null;

  isProfileOpen = false;
  isMobileMenuOpen = false;

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  private loadCurrentUser(): void {
    const token = localStorage.getItem('access_token');

    if (!token) {
      return;
    }

    this.authService.getMe().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.currentUser = res.data;
        }
      },
      error: () => {
        this.logout();
      },
    });
  }

  toggleProfile(): void {
    this.isProfileOpen = !this.isProfileOpen;
  }

  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMenus(): void {
    this.isProfileOpen = false;
    this.isMobileMenuOpen = false;
  }
  getRoleName(role: number | undefined): string {
    switch (role) {
      case 0:
        return 'Admin';

      case 1:
        return 'Owner';

      case 2:
        return 'Charterer';

      default:
        return 'User';
    }
  }

  logout(): void {
    localStorage.removeItem('access_token');

    this.closeMenus();

    this.router.navigate(['/login']);
  }
}

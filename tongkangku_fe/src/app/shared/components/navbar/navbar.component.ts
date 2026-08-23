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
  userRole: any = 0;
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
          const user = this.authService.getRole();
          console.log("data userorle", user);
          this.userRole = user;
          this.currentUser = res.data;
        }
      },
      error: () => {
        this.logout();
      },
    });
  }

  // Helper untuk mengecek apakah user login adalah Owner (Role 2)
  isOwner(): boolean {
    return this.currentUser?.role === 2;
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
        return 'Charterer';
      case 2:
        return 'Owner';
      default:
        return 'User';
    }
  }

  logout(): void {
    localStorage.removeItem('access_token');
    this.closeMenus();

    this.router.navigate(['/auth/login']);
  }
}

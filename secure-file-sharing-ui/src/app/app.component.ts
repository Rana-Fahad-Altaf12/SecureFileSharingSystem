import { Component, inject } from '@angular/core';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, CommonModule],
  template: `
    <div class="container mt-4">
      <nav *ngIf="!isLoginPage && authService.isAuthenticated()" class="navbar">
        <!-- <a *ngIf="!authService.isAuthenticated()" routerLink="/register" class="nav-link">Register</a>
        <a *ngIf="!authService.isAuthenticated()" routerLink="/login" class="nav-link">Login</a>
        <a *ngIf="authService.isAuthenticated()" routerLink="/upload" class="nav-link">Upload</a>
        <a *ngIf="authService.isAuthenticated()" routerLink="/download" class="nav-link">List</a> -->

        <button *ngIf="authService.isAuthenticated()" class="logout-btn" (click)="logout()" aria-label="Logout">
          <!-- Simple logout icon SVG -->
          <svg width="20" height="20" fill="currentColor" viewBox="0 0 16 16" >
            <path d="M6 2a1 1 0 0 1 1-1h4.5A1.5 1.5 0 0 1 13 2.5v11A1.5 1.5 0 0 1 11.5 15H7a1 1 0 0 1-1-1v-2H5v2a2 2 0 0 0 2 2h4.5a2.5 2.5 0 0 0 2.5-2.5v-11A2.5 2.5 0 0 0 11.5 1H7a2 2 0 0 0-2 2v2h1V2z"/>
            <path d="M.146 8.354a.5.5 0 0 1 0-.708L3.793 4H1.5a.5.5 0 0 1 0-1h3.5a.5.5 0 0 1 .5.5v3.5a.5.5 0 0 1-1 0V5.707L.854 8.354a.5.5 0 0 1-.708 0z"/>
          </svg>
        </button>
      </nav>

      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .navbar {
      display: flex;
      justify-content: flex-end;
      gap: 20px;
      padding: 10px 0;
      border-bottom: 1px solid #ddd;
      align-items: center;
      font-family: Arial, sans-serif;
    }
    .nav-link {
      text-decoration: none;
      color: #333;
      font-weight: 500;
      padding: 5px 10px;
      border-radius: 4px;
      transition: background-color 0.2s;
    }
    .nav-link:hover {
      background-color: #f0f0f0;
    }
    .logout-btn {
      background: none;
      border: none;
      cursor: pointer;
      color: #c0392b;
      padding: 5px;
      border-radius: 4px;
      transition: background-color 0.2s;
      display: flex;
      align-items: center;
    }
    .logout-btn:hover {
      background-color: #f9e6e6;
    }
    .logout-btn svg {
      display: block;
    }
  `]
})
export class AppComponent {
  authService = inject(AuthService);
  router = inject(Router);

  isLoginPage = false;

  constructor() {
    // Listen to router events and update isLoginPage based on current URL
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.isLoginPage = ['/login', '/register', '/third-party'].some(path => event.url.includes(path));
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

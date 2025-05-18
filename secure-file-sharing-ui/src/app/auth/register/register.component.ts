import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  email = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

loading = false;
register() {
  this.loading = true;
  this.authService.register(this.email, this.password).subscribe({
    next: () => {
        this.loading = false;
        this.router.navigate(['/login']);
      },
    error: err => {
        this.loading = false;
        this.error = (err.error?.error || err.error || 'Registration failed');
      }
  });
}

}

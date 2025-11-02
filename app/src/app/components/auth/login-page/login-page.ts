import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth-service';
import { LoginData } from '../../../interfaces';

@Component({
  selector: 'app-login-page',
  imports: [MatInputModule, MatButtonModule, ReactiveFormsModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {
  private authService = inject(AuthService);
  private router = inject(Router);
  hasError: Error | null = null;

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl(''),
  });

  onSubmit() {
    if (this.loginForm.valid) {
      const formValue = this.loginForm.value as LoginData;
      this.authService.login(formValue).subscribe({
        next: (data) => {
          const token = data.accessToken;
          localStorage.setItem('accessToken', token);
          this.authService.currentToken.set(token);
          this.router.navigate(['/home']);
        },
        error: (error) => {
          this.hasError = error;
          console.error(error);
        },
      });
    }
  }

  get emailFromField() {
    return this.loginForm.get('email');
  }
}

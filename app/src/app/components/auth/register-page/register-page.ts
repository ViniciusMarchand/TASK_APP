import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../../services/auth/auth-service';
import { UserForm } from '../../../interfaces';
import { Router } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-register-page',
  imports: [MatInputModule, MatButtonModule, ReactiveFormsModule, MatProgressSpinnerModule],
  templateUrl: './register-page.html',
  styleUrls: ['./register-page.css'],
})
export class RegisterPage {

  isLoading = false;
  private authService = inject(AuthService)
  private router = inject(Router)

  strongPasswordRegx: RegExp =
  /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{6,}$/;

  registerForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.pattern(this.strongPasswordRegx)]),
    confirmPassword: new FormControl(''),
    confirmEmail: new FormControl('', [Validators.required, Validators.email]),
    userName: new FormControl(''),
  }, {validators: [this.emailMatchValidator, this.passwordMatchValidator]});


  onSubmit() {
    this.isLoading = true;
    if (this.registerForm.valid) {
      const formValue = this.registerForm.value as UserForm;
      this.authService.register(formValue).subscribe({
        next:(_)=>{
          this.router.navigate(['/login']);
          this.isLoading = false;
        },
        error:(error) => {
          console.error(error);
          this.isLoading = false;
        }
      });
    }
  }

  get emailFromField() {
    return this.registerForm.get('email');
  }

  get confirmEmailFromField() {
    return this.registerForm.get('confirmEmail');
  }

  get passwordFromField() {
    return this.registerForm.get('password');
  }


  //validations

  validatePassword(): string {

    const password = this.registerForm.get('password')?.value || '';

    if (!password) {
      return 'Password is required';
    }

    if (password.length < 6) {
      return 'Password must be at least 6 characters long';
    }

    if (!/[A-Z]/.test(password)) {
      return 'Password must contain at least one uppercase letter';
    }

    if (!/[^a-zA-Z0-9]/.test(password)) {
      return 'Password must contain at least one special character';
    }

    return '';
  }


  emailMatchValidator(form: AbstractControl): ValidationErrors | null {
    const group = form as FormGroup;
    const email = group.get('email')?.value;
    const confirmEmail = group.get('confirmEmail')?.value;

    if(!email || !confirmEmail) {
      return null;
    }

    return email === confirmEmail ? null : { emailMismatch: true };
  }

  passwordMatchValidator(form: AbstractControl): ValidationErrors | null {
    const group = form as FormGroup;
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;

    if(!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword ? null : { passwordMismatch: true };
  }

}

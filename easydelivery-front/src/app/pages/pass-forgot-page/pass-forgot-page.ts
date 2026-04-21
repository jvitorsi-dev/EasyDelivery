import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { SpinnerComponent } from '../../componentes/spinner-component/spinner-component';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterModule,
    SpinnerComponent
  ],
  templateUrl: './pass-forgot-page.html',
  styleUrls: ['./pass-forgot-page.css'],
})
export class ForgotPasswordComponent {
  forgotForm: FormGroup;
  enviado = false;
  loading = false;

  constructor(private fb: FormBuilder) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit(): void {
    if (this.forgotForm.invalid) {
      this.forgotForm.markAllAsTouched();
      return;
    }

    const email = this.forgotForm.value.email;
    console.log('Recuperar senha para:', email);
    // Substitua pelo seu serviço: this.authService.forgotPassword(email).subscribe(...)

    this.enviado = true;
  }

  getError(field: string): string {
    const control = this.forgotForm.get(field);
    if (!control?.touched) return '';
    if (control.hasError('required')) return 'Campo obrigatório';
    if (control.hasError('email')) return 'E-mail inválido';
    return '';
  }
}
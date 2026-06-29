import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth-service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserRole } from '../../models/enums/userRole';
import { RegisterRequest } from '../../models/usuario/registerRequest';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    MatButtonModule,
    RouterModule,
  ],
  templateUrl: './register-page.html',
  styleUrls: ['./register-page.css'],
})
export class RegisterPage {
  registerForm: FormGroup;
  hidePassword = true;
  private _snackBar = inject(MatSnackBar);

  roles: { label: string; value: UserRole }[] = [
    { label: 'Cliente', value: 1 },
    { label: 'Entregador', value: 2 },
    { label: 'Restaurante', value: 3 },
  ];

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      endereco: ['', [Validators.required]],
      role: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const payload: RegisterRequest = {
      nome: this.registerForm.value.nome,
      email: this.registerForm.value.email,
      senha: this.registerForm.value.senha,
      endereco: this.registerForm.value.endereco || undefined,
      role: this.registerForm.value.role,
    };

    console.log(payload);
    this.authService.register(payload).subscribe({
      next: () =>{
        this.router.navigateByUrl('');
        this.openSnackBar('Registro concluído! Faça o login.', 'Ok');
      },
      error: (err) =>{
        this.openSnackBar(err.error, 'Ok');
      }
    })

  }

  getError(field: string): string {
    const control = this.registerForm.get(field);
    if (!control?.touched) return '';    
    if (control.hasError('required')) return 'Campo obrigatório';
    if (control.hasError('email')) return 'E-mail inválido';
    if (control.hasError('minlength')) {
      const min = control.errors?.['minlength'].requiredLength;
      return `Mínimo de ${min} caracteres`;
    }
    return '';
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action);
  }
}
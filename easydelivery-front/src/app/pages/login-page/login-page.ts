import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule, MatAnchor } from '@angular/material/button';
import { MatCardModule, MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../services/auth-service';
import {MatSnackBar, MatSnackBarModule} from '@angular/material/snack-bar';
import { SpinnerComponent } from '../../componentes/spinner-component/spinner-component';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  imports: [MatCard,
    MatCardHeader,
    MatCardContent,
    MatInputModule,
    MatCardModule,
    MatSnackBarModule,
    FormsModule,
    ReactiveFormsModule, 
    MatCardActions, 
    MatAnchor,
    SpinnerComponent],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
  providers:[MatCardModule, MatButtonModule, MatIconModule]
})

export class LoginPage {
  formLogin!: FormGroup;  
  loading = false;
  private _snackBar = inject(MatSnackBar);

  constructor(private formBuilder: FormBuilder,
    private authService: AuthService,
    private cd: ChangeDetectorRef,
    private router: Router
  ){
    this.formLogin = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  onSubmit(){
    if (this.formLogin.invalid) {
      this.formLogin.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.cd.detectChanges();
    
    var email = this.formLogin.get('email')?.value;
    var senha = this.formLogin.get('senha')?.value;

    if(email != '' && senha != ''){
      try{
        this.authService.login(email, senha).subscribe({
          next: (response) => {            
            this.authService.setToken(response.token);
            this.authService.setUsuario(response.usuario);
            this.router.navigateByUrl('cliente/home');
          },
          error: (err) => {
            this.openSnackBar(err.error, 'Ok');    
            this.loading = false;
            this.cd.detectChanges();
          }
        });
      }
      catch{
        this.loading = false;
        this.openSnackBar('Houve um erro ao fazer login.', 'Ok')
        
        this.cd.detectChanges();
      }
    }    
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action);
  }
}


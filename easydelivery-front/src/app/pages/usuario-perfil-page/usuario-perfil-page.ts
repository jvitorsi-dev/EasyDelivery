import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BottomNavComponent } from '../../componentes/bottom-nav-component/bottom-nav-component';
import { AuthService } from '../../services/auth-service';
import { UsuarioResponse } from '../../models/usuario/usuarioReponse';

@Component({
  selector: 'app-usuario-perfil-page',
  standalone: true,
  imports: [CommonModule, BottomNavComponent],
  templateUrl: './usuario-perfil-page.html',
  styleUrl: './usuario-perfil-page.css',
})
export class UsuarioPerfilPage {
  usuario: UsuarioResponse | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.usuario = this.authService.getUsuario();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('');
  }

  getRoleLabel(): string {
    if (!this.usuario) return '-';

    const roleId = this.usuario.roleId ?? this.usuario.role;
    switch (roleId) {
      case 1:
        return 'Cliente';
      case 2:
        return 'Restaurante';
      case 3:
        return 'Entregador';
      default:
        return 'Usuário';
    }
  }

  getIniciais(nome?: string): string {
    if (!nome) return 'ED';
    return nome
      .split(' ')
      .map((p) => p[0])
      .slice(0, 2)
      .join('')
      .toUpperCase();
  }
}

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BottomNavComponent } from '../../../componentes/bottom-nav-component/bottom-nav-component';
import { AuthService } from '../../../services/auth-service';
import { RestauranteService } from '../../../services/restaurante-service';
import { RestauranteResponse } from '../../../models/restaurante/restauranteResponse';

@Component({
  selector: 'app-restaurante-cardapio-page',
  standalone: true,
  imports: [CommonModule, BottomNavComponent],
  templateUrl: './restaurante-cardapio-page.html',
  styleUrl: './restaurante-cardapio-page.css',
})
export class RestauranteCardapioPage {
  restaurante: RestauranteResponse | null = null;

  constructor(
    private authService: AuthService,
    private restauranteService: RestauranteService
  ) {
    const usuario = this.authService.getUsuario();
    if (usuario?.id) {
      this.restauranteService.getRestauranteById(usuario.id).subscribe({
        next: (response) => {
          this.restaurante = response.data;
        },
      });
    }
  }

  getIniciais(nome: string): string {
    return nome
      .split(' ')
      .map((p) => p[0])
      .slice(0, 2)
      .join('')
      .toUpperCase();
  }
}

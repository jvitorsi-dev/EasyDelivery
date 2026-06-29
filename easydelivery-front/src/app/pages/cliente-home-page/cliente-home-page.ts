import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { BottomNavComponent } from '../../componentes/bottom-nav-component/bottom-nav-component';
import { RestauranteResponse } from '../../models/restaurante/restauranteResponse';
import { UsuarioResponse } from '../../models/usuario/usuarioReponse';
import { AuthService } from '../../services/auth-service';
import { RestauranteService } from '../../services/restaurante-service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoriaRestauranteResponse } from '../../models/restaurante/categoriaRestauranteResponse';

@Component({
  selector: 'app-cliente-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatIconModule, RouterModule, BottomNavComponent],
  templateUrl: './cliente-home-page.html',
  styleUrls: ['./cliente-home-page.css'],
})
export class ClienteHomePage implements OnInit {
  searchControl = new FormControl('');
  restaurantes: RestauranteResponse[] = [];
  restaurantesFiltrados: RestauranteResponse[] = [];
  categoriasResponse: CategoriaRestauranteResponse[] = [];
  categoriaSelecionada = 'Todos';
  usuario!: UsuarioResponse;
  private _snackBar = inject(MatSnackBar);

  constructor(
    private authService: AuthService,
    private restauranteService: RestauranteService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getUsuario();
    this.carregarRestaurantes();
    this.searchControl.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe((termo) => this.filtrar(termo ?? ''));
  }

  carregarRestaurantes(): void {
    this.restauranteService.getAllRestaurantes().subscribe({
      next: (response) => {
        this.restaurantes = response.data;
        this.restaurantesFiltrados = [...this.restaurantes];

        const categoriasUnicas = new Map<number, CategoriaRestauranteResponse>();
        this.restaurantes.forEach((r) => categoriasUnicas.set(r.categoria.id, r.categoria));
        this.categoriasResponse = [{ id: 0, nome: 'Todos' }, ...Array.from(categoriasUnicas.values())];

        this.cdr.detectChanges();
      },
      error: () => {
        this._snackBar.open('Houve um erro ao carregar os restaurantes.', 'Ok');
      },
    });
  }

  filtrar(termo: string): void {
    const t = termo.toLowerCase();
    this.restaurantesFiltrados = this.restaurantes.filter(
      (r) => r.nome.toLowerCase().includes(t) || r.endereco.toLowerCase().includes(t)
    );
  }

  getUsuario() {
    const usuario = this.authService.getUsuario();
    if (usuario) {
      this.usuario = usuario;
    }
  }

  selecionarCategoria(categoria: number): void {
    this.categoriaSelecionada = this.categoriasResponse.find((c) => c.id === categoria)?.nome ?? 'Todos';
    if (this.categoriaSelecionada !== 'Todos') {
      this.restaurantesFiltrados = this.restaurantes.filter((r) => r.categoria.id === categoria);
    } else {
      this.restaurantesFiltrados = this.restaurantes;
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

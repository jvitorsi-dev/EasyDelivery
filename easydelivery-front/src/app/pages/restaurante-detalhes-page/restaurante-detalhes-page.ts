import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Location } from '@angular/common';
import { RestauranteService } from '../../services/restaurante-service';
import { BottomNavComponent } from '../../componentes/bottom-nav-component/bottom-nav-component';
import { RestauranteResponse } from '../../models/restaurante/restauranteResponse';
import { ItemRestauranteResponse } from '../../models/restaurante/itemsRestauranteResponse';
import { CarrinhoService } from '../../services/carrinho-service';

@Component({
  selector: 'app-restaurante-detalhe',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, BottomNavComponent],
  templateUrl: './restaurante-detalhes-page.html',
  styleUrls: ['./restaurante-detalhes-page.css'],
})
export class RestauranteDetalhePage implements OnInit {
  restaurante!: RestauranteResponse;
  produtos: ItemRestauranteResponse[] = [];
  produtosFiltrados: ItemRestauranteResponse[] = [];
  categorias: string[] = ['Todos'];
  categoriaSelecionada = 'Todos';
  carrinho: any[] = [];
  private _snackBar = inject(MatSnackBar);

  constructor(
    private route: ActivatedRoute,
    private restauranteService: RestauranteService,
    private location: Location,
    private cdr: ChangeDetectorRef,
    private carrinhoService: CarrinhoService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.carrinho = this.carrinhoService.getItens();
    this.carregarRestaurante(Number.parseInt(id!));
  }

  carregarRestaurante(id: number): void {
    this.restauranteService.getRestauranteById(id).subscribe({
      next: (res) => {
        this.restaurante = res.data
        this.produtos = this.restaurante.itens
        this.produtosFiltrados = [...this.produtos];
        this.categorias = ['Todos', ...this.restaurante.itens.map(i => i.categoria.nome)];
        this.cdr.detectChanges();
      },
      error: () => this._snackBar.open('Erro ao carregar restaurante.', 'Ok'),
    });
  }


  selecionarCategoria(cat: string): void {
    this.categoriaSelecionada = cat;
    this.produtosFiltrados = cat === 'Todos'
      ? [...this.produtos]
      : this.produtos.filter(p => p.categoria.nome === cat);
  }

  adicionarAoCarrinho(produto: any): void {
    const existente = this.carrinho.find(i => i.idItem === produto.id);
    if (existente) {
      existente.quantidade++;
    } else {
      this.carrinho.push({ ...produto, quantidade: 1 });
    }    
    this.carrinhoService.setItens(this.carrinho);
  }

  get totalCarrinho(): number {
    return this.carrinho.reduce((acc, i) => acc + i.preco * i.quantidade, 0);
  }

  get totalItens(): number {
    return this.carrinho.reduce((acc, i) => acc + i.quantidade, 0);
  }

  getIniciais(nome: string): string {
    return nome?.split(' ').map(p => p[0]).slice(0, 2).join('').toUpperCase();
  }

  voltar(): void {
    this.location.back();
  }
}
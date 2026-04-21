import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { BottomNavComponent } from '../../componentes/bottom-nav-component/bottom-nav-component';
import { CarrinhoService } from '../../services/carrinho-service';
import { PedidoService } from '../../services/pedido-service';
import { PedidoRequest } from '../../models/pedido/pedidoRequest';
import { AuthService } from '../../services/auth-service';
import { ItemRestauranteResponse } from '../../models/restaurante/itemsRestauranteResponse';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { PagamentoComponente } from '../../componentes/pagamento-componente/pagamento-componente';
import { ItensPedidoRequest } from '../../models/pedido/itensPedidoRequest';
declare global {
  interface Window {
    MercadoPago: any;
  }
}

@Component({
  selector: 'app-carrinho',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatIconModule, BottomNavComponent],
  templateUrl: './carrinho-page.html',
  styleUrls: ['./carrinho-page.css'],
})
export class CarrinhoPage implements OnInit {
  cupomControl = new FormControl('');
  itens: ItemRestauranteResponse[] = [];
  desconto = 0;
  cupomMsg = '';
  cupomValido = false;
  formasPagamento = [ { id: 1, forma: 'credito' }, { id: 2, forma: 'debito' }, { id: 3, forma: 'pix' }, { id: 4, forma: 'dinheiro' }];
  formaPagamento = 1;
  private _snackBar = inject(MatSnackBar);
  readonly dialog = inject(MatDialog);

  readonly CUPONS: Record<string, number> = {
    'EASY10': 10,
    'BURGER15': 15,
  };

  constructor(
    private carrinhoService: CarrinhoService,
    private location: Location,
    private router: Router,
    private authService: AuthService,
    private pedidoService: PedidoService
  ) {}

  ngOnInit(): void {
    this.itens = this.carrinhoService.getItens();
  }

  incrementar(item: any): void {
    item.quantidade++;
  }

  decrementar(item: any): void {
    item.quantidade--;
    if (item.quantidade <= 0) this.remover(item);
  }

  remover(item: any): void {
    this.itens = this.itens.filter(i => i !== item);
    this.carrinhoService.setItens(this.itens);
  }

  aplicarCupom(): void {
    const codigo = this.cupomControl.value?.trim().toUpperCase() ?? '';
    if (this.CUPONS[codigo]) {
      this.desconto = this.CUPONS[codigo];
      this.cupomValido = true;
      this.cupomMsg = `Cupom aplicado! ${this.desconto}% de desconto.`;
    } else {
      this.desconto = 0;
      this.cupomValido = false;
      this.cupomMsg = 'Cupom inválido.';
    }
  }

  get subtotal(): number {
    return this.itens.reduce((acc, i) => acc + i.preco * i.quantidade, 0);
  }

  get valorDesconto(): number {
    return this.subtotal * (this.desconto / 100);
  }

  get total(): number {
    return this.subtotal - this.valorDesconto;
  }

  get totalItens(): number {
    return this.itens.reduce((acc, i) => acc + i.quantidade, 0);
  }

  getIniciais(nome: string): string {
    return nome.split(' ').map((p: string) => p[0]).slice(0, 2).join('').toUpperCase();
  }


  voltar(): void {
    this.location.back();
  }

  selecionarPagamento(id: number): void {
  this.formaPagamento = id;
}

montarPedido(): PedidoRequest {
  const pedido: PedidoRequest = {
    clienteId: Number.parseInt(this.authService.getUsuarioId()?? '')?? 0,
    restauranteId: this.itens[0]?.idRestaurante ?? 0, // Assumindo que todos os itens são do mesmo restaurante
    entregadorId: null,
    dataCriacao: new Date(),
    horaSaida: null,
    horaEntrega: null,
    status: 0, // Status inicial do pedido
    itens: this.itens.map((item) => ({
      id: 0, // Será definido pelo backend
      pedidoId: 0, // Será definido pelo backend
      nome: item.nome,
      quantidade: item.quantidade,
      preco: item.preco,
      itemRestauranteId: item.idItem
    }))
  };
  return pedido;
}


  finalizarPedido(): void {
    this.pedidoService.setPedido(this.montarPedido());
    this.dialog.open(PagamentoComponente);
  }  
  
}
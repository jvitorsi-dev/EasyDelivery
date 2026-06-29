import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../../services/auth-service';
import { PedidoService } from '../../../services/pedido-service';
import { PedidoDetalhadoResponse } from '../../../models/pedido/pedido-detalhado-response';

@Component({
  selector: 'app-restaurante-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './restaurante-home-page.html',
  styleUrls: ['./restaurante-home-page.css'],
})
export class RestauranteHomePage implements OnInit {
  restaurante: any;
  pedidosNovos: PedidoDetalhadoResponse[] = [];
  pedidosAndamento: PedidoDetalhadoResponse[] = [];
  totalPedidosHoje = 0;
  faturamentoHoje = 0;
  aberto = true;

  private _snackBar = inject(MatSnackBar);
  private cdr = inject(ChangeDetectorRef);
  private pedidoService = inject(PedidoService);
  private authService = inject(AuthService);

  readonly statusLabel: Record<number, string> = {
    0: 'Novo',
    1: 'Pagamento pendente',
    2: 'Pago',
    3: 'Em preparação',
    4: 'Aguardando entregador',
    5: 'Em entrega',
    6: 'Entregue',
    7: 'Cancelado',
  };

  readonly statusAndamento = [2, 3, 4, 5];

  ngOnInit(): void {
    this.restaurante = this.authService.getUsuario();
    this.carregarPedidos();
  }

  carregarPedidos(): void {
    this.pedidoService.getPedidosByRestaurante(this.restaurante.id).subscribe({
      next: (res) => {
        const todos: PedidoDetalhadoResponse[] = res.data ?? [];

        this.pedidosNovos = todos.filter(
          (p: PedidoDetalhadoResponse) => p.status === 0 || p.status === 1
        );

        this.pedidosAndamento = todos.filter((p: PedidoDetalhadoResponse) =>
          this.statusAndamento.includes(p.status)
        );

        this.totalPedidosHoje = todos.filter((p: PedidoDetalhadoResponse) =>
          this.isHoje(p.dataCriacao)
        ).length;

        this.faturamentoHoje = todos
          .filter((p: PedidoDetalhadoResponse) => this.isHoje(p.dataCriacao) && p.status !== 7)
          .reduce((acc: number, p: PedidoDetalhadoResponse) => acc + p.valorTotal, 0);

        this.cdr.detectChanges();
      },
      error: () => this._snackBar.open('Erro ao carregar pedidos.', 'Ok'),
    });
  }

  aceitarPedido(id: number): void {
    this.pedidoService.atualizarStatusPedido(id, 3).subscribe({
      next: () => {
        this._snackBar.open('Pedido aceito!', 'Ok');
        this.carregarPedidos();
      },
      error: () => this._snackBar.open('Erro ao aceitar pedido.', 'Ok'),
    });
  }

  recusarPedido(id: number): void {
    this.pedidoService.atualizarStatusPedido(id, 7).subscribe({
      next: () => {
        this._snackBar.open('Pedido recusado.', 'Ok');
        this.carregarPedidos();
      },
      error: () => this._snackBar.open('Erro ao recusar pedido.', 'Ok'),
    });
  }

  avancarStatus(pedido: PedidoDetalhadoResponse): void {
    const proximo = pedido.status + 1;
    this.pedidoService.atualizarStatusPedido(pedido.id, proximo).subscribe({
      next: () => {
        this._snackBar.open('Status atualizado!', 'Ok');
        this.carregarPedidos();
      },
      error: () => this._snackBar.open('Erro ao atualizar status.', 'Ok'),
    });
  }

  toggleAberto(): void {
    this.aberto = !this.aberto;
  }

  isHoje(data: string): boolean {
    return new Date(data).toDateString() === new Date().toDateString();
  }

  getIniciais(nome: string): string {
    return nome?.split(' ').map((p: string) => p[0]).slice(0, 2).join('').toUpperCase();
  }
}
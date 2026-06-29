import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BottomNavComponent } from '../../../componentes/bottom-nav-component/bottom-nav-component';
import { AuthService } from '../../../services/auth-service';
import { EntregadorService } from '../../../services/entregador-service';
import { PedidoDetalhadoResponse } from '../../../models/pedido/pedido-detalhado-response';
import { RestauranteService } from '../../../services/restaurante-service';
import { RestauranteResponse } from '../../../models/restaurante/restauranteResponse';
import { TaskResult } from '../../../models/servico/taskResult';

@Component({
  selector: 'app-entregador-home-page',
  standalone: true,
  imports: [CommonModule, BottomNavComponent],
  templateUrl: './entregador-home-page.html',
  styleUrl: './entregador-home-page.css',
})
export class EntregadorHomePage implements OnInit {
  usuario: any = null;
  entregador: any = null;
  pedidosDisponiveis: PedidoDetalhadoResponse[] = [];
  meusPedidos: PedidoDetalhadoResponse[] = [];
  carregando = true;

  private snackBar = inject(MatSnackBar);

  constructor(
    private authService: AuthService,
    private entregadorService: EntregadorService,
    private restauranteService: RestauranteService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.usuario = this.authService.getUsuario();

    setTimeout(() => {
      this.carregarContextoEntregador();
    });
  }

  carregarContextoEntregador(): void {
    if (!this.usuario?.id) {
      this.carregando = false;
      this.cdr.detectChanges();
      this.snackBar.open('Usuário do entregador não encontrado.', 'Ok');
      return;
    }

    this.entregadorService.getEntregadorById(this.usuario.id).subscribe({
      next: (res) => {
        this.entregador = res.data;
        this.carregarPedidos();
      },
      error: () => {
        this.carregando = false;
        this.cdr.detectChanges();
        this.snackBar.open('Não foi possível localizar o entregador cadastrado.', 'Ok');
      },
    });
  }

  carregarPedidos(): void {
    this.restauranteService.getAllRestaurantes().subscribe({
      next: (res) => {
        const restaurantes: RestauranteResponse[] = res.data ?? [];
        const restauranteIds = restaurantes.map((r: RestauranteResponse) => r.id);

        if (restauranteIds.length === 0) {
          this.pedidosDisponiveis = [];
          this.meusPedidos = [];
          this.carregando = false;
          this.cdr.detectChanges();
          return;
        }

        this.entregadorService.getPedidosPorRestaurantes(restauranteIds).subscribe({
          next: (respostas) => {
            const pedidos = respostas
              .flatMap(
                (response: TaskResult<PedidoDetalhadoResponse[]>) => response.data ?? []
              )
              .filter(
                (pedido: PedidoDetalhadoResponse | null | undefined): pedido is PedidoDetalhadoResponse =>
                  !!pedido
              );

            const mapa = new Map<number, PedidoDetalhadoResponse>();
            pedidos.forEach((pedido: PedidoDetalhadoResponse) => {
              mapa.set(pedido.id, pedido);
            });

            const pedidosUnicos = Array.from(mapa.values());

            this.pedidosDisponiveis = pedidosUnicos.filter(
              (p: PedidoDetalhadoResponse) =>
                p.status === 4 && (!p.entregadorId || p.entregadorId === 0)
            );

            this.meusPedidos = pedidosUnicos.filter(
              (p: PedidoDetalhadoResponse) =>
                p.entregadorId === this.entregador?.id && p.status === 5
            );

            this.carregando = false;
            this.cdr.detectChanges();
          },
          error: () => {
            this.carregando = false;
            this.cdr.detectChanges();
            this.snackBar.open('Erro ao carregar pedidos dos restaurantes.', 'Ok');
          },
        });
      },
      error: () => {
        this.carregando = false;
        this.cdr.detectChanges();
        this.snackBar.open('Erro ao carregar restaurantes.', 'Ok');
      },
    });
  }

  aceitarPedido(pedidoId: number): void {
    if (!this.entregador?.id) {
      this.snackBar.open('Entregador não identificado.', 'Ok');
      return;
    }

    this.entregadorService.aceitarPedido(pedidoId, this.entregador.id).subscribe({
      next: () => {
        this.snackBar.open('Pedido aceito com sucesso!', 'Ok');
        this.carregarPedidos();
      },
      error: () => {
        this.snackBar.open('Erro ao aceitar pedido.', 'Ok');
      },
    });
  }

  concluirEntrega(pedidoId: number): void {
    this.entregadorService.atualizarStatusPedido(pedidoId, 6).subscribe({
      next: () => {
        this.snackBar.open('Entrega concluída!', 'Ok');
        this.carregarPedidos();
      },
      error: () => {
        this.snackBar.open('Erro ao concluir entrega.', 'Ok');
      },
    });
  }

  getIniciais(nome: string): string {
    return nome
      ?.split(' ')
      .map((p: string) => p[0])
      .slice(0, 2)
      .join('')
      .toUpperCase();
  }
}
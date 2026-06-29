import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BottomNavComponent } from '../../componentes/bottom-nav-component/bottom-nav-component';
import { PedidoDetalhadoResponse } from '../../models/pedido/pedido-detalhado-response';
import { PedidoService } from '../../services/pedido-service';

@Component({
  selector: 'app-pedido-detalhe-page',
  standalone: true,
  imports: [CommonModule, BottomNavComponent],
  templateUrl: './pedido-detalhe-page.html',
  styleUrl: './pedido-detalhe-page.css',
})
export class PedidoDetalhePage implements OnInit {
  pedido: PedidoDetalhadoResponse | null = null;
  private snackBar = inject(MatSnackBar);

  readonly statusLabel: Record<number, string> = {
    0: 'Criado',
    1: 'Pagamento pendente',
    2: 'Pago',
    3: 'Em preparação',
    4: 'Aguardando entregador',
    5: 'Em entrega',
    6: 'Entregue',
    7: 'Cancelado',
  };

  constructor(
    private route: ActivatedRoute,
    private pedidoService: PedidoService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.pedidoService.getPedidoById(id).subscribe({
      next: (res) => (this.pedido = res.data),
      error: () => this.snackBar.open('Erro ao carregar detalhes do pedido.', 'Ok'),
    });
  }
}

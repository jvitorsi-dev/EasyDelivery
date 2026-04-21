import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { inject } from '@angular/core';
import { BottomNavComponent } from '../../componentes/bottom-nav-component/bottom-nav-component';
import { PedidoService } from '../../services/pedido-service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth-service';
import { CarrinhoService } from '../../services/carrinho-service';

@Component({
  selector: 'app-pedidos',
  standalone: true,
  imports: [CommonModule, BottomNavComponent],
  templateUrl: './pedidos-page.html',
  styleUrls: ['./pedidos-page.css'],
})
export class PedidosPage implements OnInit {
  abaSelecionada: 'ativos' | 'historico' = 'ativos';
  pedidosAtivos: any[] = [];
  pedidosHistorico: any[] = [];
  usuarioId = 0;
  private _snackBar = inject(MatSnackBar);


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

  readonly statusAtivos = [2, 3, 4, 5]; 

  readonly timeline: { status: number; label: string }[] = [
  { status: 0, label: 'Criado' },
  { status: 1, label: 'Pagamento pendente' },
  { status: 2, label: 'Pago' },
  { status: 3, label: 'Em preparação' },
  { status: 4, label: 'Aguardando entregador' },
  { status: 5, label: 'Em entrega' },
  { status: 6, label: 'Entregue' },
];

  constructor(private pedidoService: PedidoService,
    private authService: AuthService,
    private carrinhoService: CarrinhoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {    
    this.usuarioId = Number.parseInt(this.authService.getUsuarioId()!) ?? 0;
    this.carregarPedidos();
    this.carrinhoService.limpar(); // Limpa o carrinho ao acessar a página de pedidos
  }

  carregarPedidos(): void {
  this.pedidoService.getPedidosByClienteId(this.usuarioId).subscribe({
    next: (res) => {
      this.pedidosAtivos    = res.data.filter((p: any) => this.statusAtivos.includes(p.status)).sort((a: any, b: any) => new Date(b.dataCriacao).getTime() - new Date(a.dataCriacao).getTime());
      this.pedidosHistorico = res.data.filter((p: any) => !this.statusAtivos.includes(p.status)).sort((a: any, b: any) => new Date(b.dataCriacao).getTime() - new Date(a.dataCriacao).getTime());
      this.cdr.detectChanges(); // ← força atualização
    },
    error: (err) => this._snackBar.open(err?.error, 'Ok'),
  });
  }

  cancelarPedido(id: number): void {
    this.pedidoService.cancelarPedido(id).subscribe({
      next: () => {
        this._snackBar.open('Pedido cancelado.', 'Ok');
        this.carregarPedidos();
      },
      error: (err) => this._snackBar.open(err?.error, 'Ok'),
    });
  }

  getStatusStep(status: number): number {
    return this.timeline.findIndex(t => t.status === status);
  }

  getIniciais(nome: string): string {
    return nome?.split(' ').map((p: string) => p[0]).slice(0, 2).join('').toUpperCase();
  }

  trocarAba(aba: 'ativos' | 'historico'): void {
    this.abaSelecionada = aba;
  }
}
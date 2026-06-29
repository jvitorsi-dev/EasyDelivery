import { Component, OnInit, inject } from '@angular/core';
import { PedidoService } from '../../services/pedido-service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CarrinhoService } from '../../services/carrinho-service';
import { environment } from '../../../environments/environment';

declare global {
  interface Window {
    MercadoPago: any;
  }
}

@Component({
  selector: 'app-pagamento-componente',
  imports: [MatDialogActions, MatDialogContent, MatDialogTitle, MatButtonModule],
  templateUrl: './pagamento-componente.html',
  styleUrl: './pagamento-componente.css',
})
export class PagamentoComponente implements OnInit {
  private _snackBar = inject(MatSnackBar);
  readonly dialogRef = inject(MatDialogRef<PagamentoComponente>);

  constructor(
    private pedidoService: PedidoService,
    private carrinhoService: CarrinhoService
  ) {}

  ngOnInit(): void {
    this.iniciarPagamento();
  }

  iniciarPagamento() {
    this.pedidoService.criarPedido().subscribe({
      next: (res) => {
        const preferenceId = res.data?.preferenceId;

        if (!preferenceId || !window.MercadoPago) {
          this._snackBar.open('Não foi possível iniciar o pagamento.', 'Fechar', {
            duration: 3000,
          });
          return;
        }

        const mp = new window.MercadoPago(environment.mercadoPagoPublicKey, {
          locale: 'pt-BR',
        });

        mp.bricks().create('wallet', 'wallet_container', {
          initialization: {
            preferenceId,
          },
        });

        this.pedidoService.clearPedido();
        this.carrinhoService.limpar();
      },
      error: () => {
        this._snackBar.open('Erro ao criar pedido. Tente novamente.', 'Fechar', {
          duration: 3000,
        });
      },
    });
  }

  fechar() {
    this.dialogRef.close();
  }
}

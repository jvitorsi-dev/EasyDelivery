import { Component, inject, OnInit } from '@angular/core';
import { PedidoRequest } from '../../models/pedido/pedidoRequest';
import { PedidoService } from '../../services/pedido-service';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent } from "@angular/material/card";
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CarrinhoService } from '../../services/carrinho-service';

@Component({
  selector: 'app-pagamento-componente',
  imports: [
  MatDialogActions,
  MatDialogContent,
  MatDialogTitle,
  MatButtonModule
],
  templateUrl: './pagamento-componente.html',
  styleUrl: './pagamento-componente.css',
})
export class PagamentoComponente implements OnInit{  
  private _snackBar = inject(MatSnackBar);
  readonly dialogRef = inject(MatDialogRef<PagamentoComponente>);
  pedidoId: number = 0;


  constructor(private pedidoService: PedidoService,
    private carrinhoService: CarrinhoService
  ){}

  ngOnInit(): void {
    this.iniciarPagamento();
    this.pedidoId = this.pedidoService.getPedido()?.id ?? 0;
  }

  async iniciarPagamento() {
    this.pedidoService.criarPedido().subscribe({
      next: (res) => {
        const mp = new window.MercadoPago('APP_USR-bbb06632-cf51-428f-861e-0db2b9814fb2', {
          locale: 'pt-BR'
        });

        mp.bricks().create("wallet", "wallet_container", {
          initialization: {
            preferenceId: res.data.preferenceId
          }
        });

        this.carrinhoService.limpar();
      },
      error: (err) => {
        this._snackBar.open('Erro ao criar pedido. Tente novamente.', 'Fechar', {
          duration: 3000,
        });
        return;
      }
    })    
  }

  fechar(){
    this.dialogRef.close();
  }

}

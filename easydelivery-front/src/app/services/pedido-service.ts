import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PedidoRequest } from '../models/pedido/pedidoRequest';
import { ItensPedidoRequest } from '../models/pedido/itensPedidoRequest';
import { StatusPedido } from '../models/enums/statusPedido';
import { PedidoResponse } from '../models/pedido/pedidoResponse';
import { TaskResult } from '../models/servico/taskResult';

@Injectable({
  providedIn: 'root',
})
export class PedidoService {
  private apiUrl = 'https://localhost:7158/api/Pedidos';

  constructor(private http: HttpClient){}

  //#region Clientes
  getPedidosByClienteId(id: number) {
    return this.http.get<any>(`${this.apiUrl}/pedidos-cliente/` + id );
  }

  criarPedido(){
    var pedido = this.getPedido();
    console.log(pedido);
    return this.http.post<any>(this.apiUrl + '/criar-pedido', pedido, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  cancelarPedido(id: number){
    return this.http.get(`${this.apiUrl}/cancelar` + id);
  }

  setPedido(pedido: PedidoRequest){
    localStorage.setItem('pedido', JSON.stringify(pedido));
  }

  getPedido(): PedidoRequest | null {
    const pedido = localStorage.getItem('pedido');
    if (!pedido) return null;
    return JSON.parse(pedido);
  }

  getPedidoById(id: number) {
    return this.http.get<TaskResult<PedidoResponse>>(`${this.apiUrl}/${id}`);
  }
  //#endregion

  // #region Restaurante
  getPedidosByRestaurante(id: number) {
    return this.http.get<any>(`${this.apiUrl}/pedidos-restaurante/` + id );
  }

  atualizarStatusPedido(id: number, status: number) {
    return this.http.get(`${this.apiUrl}/atualizar-status/${id}/${status}`);
  }

  // #endregion
}
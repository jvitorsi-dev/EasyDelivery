import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PedidoRequest } from '../models/pedido/pedidoRequest';
import { PedidoResponse } from '../models/pedido/pedidoResponse';
import { TaskResult } from '../models/servico/taskResult';
import { PedidoDetalhadoResponse } from '../models/pedido/pedido-detalhado-response';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PedidoService {
  private apiUrl = `${environment.apiUrl}/Pedidos`;

  constructor(private http: HttpClient) {}

  getPedidosByClienteId(id: number) {
    return this.http.get<TaskResult<PedidoDetalhadoResponse[]>>(`${this.apiUrl}/pedidos-cliente/${id}`);
  }

  criarPedido() {
    const pedido = this.getPedido();
    return this.http.post<TaskResult<PedidoResponse>>(this.apiUrl + '/criar-pedido', pedido, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }

  cancelarPedido(id: number) {
    return this.http.get<TaskResult<PedidoDetalhadoResponse>>(`${this.apiUrl}/cancelar/${id}`);
  }

  setPedido(pedido: PedidoRequest) {
    localStorage.setItem('pedido', JSON.stringify(pedido));
  }

  getPedido(): PedidoRequest | null {
    const pedido = localStorage.getItem('pedido');
    if (!pedido) return null;
    return JSON.parse(pedido) as PedidoRequest;
  }

  clearPedido() {
    localStorage.removeItem('pedido');
  }

  getPedidoById(id: number) {
    return this.http.get<TaskResult<PedidoDetalhadoResponse>>(`${this.apiUrl}/${id}`);
  }

  getPedidosByRestaurante(id: number) {
    return this.http.get<TaskResult<PedidoDetalhadoResponse[]>>(`${this.apiUrl}/pedidos-restaurante/${id}`);
  }

  atualizarStatusPedido(id: number, status: number) {
    return this.http.post<TaskResult<PedidoDetalhadoResponse>>(`${this.apiUrl}/atualizar-status`, {
      pedidoId: id,
      status,
    });
  }
}

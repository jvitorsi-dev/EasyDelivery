import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { TaskResult } from '../models/servico/taskResult';
import { PedidoDetalhadoResponse } from '../models/pedido/pedido-detalhado-response';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EntregadorService {
  private apiUrl = `${environment.apiUrl}/Entregador`;
  private pedidosApiUrl = `${environment.apiUrl}/Pedidos`;

  constructor(private http: HttpClient) {}

  getEntregadorById(id: number): Observable<TaskResult<any>> {
    return this.http.get<TaskResult<any>>(`${this.apiUrl}/${id}`);
  }

  aceitarPedido(pedidoId: number, entregadorId: number): Observable<TaskResult<any>> {
    return this.http.put<TaskResult<any>>(`${this.apiUrl}/aceitar-pedido`, {
      pedidoId,
      entregadorId,
      status: 5,
    });
  }

  getPedidosPorRestaurantes(restauranteIds: number[]): Observable<TaskResult<PedidoDetalhadoResponse[]>[]> {
    return forkJoin(
      restauranteIds.map((id) =>
        this.http.get<TaskResult<PedidoDetalhadoResponse[]>>(
          `${this.pedidosApiUrl}/pedidos-restaurante/${id}`
        )
      )
    );
  }

  atualizarStatusPedido(
    pedidoId: number,
    status: number
  ): Observable<TaskResult<PedidoDetalhadoResponse>> {
    return this.http.post<TaskResult<PedidoDetalhadoResponse>>(
      `${this.pedidosApiUrl}/atualizar-status`,
      {
        pedidoId,
        status,
      }
    );
  }
}
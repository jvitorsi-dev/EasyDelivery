import { StatusPedido } from '../enums/statusPedido';
import { ItensPedidoResponse } from './itensPedidoResponse';

export interface PessoaResumo {
  id: number;
  nome: string;
  endereco?: string;
  email?: string;
}

export interface RestauranteResumoPedido {
  id: number;
  nome: string;
  endereco: string;
  email: string;
}

export interface PedidoDetalhadoResponse {
  id: number;
  clienteId: number;
  cliente: PessoaResumo;
  restauranteId: number;
  restaurante: RestauranteResumoPedido;
  entregadorId: number | null;
  entregador?: PessoaResumo | null;
  preferenceId: string;
  status: StatusPedido;
  dataCriacao: string;
  horaSaida: string | null;
  horaEntrega: string | null;
  itens: ItensPedidoResponse[];
  valorTotal: number;
}

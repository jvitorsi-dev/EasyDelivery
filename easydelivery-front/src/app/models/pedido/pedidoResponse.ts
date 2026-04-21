import { StatusPedido } from "../enums/statusPedido";
import { ItensPedidoResponse } from "./itensPedidoResponse";

export interface PedidoResponse {
    id: Number;
    clienteId: Number;
    restauranteId: Number;
    entregadorId: Number | null;
    preferenceId: string;
    status: StatusPedido;
    dataCriacao: Date;
    horaSaida: Date | null;
    horaEntrega: Date | null;
    itens: ItensPedidoResponse[];
}
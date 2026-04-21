import { StatusPedido } from "../enums/statusPedido";
import { ItensPedidoRequest } from "./itensPedidoRequest";

export interface PedidoRequest{
    id?: number | null;
    clienteId: number;
    restauranteId: number;
    entregadorId: number | null;
    dataCriacao: Date;
    horaSaida: Date | null;
    horaEntrega: Date | null;
    status: StatusPedido;
    itens: ItensPedidoRequest[];
}


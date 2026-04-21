export interface ItensPedidoRequest{
    id: number;
    pedidoId: number;
    nome: string;
    quantidade: number;
    preco: number;
    itemRestauranteId: number;
    idRestaurante?: number;
}

import { CategoriaItensRestauranteResponse } from "./categoriaItensRestauranteResponse";


export interface ItemRestauranteResponse {
  idRestaurante: number;
  idItem: number;
  quantidade: number;
  nome: string;
  preco: number;
  categoria: CategoriaItensRestauranteResponse;
  descricao: string;
}
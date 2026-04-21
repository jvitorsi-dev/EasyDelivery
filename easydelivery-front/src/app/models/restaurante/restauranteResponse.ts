import { CategoriaRestauranteResponse } from "./categoriaRestauranteResponse";
import { ItemRestauranteResponse } from "./itemsRestauranteResponse";

export interface RestauranteResponse {
  id: number;
  nome: string;
  endereco: string;
  email: string;
  nota: number;
  categoria: CategoriaRestauranteResponse;
  itens: ItemRestauranteResponse[];
}
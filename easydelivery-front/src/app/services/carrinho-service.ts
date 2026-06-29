import { Injectable } from '@angular/core';
import { ItemRestauranteResponse } from '../models/restaurante/itemsRestauranteResponse';

@Injectable({
  providedIn: 'root',
})
export class CarrinhoService {
  setItens(itens: ItemRestauranteResponse[]): void {
    localStorage.setItem('carrinho', JSON.stringify(itens));
  }

  getItens(): ItemRestauranteResponse[] {
    const itens = localStorage.getItem('carrinho');
    if (!itens) return [];
    return JSON.parse(itens) as ItemRestauranteResponse[];
  }

  adicionarItem(item: ItemRestauranteResponse): void {
    const itens = this.getItens();
    const existente = itens.find((i) => i.idItem === item.idItem);

    if (existente) {
      existente.quantidade++;
    } else {
      itens.push({ ...item, quantidade: 1 });
    }

    this.setItens(itens);
  }

  removerItem(idItem: number): void {
    const itens = this.getItens().filter((i) => i.idItem !== idItem);
    this.setItens(itens);
  }

  limpar(): void {
    localStorage.removeItem('carrinho');
  }

  get totalItens(): number {
    return this.getItens().reduce((acc, i) => acc + i.quantidade, 0);
  }
}

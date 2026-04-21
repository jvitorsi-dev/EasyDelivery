import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class CarrinhoService {

  setItens(itens: any[]): void {
    localStorage.setItem('carrinho', JSON.stringify(itens));
  }

  getItens(): any[] {
    const itens = localStorage.getItem('carrinho');
    if (!itens) return []; // ← retorna array vazio se não existir
    return JSON.parse(itens);
  }

  adicionarItem(item: any): void {
    const itens = this.getItens();
    const existente = itens.find((i: any) => i.id === item.id);
    if (existente) {
      existente.quantidade++;
    } else {
      itens.push({ ...item, quantidade: 1 });
    }
    this.setItens(itens);
  }

  removerItem(id: number): void {
    const itens = this.getItens().filter((i: any) => i.id !== id);
    this.setItens(itens);
  }

  limpar(): void {
    localStorage.removeItem('carrinho');
  }

  get totalItens(): number {
    return this.getItens().reduce((acc: number, i: any) => acc + i.quantidade, 0);
  }
}
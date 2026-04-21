import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterLinkActive } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CarrinhoService } from '../../services/carrinho-service';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-bottom-nav',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './bottom-nav-component.html',
  styleUrls: ['./bottom-nav-component.css'],
})
export class BottomNavComponent {
  @Input() nomeUsuario = 'João';
  totalCarrinho = 0;

  navItems: NavItem[] = [
    { label: 'Início',   icon: 'home',              route: '/cliente/home' },
    { label: 'Carrinho', icon: 'shopping_cart',      route: '/cliente/carrinho' },
    { label: 'Pedidos',  icon: 'receipt_long',       route: '/cliente/pedidos' },
    { label: 'Perfil',   icon: 'person',             route: '/cliente/perfil' },
  ];

  constructor(private carrinhoService: CarrinhoService){}

  getIniciais(nome: string): string {
    return nome.split(' ').map(p => p[0]).slice(0, 2).join('').toUpperCase();
  }

  get totalItens(): number{
    return this.carrinhoService.totalItens;
  }
}
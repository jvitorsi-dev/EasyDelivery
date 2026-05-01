import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterLinkActive } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CarrinhoService } from '../../services/carrinho-service';
import { AuthService } from '../../services/auth-service';
import { UserRole } from '../../models/enums/userRole';

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
export class BottomNavComponent implements OnInit{
  @Input() nomeUsuario = 'João';
  totalCarrinho = 0;

  navItems: NavItem[] = [];

  navItemsCliente: NavItem[] = [
    { label: 'Início',   icon: 'home',              route: '/cliente/home' },
    { label: 'Carrinho', icon: 'shopping_cart',      route: '/cliente/carrinho' },
    { label: 'Pedidos',  icon: 'receipt_long',       route: '/cliente/pedidos' },
    { label: 'Perfil',   icon: 'person',             route: '/cliente/perfil' },
  ];

  navItemsRestaurante: NavItem[] = [
    { label: 'Início',   icon: 'home',              route: '/restaurante/home' },
    { label: 'Cardápio',  icon: 'receipt_long',       route: '/restaurante/cardapio' },
    { label: 'Perfil',   icon: 'person',             route: '/restaurante/perfil' },
  ];

  constructor(private carrinhoService: CarrinhoService,
    private authService: AuthService
  ){}

  ngOnInit(): void {
    var usuario = this.authService.getUsuario();
    if(usuario.role == UserRole.Cliente){
      this.navItems = this.navItemsCliente;
      this.carrinhoService.getItens();
    } else {
      this.navItems = this.navItemsRestaurante;
    }
  }

  getIniciais(nome: string): string {
    return nome.split(' ').map(p => p[0]).slice(0, 2).join('').toUpperCase();
  }

  get totalItens(): number{
    return this.carrinhoService.totalItens;
  }
}
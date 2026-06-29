import { Routes } from '@angular/router';
import { ClienteHomePage } from './pages/cliente-home-page/cliente-home-page';
import { RestauranteDetalhePage } from './pages/restaurante-detalhes-page/restaurante-detalhes-page';
import { CarrinhoPage } from './pages/carrinho-page/carrinho-page';
import { PagamentoComponente } from './componentes/pagamento-componente/pagamento-componente';
import { PedidosPage } from './pages/pedidos-page/pedidos-page';
import { RestauranteHomePage } from './pages/restaurante/restaurante-home-page/restaurante-home-page';
import { authGuard } from './guard/auth-guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/login-page/login-page').then((m) => m.LoginPage),
    data: { animation: 'LoginPage' },
  },
  {
    path: 'register',
    loadComponent: () => import('./pages/register-page/register-page').then((m) => m.RegisterPage),
    data: { animation: 'RegisterPage' },
  },
  {
    path: 'forgot',
    loadComponent: () => import('./pages/pass-forgot-page/pass-forgot-page').then((m) => m.ForgotPasswordComponent),
    data: { animation: 'ForgotPasswordComponent' },
  },
  {
    path: 'cliente',
    canActivate: [authGuard],
    children: [
      { path: 'home', component: ClienteHomePage, data: { animation: 'cliente-home' } },
      { path: 'restaurante/:id', component: RestauranteDetalhePage, data: { animation: 'cliente-restaurante' } },
      { path: 'carrinho', component: CarrinhoPage, data: { animation: 'cliente-carrinho' } },
      { path: 'pagamento', component: PagamentoComponente, data: { animation: 'cliente-pagamento' } },
      { path: 'pedidos', component: PedidosPage, data: { animation: 'cliente-pedidos' } },
      {
        path: 'perfil',
        loadComponent: () => import('./pages/usuario-perfil-page/usuario-perfil-page').then((m) => m.UsuarioPerfilPage),
        data: { animation: 'cliente-perfil' },
      },
    ],
    runGuardsAndResolvers: 'always',
  },
  {
    path: 'restaurante',
    canActivate: [authGuard],
    children: [
      { path: 'home', component: RestauranteHomePage, data: { animation: 'restaurante-home' } },
      {
        path: 'cardapio',
        loadComponent: () => import('./pages/restaurante/restaurante-cardapio-page/restaurante-cardapio-page').then((m) => m.RestauranteCardapioPage),
        data: { animation: 'restaurante-cardapio' },
      },
      {
        path: 'perfil',
        loadComponent: () => import('./pages/usuario-perfil-page/usuario-perfil-page').then((m) => m.UsuarioPerfilPage),
        data: { animation: 'restaurante-perfil' },
      },
    ],
  },
  {
  path: 'entregador',
  canActivate: [authGuard],
  children: [
    {
      path: 'home',
      loadComponent: () =>
        import('./pages/entregador/entregador-home-page/entregador-home-page')
          .then((m) => m.EntregadorHomePage),
      data: { animation: 'entregador-home' },
    },
    {
      path: 'perfil',
      loadComponent: () =>
        import('./pages/usuario-perfil-page/usuario-perfil-page')
          .then((m) => m.UsuarioPerfilPage),
      data: { animation: 'entregador-perfil' },
    },
  ],
},
  {
    path: '**',
    redirectTo: '',
  },
];

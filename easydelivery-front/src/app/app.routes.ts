import { Routes } from '@angular/router';
import { ClienteHomePage } from './pages/cliente-home-page/cliente-home-page';
import { RestauranteDetalhePage } from './pages/restaurante-detalhes-page/restaurante-detalhes-page';
import { CarrinhoPage } from './pages/carrinho-page/carrinho-page';
import { PagamentoComponente } from './componentes/pagamento-componente/pagamento-componente';
import { PedidosPage } from './pages/pedidos-page/pedidos-page';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/login-page/login-page')
        .then(m => m.LoginPage),
        data: { animation: 'LoginPage' }
    },
    {
        path: 'register',
        loadComponent: () => import('./pages/register-page/register-page')
        .then(m => m.RegisterPage),
        data: { animation: 'RegisterPage' }
    },
    {
        path:'forgot',
        loadComponent: () => import('./pages/pass-forgot-page/pass-forgot-page')
        .then(m => m.ForgotPasswordComponent),
        data: { animation: 'ForgotPasswordComponent' }
    },
    {
        path: 'cliente',
        children: [
                { path: 'home', component: ClienteHomePage,  data: { animation: 'cliente-home' } },
                { path: 'restaurante/:id', component: RestauranteDetalhePage, data: { animation: 'cliente-restaurante' }  },
                { path: 'carrinho', component: CarrinhoPage, data: { animation: 'cliente-carrinho' }  },
                { path: 'pagamento', component: PagamentoComponente, data: { animation: 'cliente-pagamento' }  },
                { path: 'pedidos', component: PedidosPage, data: { animation: 'cliente-pedidos' }  }
            ],
        runGuardsAndResolvers: 'always'
        }
];

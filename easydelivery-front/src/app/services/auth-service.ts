import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/usuario/registerRequest';
import { LoginResponse } from '../models/usuario/loginResponse';
import { UsuarioResponse } from '../models/usuario/usuarioReponse';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrlLogin = `${environment.apiUrl}/Auth`;
  private apiUrlUsuario = `${environment.apiUrl}/Usuario`;

  constructor(private http: HttpClient) {}

  login(email: string, senha: string) {
    return this.http.post<LoginResponse>(this.apiUrlLogin, { email, senha });
  }

  register(registerRequest: RegisterRequest) {
    return this.http.post(`${this.apiUrlUsuario}/criar`, registerRequest);
  }

  getUsuarioId() {
    return localStorage.getItem('usuarioId');
  }

  setUsuario(usuarioLogado: UsuarioResponse) {
    localStorage.setItem('usuario', JSON.stringify(usuarioLogado));
    localStorage.setItem('usuarioId', usuarioLogado.id.toString());
  }

  getUsuario(): UsuarioResponse | null {
    const usuarioStr = localStorage.getItem('usuario');
    if (!usuarioStr) return null;

    try {
      return JSON.parse(usuarioStr) as UsuarioResponse;
    } catch {
      return null;
    }
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    localStorage.removeItem('usuarioId');
    localStorage.removeItem('pedido');
    localStorage.removeItem('carrinho');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

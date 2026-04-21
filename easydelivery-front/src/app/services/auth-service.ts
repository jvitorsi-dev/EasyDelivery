import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/usuario/registerRequest';
import { TaskResult } from '../models/servico/taskResult';
import { LoginResponse } from '../models/usuario/loginResponse';
import { UsuarioResponse } from '../models/usuario/usuarioReponse';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private apiUrlLogin = 'https://localhost:7158/api/Auth';
  private apiUrlUsuario = 'https://localhost:7158/api/Usuario';

  constructor(private http: HttpClient) {}

  login(email: string, senha: string) {
    return this.http.post<LoginResponse>(`${this.apiUrlLogin}`, { email, senha });
  }

  register(registerRequest: RegisterRequest){
    return this.http.post(`${this.apiUrlUsuario}/criar`, registerRequest);
  }

  getUsuarioId(){
    return localStorage.getItem('usuarioId');
  }

  setUsuario(usuarioLogado: UsuarioResponse){
    var usuario = JSON.stringify(usuarioLogado);
    localStorage.setItem('usuario', usuario);
    localStorage.setItem('usuarioId', usuarioLogado.id.toString())
  }

  getUsuario(){
    var usuario = new UsuarioResponse;
    var usuarioStr = localStorage.getItem('usuario');
    usuario = JSON.parse(usuarioStr!);
    return usuario;
  }

  logout() {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  setToken(token: string){
    localStorage.setItem('token', token);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

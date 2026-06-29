import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RestauranteResponse } from '../models/restaurante/restauranteResponse';
import { TaskResult } from '../models/servico/taskResult';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RestauranteService {
  private apiUrlRestaurante = `${environment.apiUrl}/Restaurante`;

  constructor(private http: HttpClient) {}

  getAllRestaurantes() {
    return this.http.get<TaskResult<RestauranteResponse[]>>(this.apiUrlRestaurante);
  }

  getRestauranteById(restauranteId: number) {
    return this.http.get<TaskResult<RestauranteResponse>>(`${this.apiUrlRestaurante}/${restauranteId}`);
  }
}

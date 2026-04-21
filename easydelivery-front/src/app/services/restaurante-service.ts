import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RestauranteResponse } from '../models/restaurante/restauranteResponse';
import { TaskResult } from '../models/servico/taskResult';

@Injectable({
  providedIn: 'root',
})
export class RestauranteService {
  private apiUrlRestaurante = 'https://localhost:7158/api/Restaurante';

  constructor(private http: HttpClient){}

  getAllRestaurantes(){
    return this.http.get<TaskResult<RestauranteResponse[]>>(this.apiUrlRestaurante);
  }

  getRestauranteById(restauranteId: number){
    return this.http.get<TaskResult<RestauranteResponse>>(this.apiUrlRestaurante + '/' + restauranteId);
  }

  

}

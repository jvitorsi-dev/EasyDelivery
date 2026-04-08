using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IRestauranteService
    {
        public Task<TaskResult<RestauranteResponse>> GetRestaurante(int id);
        public Task<TaskResult<List<RestauranteResponse>>> GetAllRestaurantes();
        public Task<TaskResult<RestauranteResponse>> UpdateRestaurante(RestauranteRequest restaurante);
        public Task<TaskResult<List<RestauranteResponse>>> SearchRestaurantes(string nome);
        public Task<TaskResult<List<ItemRestauranteResponse>>> AddItemRestauranteRange(List<ItemRestauranteRequest> itens);
    }
}

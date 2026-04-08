using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IItemRestauranteService
    {
        public Task<TaskResult<int>> GetQtdeEstoqueItem(int idRestaurante, int idItem);
        public Task<TaskResult<List<ItemRestaurante>>> GetItemRestaurante(int idRestaurante);
        public Task<TaskResult<ItemRestaurante>> GetItemRestauranteById(int idRestaurante, int idItem);
        public Task<TaskResult<string>> UpdateItemRestaurante(List<ItemRestauranteRequest> itens);
    }
}

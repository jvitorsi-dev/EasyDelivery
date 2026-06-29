using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface ICategoriaRestauranteRepository
    {
        public Task<CategoriaRestaurante?> GetCategoria(int id);
        public Task<List<CategoriaRestaurante>> GetCategorias(List<int> ids);
    }
}

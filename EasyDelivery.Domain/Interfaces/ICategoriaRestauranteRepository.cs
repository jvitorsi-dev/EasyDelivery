using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface ICategoriaRestauranteRepository
    {
        public Task<CategoriaItemRestaurante?> GetCategoria(int id);
        public Task<List<CategoriaItemRestaurante>> GetCategorias(List<int> ids);
    }
}

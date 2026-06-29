using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface ICategoriaItemRestauranteRepository
    {
        public Task<List<CategoriaItensRestaurante>> GetCategoriasItens(List<int> itensId);
    }
}

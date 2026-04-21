using EasyDelivery.Application.DTOs.Categoria;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IRestauranteRepository _restauranteRepository;
        private readonly ICategoriaRestauranteRepository _categoriaRepository;
        private readonly ICategoriaItemRestauranteRepository _categoriaItemRepository;

        public CategoriaService(IRestauranteRepository restauranteRepository, 
            ICategoriaRestauranteRepository categoriaRepository,
            ICategoriaItemRestauranteRepository categoriaItemRepository)
        {
            _restauranteRepository = restauranteRepository;
            _categoriaRepository = categoriaRepository;
            _categoriaItemRepository = categoriaItemRepository;
        }
    }
}

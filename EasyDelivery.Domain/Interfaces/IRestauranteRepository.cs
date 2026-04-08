using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IRestauranteRepository
    {
        public Task<Restaurante?> GetRestaurante(int id);
        public Task<Restaurante?> GetRestauranteByNameEmail(string name, string email);
        public Task<List<Restaurante>> GetAllRestaurantes();
        public Task<List<Restaurante>> SearchRestaurantes(string nome);
        public Task AddRestaurante(Restaurante restaurante);
        public Task UpdateRestaurante(Restaurante restaurante);
    }
}

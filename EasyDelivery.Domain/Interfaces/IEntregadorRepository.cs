using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IEntregadorRepository
    {
        public Task<List<Entregador>> GetAllEntregadores();
        public Task<Entregador?> GetEntregadorById(int? id);
        public Task AddEntregador(Entregador entregador);
        public Task UpdateEntregador(Entregador entregador);
        public Task<Entregador?> GetEntregadorByEmail(string email);
    }
}

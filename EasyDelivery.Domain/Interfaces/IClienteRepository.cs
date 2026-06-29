using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IClienteRepository
    {
        public Task<Cliente?> GetCliente(int id);
        public Task AdicionarCliente(Cliente cliente);
        public Task<Cliente?> GetClienteByUsuarioId(int userId);
        public Task<Cliente?> GetClienteByEmail(string email);
    }
}

using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IClienteService
    {
        public Task<TaskResult<ClienteResponse>> GetCliente(int id);
    }
}

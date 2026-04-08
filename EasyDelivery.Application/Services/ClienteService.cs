using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;

namespace EasyDelivery.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<TaskResult<ClienteResponse>> GetCliente(int id)
        {
            try
            {
                var cliente = await _clienteRepository.GetCliente(id);
                if (cliente == null)
                    return TaskResult<ClienteResponse>.Fail("Cliente não encontrado.");

                var clienteResponse = new ClienteResponse
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Endereco = cliente.Endereco,
                    Email = cliente.Email,
                };  

                return TaskResult<ClienteResponse>.Ok(clienteResponse, "Cliente obtido com sucesso.");
            }
            catch
            {
                return TaskResult<ClienteResponse>.Fail("Erro ao obter cliente.");
            }
        }

    }
}

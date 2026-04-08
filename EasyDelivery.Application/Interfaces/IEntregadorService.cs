using EasyDelivery.Application.DTOs.Entregador;
using EasyDelivery.Domain.Entities;

namespace EasyDelivery.Application.Interfaces
{
    public interface IEntregadorService
    {
        public Task<TaskResult<List<EntregadorResponse>>> GetAllEntregadoresAsync();
        public Task<TaskResult<EntregadorResponse>> GetEntregadorById(int id);
        public Task<TaskResult<EntregadorResponse>> UpdateEntregador(EntregadorRequest entregadorRequest);
    }
}

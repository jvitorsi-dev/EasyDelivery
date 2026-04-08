using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IPedidoService
    {
        public Task<TaskResult<PedidoResponse>> AdicionarPedido(PedidoRequest pedidoRequest);
        public Task<TaskResult<PedidoResponse>> AtualizarStatusPedido(int pedidoId, StatusPedido novoStatus);
        public Task<TaskResult<PedidoResponse>> EditarPedido(PedidoRequest pedidoRequest);
        public Task<TaskResult<PedidoResponse>> ObterPedidoPorId(int id);
        public Task<TaskResult<List<PedidoResponse>>> GetPedidosPorRestaurante(int restauranteId, StatusPedido status = default);
        public Task<TaskResult<List<PedidoResponse>>> GetPedidosPorCliente(int clienteId, StatusPedido status = default);
    }
}

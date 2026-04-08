using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        public Task Adicionar(Pedido pedido);
        public Task<Pedido?> ObterPorId(int id);
        public Task AtualizarStatus(int id, StatusPedido novoStatus);
        public Task EditarPedido(Pedido pedido);
        public Task<List<Pedido>> ObterPorClienteStatus(int clienteId, StatusPedido status = default);
        public Task<List<Pedido>> ObterPorRestauranteStatus(int restauranteId, StatusPedido status = default);
    }
}

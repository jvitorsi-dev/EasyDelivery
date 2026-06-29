using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IItemPedidoRepository
    {
        public Task<List<ItemPedido>> GetItensPedido(int pedidoId);
    }
}

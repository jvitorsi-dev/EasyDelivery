using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        public Task Adicionar(Pagamento pagamento);
        public Task<Pagamento?> ObterPorId(int id);
        public Task AtualizarStatus(int id, StatusPagamento novoStatus);
    }
}

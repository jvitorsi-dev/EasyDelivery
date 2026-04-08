using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly EasyDeliveryContext _context;

        public PagamentoRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task<Pagamento?> ObterPorId(int id)
        {
            return await _context.Pagamentos.FindAsync(id);
        }

        public async Task AtualizarStatus(int id, StatusPagamento novoStatus)
        {
            var pagamento = _context.Pagamentos.Find(id);
            if (pagamento != null)
            {
                pagamento.Status = novoStatus;
                await _context.SaveChangesAsync();
            }
        }
    }
}

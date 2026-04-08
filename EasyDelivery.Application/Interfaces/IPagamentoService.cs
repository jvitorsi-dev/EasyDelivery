using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IPagamentoService
    {
        public Task<TaskResult<PagamentoResponse>> ProcessarPagamento(PagamentoRequest pagamentoRequest);
    }
}

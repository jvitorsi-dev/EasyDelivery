using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        public PagamentoService(IPagamentoRepository pagamentoRepository,
            IPedidoRepository pedidoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<TaskResult<PagamentoResponse>> ProcessarPagamento(PagamentoRequest pagamentoRequest)
        {
            var pedido = await _pedidoRepository.ObterPorId(pagamentoRequest.PedidoId);
            if (pedido == null)
                return TaskResult<PagamentoResponse>.Fail("Pedido não encontrado");
            // Lógica de processamento de pagamento
            var pagamento = new Pagamento
            {
                PedidoId = pagamentoRequest.PedidoId,
                Metodo = pagamentoRequest.Metodo,
                Status = StatusPagamento.Pendente
            };
            try
            {                
                await _pagamentoRepository.Adicionar(pagamento);
                // Simula o processamento do pagamento
                await Task.Delay(2000); // Simula um tempo de processamento
                // Atualiza o status do pagamento para concluído
                await _pagamentoRepository.AtualizarStatus(pagamento.Id, StatusPagamento.Aprovado);
                await _pedidoRepository.AtualizarStatus(pagamento.PedidoId, StatusPedido.Pago);

                var pagamentoResponse = new PagamentoResponse
                {
                    Id = pagamento.Id,
                    PedidoId = pagamento.PedidoId
                };  

                return TaskResult<PagamentoResponse>.Ok(pagamentoResponse, "Pagamento aprovado!");
            }
            catch
            {
                await _pagamentoRepository.AtualizarStatus(pagamento.Id, StatusPagamento.Recusado);
                return TaskResult<PagamentoResponse>.Fail("Erro ao processar pagamento");
            }
        }
    }
}

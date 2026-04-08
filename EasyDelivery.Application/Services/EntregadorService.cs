using EasyDelivery.Application.DTOs.Entregador;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Services
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _entregadorRepository;

        public EntregadorService(IEntregadorRepository entregadorRepository)
        {
            _entregadorRepository = entregadorRepository;
        }

        public async Task<TaskResult<List<EntregadorResponse>>> GetAllEntregadoresAsync()
        {
            try
            {
                var entregadores = await _entregadorRepository.GetAllEntregadores();
                var entregadorResponses = new List<EntregadorResponse>();
                entregadorResponses.AddRange(entregadores.Select(e => new EntregadorResponse
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Disponivel = e.Disponivel
                }));
                return TaskResult<List<EntregadorResponse>>.Ok(entregadorResponses, "Entregadores obtidos com sucesso!");

            }
            catch
            {
                // Log the exception (not implemented here)
                return TaskResult<List<EntregadorResponse>>.Fail("Ocorreu um erro ao obter os entregadores.");
            }
        }

        public async Task<TaskResult<EntregadorResponse>> GetEntregadorById(int id)
        {
            try
            {
                var entregador = await _entregadorRepository.GetEntregadorById(id);
                if (entregador == null)
                {
                    return TaskResult<EntregadorResponse>.Fail("Entregador não encontrado.");
                }
                var entregadorResponse = new EntregadorResponse
                {
                    Id = entregador.Id,
                    Nome = entregador.Nome,
                    Disponivel = entregador.Disponivel
                };
                return TaskResult<EntregadorResponse>.Ok(entregadorResponse, "Entregador obtido com sucesso.");
            }
            catch
            {
                return TaskResult<EntregadorResponse>.Fail("Ocorreu um erro ao obter o entregador.");
            }
        }

        public async Task<TaskResult<EntregadorResponse>> UpdateEntregador(EntregadorRequest entregadorRequest)
        {
            try
            {
                var entregador = await _entregadorRepository.GetEntregadorById(entregadorRequest.Id);
                if (entregador == null)
                    return TaskResult<EntregadorResponse>.Fail("Entregador não encontrado.");

                var novoEntregador = new Entregador
                {
                    Id = entregador.Id,
                    Nome = entregadorRequest.Nome,
                    Disponivel = entregadorRequest.Disponivel
                };

                await _entregadorRepository.UpdateEntregador(novoEntregador);

                var entregadorResponse = new EntregadorResponse
                {
                    Id = novoEntregador.Id,
                    Nome = novoEntregador.Nome,
                    Disponivel = novoEntregador.Disponivel
                };

                return TaskResult<EntregadorResponse>.Ok(entregadorResponse, "Entregador atualizado com sucesso!");
            }
            catch
            {
                return TaskResult<EntregadorResponse>.Fail("Ocorreu um erro ao atualizar o entregador.");
            }
        }
    }
}

using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Services
{
    public class ItemRestauranteService : IItemRestauranteService
    {
        private readonly IItemRestauranteRepository _itemRestauranteRepository;
        private readonly IRestauranteRepository _restauranteRepository;

        public ItemRestauranteService(IItemRestauranteRepository itemRestauranteRepository,
            IRestauranteRepository restauranteRepository)
        {
            _itemRestauranteRepository = itemRestauranteRepository;
            _restauranteRepository = restauranteRepository;
        }

        public async Task<TaskResult<int>> GetQtdeEstoqueItem(int idRestaurante, int idItem)
        {
            var restaurante = await _restauranteRepository.GetRestaurante(idRestaurante);
            if (restaurante == null)
                return TaskResult<int>.Fail("Restaurante não encontrado.");

            var item = await _itemRestauranteRepository.GetItemRestauranteById(idRestaurante, idItem);
            if (item == null)
                return TaskResult<int>.Fail("Item não encontrado para o restaurante.");

            var qtdeEstoque = await _itemRestauranteRepository.GetQtdeEstoqueItem(idRestaurante, idItem);

            return TaskResult<int>.Ok(qtdeEstoque, "Quantidade do Item obtida com sucesso!");
        }

        public async Task<TaskResult<List<ItemRestaurante>>> GetItemRestaurante(int idRestaurante)
        {
            var restaurante = await _restauranteRepository.GetRestaurante(idRestaurante);
            if (restaurante == null)
                return TaskResult<List<ItemRestaurante>>.Fail("Restaurante não encontrado.");

            var itemRestaurante = await _itemRestauranteRepository.GetItemRestaurante(idRestaurante);
            if (itemRestaurante == null || itemRestaurante.Count == 0)
                return TaskResult<List<ItemRestaurante>>.Fail("Nenhum item encontrado para o restaurante.");

            return TaskResult<List<ItemRestaurante>>.Ok(itemRestaurante, "Item obtido com sucesso!");
        }

        public async Task<TaskResult<ItemRestaurante>> GetItemRestauranteById(int idRestaurante, int idItem)
        {
            var restaurante = await _restauranteRepository.GetRestaurante(idRestaurante);
            if (restaurante == null)
                return TaskResult<ItemRestaurante>.Fail("Restaurante não encontrado.");
            var item = await _itemRestauranteRepository.GetItemRestauranteById(idRestaurante, idItem);
            if (item == null)
                return TaskResult<ItemRestaurante>.Fail("Item não encontrado para o restaurante.");
            return TaskResult<ItemRestaurante>.Ok(item, "Item obtido com sucesso!");
        }
        public async Task<TaskResult<string>> UpdateItemRestaurante(List<ItemRestauranteRequest> itens)
        {
            try
            {
                var itensRestaurante = await _itemRestauranteRepository.GetItensRestauranteByIds(itens.Select(i => i.IdItem).ToList());
                List<ItemRestauranteRequest> itensNaoEncontrados = itens
                    .Where(i => !itensRestaurante
                    .Any(ir => ir.Id == i.IdItem && ir.RestauranteId == i.IdRestaurante)).ToList();                   
                
                if(itensNaoEncontrados.Count > 0)
                {
                    var mensagem = "Os seguintes itens não foram encontrados para o restaurante: ";
                    foreach (var itemNaoEncontrado in itensNaoEncontrados)
                    {
                        mensagem = string.Join("\n ", itemNaoEncontrado.Nome);                        
                    }

                    return TaskResult<string>.Fail(mensagem);
                }

                var itensExistentesDic = itensRestaurante.ToDictionary(i => i.Id, i => i);

                foreach (var item in itens)
                {
                    var itemDb = itensExistentesDic[item.IdItem];

                    itemDb.QuantidadeEstoque -= item.Quantidade;
                }

                await _itemRestauranteRepository.UpdateItemRestaurante(itensRestaurante);
                return TaskResult<string>.Ok("Itens atualizados com sucesso.");
            }
            catch
            {
                return TaskResult<string>.Fail("Erro ao atualizar itens.");
            }
        }
    }
}

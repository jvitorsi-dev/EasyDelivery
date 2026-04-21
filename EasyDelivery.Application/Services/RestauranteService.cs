using EasyDelivery.Application.DTOs.Categoria;
using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Services
{
    public class RestauranteService : IRestauranteService
    {
        private readonly IRestauranteRepository _restauranteRepository;
        private readonly IItemRestauranteRepository _itemRestauranteRepository;
        private readonly ICategoriaRestauranteRepository _categoriaRestauranteRepository;
        private readonly ICategoriaItemRestauranteRepository _categoriaItemRestauranteRepository;

        public RestauranteService(IRestauranteRepository restauranteRepository, 
            IItemRestauranteRepository itemRestauranteRepository,
            ICategoriaRestauranteRepository categoriaRestauranteRepository,
            ICategoriaItemRestauranteRepository categoriaItemRestauranteRepository)
        {
            _restauranteRepository = restauranteRepository;
            _itemRestauranteRepository = itemRestauranteRepository;
            _categoriaRestauranteRepository = categoriaRestauranteRepository;
            _categoriaItemRestauranteRepository = categoriaItemRestauranteRepository;
        }

        public async Task<TaskResult<RestauranteResponse>> GetRestaurante(int id)
        {
            var restaurante = await _restauranteRepository.GetRestaurante(id);
            if (restaurante == null)
                return TaskResult<RestauranteResponse>.Fail("Restaurante não encontrado.");

            restaurante.Itens = await _itemRestauranteRepository.GetItemRestaurante(id);
            restaurante.Categoria = await _categoriaRestauranteRepository.GetCategoria(restaurante.CategoriaId);
            var categoriasItens = await _categoriaItemRestauranteRepository.GetCategoriasItens(restaurante.Itens.Select(i => i.Id).ToList());

            var restauranteResponse = new RestauranteResponse
            {
                Id = restaurante.Id,
                Nome = restaurante.Nome,
                Endereco = restaurante.Endereco,
                Email = restaurante.Email,
                Nota = restaurante.Nota,
                Itens = restaurante.Itens.Select(i => new ItemRestauranteResponse
                {
                    IdItem = i.Id,
                    IdRestaurante = i.RestauranteId,
                    Nome = i.Nome,
                    Quantidade = i.QuantidadeEstoque,
                    Preco = i.Preco,
                    CategoriaId = i.CategoriaId!.Value,
                    Categoria = new CategoriaItemRestauranteResponse
                    {
                        Id = i.CategoriaId!.Value,
                        Nome = categoriasItens.FirstOrDefault(ci => ci.Id == i.CategoriaId)!.Nome
                    },
                    Descricao = i.Descricao!
                }).ToList(),
                Categoria = new CategoriaRestauranteResponse
                {
                    Id = restaurante.CategoriaId,
                    Nome = restaurante.Categoria!.Nome,
                }
            }; 

            return TaskResult<RestauranteResponse>.Ok(restauranteResponse, "Restaurante obtido com sucesso!");
        }

        public async Task<TaskResult<List<RestauranteResponse>>> GetAllRestaurantes()
        {
            var restaurantes = await _restauranteRepository.GetAllRestaurantes();
            if (restaurantes == null || restaurantes.Count == 0)
                return TaskResult<List<RestauranteResponse>>.Fail("Nenhum restaurante encontrado.");

            foreach(var restaurante in restaurantes)
            {
                restaurante.Itens = await _itemRestauranteRepository.GetItemRestaurante(restaurante.Id);
                restaurante.Categoria = await _categoriaRestauranteRepository.GetCategoria(restaurante.CategoriaId);
            }

            var restauranteResponse = restaurantes
                .Select(r => new RestauranteResponse
                {
                    Id = r.Id,
                    Nome = r.Nome,
                    Endereco = r.Endereco,
                    Email = r.Email,
                    Nota = r.Nota,
                    Categoria = new CategoriaRestauranteResponse
                    {
                        Id = r.CategoriaId,
                        Nome = r.Categoria.Nome,
                    },
                    Itens = r.Itens.Select(i => new ItemRestauranteResponse
                    {
                        IdItem = i.Id,
                        IdRestaurante = i.RestauranteId,
                        Nome = i.Nome,
                        Quantidade = i.QuantidadeEstoque,
                        Preco = i.Preco
                    }).ToList()
                }).ToList();

            return TaskResult<List<RestauranteResponse>>.Ok(restauranteResponse, "Restaurantes obtido com sucesso!");
        }

        public async Task<TaskResult<RestauranteResponse>> UpdateRestaurante(RestauranteRequest restaurante)
        {
            var restauranteExistente = await _restauranteRepository.GetRestaurante(restaurante.Id);
            if (restauranteExistente == null)
                return TaskResult<RestauranteResponse>.Fail("Restaurante não encontrado.");

            try
            {
                Restaurante restauranteAtualizado = new Restaurante
                {
                    Id = restauranteExistente.Id,
                    Nome = restaurante.Nome,
                    Endereco = restaurante.Endereco,
                    Email = restaurante.Email,
                };
                await _restauranteRepository.UpdateRestaurante(restauranteAtualizado);

                return TaskResult<RestauranteResponse>.Ok(new RestauranteResponse
                {
                    Id = restauranteAtualizado.Id,
                    Nome = restauranteAtualizado.Nome,
                    Endereco = restauranteAtualizado.Endereco,
                    Email = restauranteAtualizado.Email,
                    Nota = restauranteAtualizado.Nota,
                    Categoria = new CategoriaRestauranteResponse
                    {
                        Id = restauranteAtualizado.CategoriaId,
                        Nome = restauranteAtualizado.Categoria.Nome
                    }
                }, "Restaurante atualizado com sucesso.");
            }
            catch
            {
                return TaskResult<RestauranteResponse>.Fail($"Erro ao atualizar restaurante.");
            }
        }

        public async Task<TaskResult<List<RestauranteResponse>>> SearchRestaurantes(string nome)
        {
            var restaurantes = await _restauranteRepository.SearchRestaurantes(nome);
            if (restaurantes == null)
                return TaskResult<List<RestauranteResponse>>.Fail("Nenhum restaurante encontrado com esse nome.");

            foreach (var restaurante in restaurantes)
            {
                restaurante.Itens = await _itemRestauranteRepository.GetItemRestaurante(restaurante.Id);
                restaurante.Categoria = await _categoriaRestauranteRepository.GetCategoria(restaurante.CategoriaId);
            }

            var restauranteResponse = restaurantes
                .Select(r => new RestauranteResponse
                {
                    Id = r.Id,
                    Nome = r.Nome,
                    Endereco = r.Endereco,
                    Email = r.Email,
                    Nota = r.Nota,
                    Itens = r.Itens.Select(i => new ItemRestauranteResponse
                    {
                        IdItem = i.Id,
                        IdRestaurante = i.RestauranteId,
                        Nome = i.Nome,
                        Quantidade = i.QuantidadeEstoque,
                        Preco = i.Preco
                    }).ToList(),
                    Categoria = new CategoriaRestauranteResponse
                    {
                        Id = r.CategoriaId,
                        Nome = r.Categoria.Nome,
                    }
                }).ToList();

            return TaskResult<List<RestauranteResponse>>.Ok(restauranteResponse, "Restaurantes obtidos com sucesso!");
        }

        public async Task<TaskResult<List<ItemRestauranteResponse>>> AddItemRestauranteRange(List<ItemRestauranteRequest> itens)
        {
            try
            {
                var itensAdicionados = new List<ItemRestauranteResponse>();
                foreach (var item in itens)
                {
                    var itensRequest = new ItemRestaurante
                    {
                        RestauranteId = item.IdRestaurante,
                        Nome = item.Nome,
                        QuantidadeEstoque = item.Quantidade,
                        Preco = item.Preco
                    };

                    var restaurante = await _restauranteRepository.GetRestaurante(item.IdRestaurante);
                    if (restaurante == null)
                        return TaskResult<List<ItemRestauranteResponse>>.Fail($"Restaurante com ID {item.IdRestaurante} não encontrado.");

                    var itemAdicionado = await _itemRestauranteRepository.AddItemRestaurante(itensRequest);

                    var itensResponse = new ItemRestauranteResponse
                    {
                        IdRestaurante = itemAdicionado.RestauranteId,
                        IdItem = itemAdicionado.Id,
                        Nome = itemAdicionado.Nome,
                        Quantidade = itemAdicionado.QuantidadeEstoque,
                        Preco = itemAdicionado.Preco
                    };

                    itensAdicionados.Add(itensResponse);
                }
                return TaskResult<List<ItemRestauranteResponse>>.Ok(itensAdicionados, "Itens adicionados com sucesso.");
            }
            catch
            {
                return TaskResult<List<ItemRestauranteResponse>>.Fail("Erro ao adicionar itens ao restaurante.");
            }
        }
    }
}

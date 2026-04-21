using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.Entregador;
using EasyDelivery.Application.DTOs.ItemPedido;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;

namespace EasyDelivery.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IRestauranteRepository _restauranteRepository;
        private readonly IItemRestauranteRepository _itemRestauranteRepository;
        private readonly IItemPedidoRepository _itemPedidoRepository;
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IPagamentoService _pagamentoService;

        public PedidoService(IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository,
            IRestauranteRepository restauranteRepository,
            IItemRestauranteRepository itemRestauranteRepository,
            IItemPedidoRepository itemPedidoRepository,
            IEntregadorRepository entregadorRepository,
            IPagamentoService pagamentoService)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _restauranteRepository = restauranteRepository;
            _itemRestauranteRepository = itemRestauranteRepository;
            _itemPedidoRepository = itemPedidoRepository;
            _entregadorRepository = entregadorRepository;
            _pagamentoService = pagamentoService;
        }

        public async Task<TaskResult<PedidoRequest>> ValidarPedido(PedidoRequest pedidoRequest)
        {
            // Validações
            if(pedidoRequest.Id > 0)
            {
                var pedido = await _pedidoRepository.ObterPorId(pedidoRequest.Id);
                if (pedido == null)
                    return TaskResult<PedidoRequest>.Fail("Pedido não encontrado.");
            }
            

            var cliente = await _clienteRepository.GetCliente(pedidoRequest.ClienteId);
            if (cliente == null)
                return TaskResult<PedidoRequest>.Fail("Cliente não encontrado.");

            var restaurante = await _restauranteRepository.GetRestaurante(pedidoRequest.RestauranteId);
            if (restaurante == null)
                return TaskResult<PedidoRequest>.Fail("Restaurante não encontrado.");

            var itensRestaurante = await _itemRestauranteRepository
                .GetItemRestaurante(pedidoRequest.RestauranteId);

            foreach (var item in pedidoRequest.Itens)
            {
                var itemRestaurante = itensRestaurante
                    .FirstOrDefault(i => i.Id == item.ItemRestauranteId);

                if (itemRestaurante == null)
                    return TaskResult<PedidoRequest>.Fail($"Item {item.Nome} não encontrado no restaurante.");

                if (itemRestaurante.QuantidadeEstoque == 0)
                    return TaskResult<PedidoRequest>.Fail($"Quantidade insuficiente para o item {item.Nome}.");

                if (itemRestaurante.Preco != item.Preco)
                    return TaskResult<PedidoRequest>.Fail($"Preço do item {item.Nome} está desatualizado.");
            }

            return TaskResult<PedidoRequest>.Ok(pedidoRequest);
        }

        public async Task<TaskResult<PedidoResponse>> AdicionarPedido(PedidoRequest pedidoRequest)
        {
            var itensRestaurante = await _itemRestauranteRepository
                .GetItemRestaurante(pedidoRequest.RestauranteId);

            var result = await ValidarPedido(pedidoRequest);
            if (!result.Success)
                return TaskResult<PedidoResponse>.Fail(result.Message);

            // Criação do pedido
            Pedido pedido = new Pedido
            {
                ClienteId = pedidoRequest.ClienteId,
                RestauranteId = pedidoRequest.RestauranteId,
                Itens = pedidoRequest.Itens.Select(i =>
                {
                    var itemRestaurante = itensRestaurante.First(ir => ir.Id == i.ItemRestauranteId);

                    return new ItemPedido
                    {
                        Quantidade = i.Quantidade,
                        Preco = itemRestaurante.Preco,
                        ItemRestauranteId = i.ItemRestauranteId
                    };
                }).ToList(),
                Status = StatusPedido.PagamentoPendente,
                DataCriacao = DateTime.UtcNow,
                ValorTotal = pedidoRequest.Itens.Sum(i =>
                {
                    var itemRestaurante = itensRestaurante.First(ir => ir.Id == i.ItemRestauranteId);
                    return itemRestaurante.Preco * i.Quantidade;
                })
            };

            // Cria o pedido e retorna resposta
            try
            {
                await _pedidoRepository.Adicionar(pedido);
                //TESTE
                var preferenceId = await _pagamentoService.CriarPreferenciaMercadoPago(pedido);

                return TaskResult<PedidoResponse>.Ok(new PedidoResponse
                {
                    Id = pedido.Id,
                    ClienteId = pedido.ClienteId,
                    RestauranteId = pedido.RestauranteId,
                    Itens = pedido.Itens.Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        PedidoId = i.PedidoId,
                        Quantidade = i.Quantidade,
                        Preco = i.Preco,
                        Nome = itensRestaurante.First(ir => ir.Id == i.ItemRestauranteId).Nome
                    }).ToList(),
                    Status = pedido.Status,
                    DataCriacao = pedido.DataCriacao,
                    PreferenceId = preferenceId,
                }, "Pedido criado com sucesso.");
            }
            catch
            {
                return TaskResult<PedidoResponse>.Fail("Erro ao criar o pedido.");
            }
        }

        public async Task<TaskResult<PedidoResponse>> AtualizarStatusPedido(int pedidoId, StatusPedido novoStatus)
        {
            try
            {
                await _pedidoRepository.AtualizarStatus(pedidoId, novoStatus);

                var pedidoResponse = ObterPedidoPorId(pedidoId).GetAwaiter().GetResult().Data;

                return TaskResult<PedidoResponse>.Ok(pedidoResponse, "Status do pedido atualizado com sucesso.");
            }
            catch
            {
                return TaskResult<PedidoResponse>.Fail("Erro ao atualizar o status do pedido.");
            }
        }

        public async Task<TaskResult<PedidoResponse>> EditarPedido(PedidoRequest pedidoRequest)
        {
            var pedidoExistente = await _pedidoRepository.ObterPorId(pedidoRequest.Id);
            if (pedidoExistente == null)
                return TaskResult<PedidoResponse>.Fail("Pedido não encontrado.");

            pedidoExistente.Itens = await _itemPedidoRepository.GetItensPedido(pedidoExistente.Id);

            try
            {
                Pedido pedido = new Pedido
                {
                    Id = pedidoExistente.Id,
                    DataCriacao = pedidoExistente.DataCriacao,
                    ClienteId = pedidoRequest.ClienteId == 0 ? pedidoExistente.ClienteId : pedidoRequest.ClienteId,
                    RestauranteId = pedidoRequest.RestauranteId == 0 ? pedidoExistente.RestauranteId : pedidoRequest.RestauranteId,
                    EntregadorId = pedidoRequest.EntregadorId > 0 ? pedidoRequest.EntregadorId : 0,
                    ValorTotal = pedidoExistente.ValorTotal,
                    Status = pedidoRequest.Status,
                    HoraEntrega = pedidoRequest.HoraEntrega.HasValue ? pedidoRequest.HoraEntrega : null,
                    HoraSaida = pedidoRequest.HoraSaida.HasValue ? pedidoRequest.HoraSaida : null,
                    Itens = pedidoExistente.Itens.Select(i =>
                    {
                        var itemRestaurante = pedidoExistente.Itens.First(ir => ir.Id == i.Id);
                        return new ItemPedido
                        {
                            Id = i.Id,
                            Quantidade = i.Quantidade,
                            Preco = itemRestaurante.Preco,
                            Nome = itemRestaurante.Nome,
                        };
                    }).ToList(),
                };

                await _pedidoRepository.EditarPedido(pedido);

                PedidoResponse response = new PedidoResponse
                {
                    Id = pedido.Id,
                    ClienteId = pedido.ClienteId,
                    RestauranteId = pedido.RestauranteId,
                    EntregadorId = pedido.EntregadorId > 0 ? pedido.EntregadorId : 0,
                    HoraEntrega = pedido.HoraEntrega.HasValue ? pedido.HoraEntrega : null,
                    HoraSaida = pedido.HoraSaida.HasValue ? pedido.HoraSaida : null,
                    Itens = pedido.Itens.Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        Quantidade = i.Quantidade,
                        Preco = i.Preco
                    }).ToList(),
                    Status = pedidoExistente.Status,
                    DataCriacao = pedidoExistente.DataCriacao
                };

                return TaskResult<PedidoResponse>.Ok(response, "Pedido editado com sucesso.");
            }
            catch
            {
                return TaskResult<PedidoResponse>.Fail("Erro ao editar o pedido.");
            }
        }

        public async Task<TaskResult<PedidoResponse>> ObterPedidoPorId(int id)
        {
            try
            {
                var pedido = await _pedidoRepository.ObterPorId(id);
                if (pedido == null)
                    return TaskResult<PedidoResponse>.Fail("Pedido não encontrado.");

                var itensPedido = await _itemPedidoRepository.GetItensPedido(pedido.Id);
                var itensRestaurante = await _itemRestauranteRepository
                    .GetItensRestauranteByIds(itensPedido.Select(i => i.ItemRestauranteId).ToList());
                var cliente = await _clienteRepository.GetCliente(pedido.ClienteId);
                var restaurante = await _restauranteRepository.GetRestaurante(pedido.RestauranteId);
                var entregador = new Entregador();
                if(pedido.EntregadorId > 0)
                    entregador = await _entregadorRepository.GetEntregadorById(pedido.EntregadorId);

                return TaskResult<PedidoResponse>.Ok(new PedidoResponse
                {
                    Id = pedido.Id,
                    ClienteId = pedido.ClienteId,
                    Cliente = new ClienteResponse
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome,
                        Email = cliente.Email,
                        Endereco = cliente.Endereco
                    },
                    RestauranteId = pedido.RestauranteId,
                    Restaurante = new RestauranteResponse
                    {
                        Id = restaurante.Id,
                        Nome = restaurante.Nome,
                        Endereco = restaurante.Endereco,
                        Email = restaurante.Email
                    },
                    EntregadorId = pedido.EntregadorId,
                    Entregador = pedido.EntregadorId > 0 ? new EntregadorResponse
                    {
                        Id = entregador.Id,
                        Nome = entregador.Nome,
                        Email = entregador.Email,
                        Disponivel = entregador.Disponivel
                    } : null,
                    HoraEntrega = pedido.HoraEntrega,
                    HoraSaida = pedido.HoraSaida,
                    Itens = itensPedido.Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        PedidoId = i.PedidoId,
                        Quantidade = i.Quantidade,
                        Preco = i.Preco,
                        Nome = itensRestaurante.First(ir => ir.Id == i.ItemRestauranteId).Nome,
                        ItemRestauranteId = i.ItemRestauranteId
                    }).ToList(),
                    Status = pedido.Status,
                    DataCriacao = pedido.DataCriacao
                }, "Pedido obtido com sucesso.");
            }
            catch
            {
                return TaskResult<PedidoResponse>.Fail("Erro ao obter o pedido.");
            }
        }

        public async Task<TaskResult<List<PedidoResponse>>> GetPedidosPorRestaurante(int restauranteId, StatusPedido status = default)
        {
            try
            {
                var pedidos = await _pedidoRepository.ObterPorRestauranteStatus(restauranteId, status);
                if(pedidos.Count > 0)
                {
                    foreach (var pedido in pedidos)
                    {
                        var itensPedido = await _itemPedidoRepository.GetItensPedido(pedido.Id);
                        var itensRestaurante = await _itemRestauranteRepository
                            .GetItensRestauranteByIds(itensPedido.Select(i => i.ItemRestauranteId).ToList());
                        var cliente = await _clienteRepository.GetCliente(pedido.ClienteId);
                        pedido.Itens = itensPedido.Select(i =>
                        {
                            var itemRestaurante = itensRestaurante.First(ir => ir.Id == i.ItemRestauranteId);
                            return new ItemPedido
                            {
                                Id = i.Id,
                                PedidoId = i.PedidoId,
                                Quantidade = i.Quantidade,
                                Preco = i.Preco,
                                Nome = itemRestaurante.Nome
                            };
                        }).ToList();
                    }
                }

                var response = pedidos.Select(p => new PedidoResponse
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    RestauranteId = p.RestauranteId,
                    EntregadorId = p.EntregadorId,
                    HoraEntrega = p.HoraEntrega,
                    HoraSaida = p.HoraSaida,
                    Itens = p.Itens.Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        Quantidade = i.Quantidade,
                        Preco = i.Preco
                    }).ToList(),
                    Status = p.Status,
                    DataCriacao = p.DataCriacao
                }).ToList();
                return TaskResult<List<PedidoResponse>>.Ok(response, "Pedidos obtidos com sucesso.");
            }
            catch
            {
                return TaskResult<List<PedidoResponse>>.Fail("Erro ao obter os pedidos.");
            }
        }

        public async Task<TaskResult<List<PedidoResponse>>> GetPedidosPorCliente(int clienteId, StatusPedido status = default)
        {
            try
            {
                var restaurantes = new List<Restaurante>();
                var itensPedido = new List<ItemPedido>();
                var pedidos = await _pedidoRepository.ObterPorClienteStatus(clienteId, status);
                foreach(var pedido in pedidos)
                {
                    var restaurante = await _restauranteRepository.GetRestaurante(pedido.RestauranteId);
                    if(restaurante == null)
                        restaurantes.Add(new Restaurante
                        {
                            Id = 0,
                            Nome = "Restaurante não encontrado",
                            Endereco = "",
                            Email = ""
                        });
                    else
                        restaurantes.Add(restaurante);

                    var itens = await _itemPedidoRepository.GetItensPedido(pedido.Id);
                    var itensRestaurante = await _itemRestauranteRepository
                        .GetItensRestauranteByIds(itens.Select(i => i.ItemRestauranteId).ToList());
                    itensPedido.AddRange(itens.Select(i =>
                    {
                        var itemRestaurante = itensRestaurante.First(ir => ir.Id == i.ItemRestauranteId);
                        return itemRestaurante != null ? new ItemPedido
                        {
                            Id = i.Id,
                            PedidoId = i.PedidoId,
                            Quantidade = i.Quantidade,
                            Preco = i.Preco,
                            Nome = itemRestaurante.Nome
                        } : new ItemPedido
                        {
                            Id = i.Id,
                            PedidoId = i.PedidoId,
                            Quantidade = i.Quantidade,
                            Preco = i.Preco,
                            Nome = "Item não encontrado"
                        };
                    }).ToList());
                }

                var response = pedidos.Select(p => new PedidoResponse
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    RestauranteId = p.RestauranteId,
                    EntregadorId = p.EntregadorId,
                    HoraEntrega = p.HoraEntrega,
                    HoraSaida = p.HoraSaida,
                    Itens = itensPedido.Where(i => i.PedidoId == p.Id).Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        Quantidade = i.Quantidade,
                        Preco = i.Preco,
                        Nome = i.Nome
                    }).ToList(),
                    Restaurante = restaurantes.Where(r => r.Id == p.RestauranteId).Select(r => new RestauranteResponse
                    {
                        Id = r.Id,
                        Nome = r.Nome,
                        Endereco = r.Endereco,
                        Email = r.Email
                    }).FirstOrDefault()!,
                    Status = p.Status,
                    DataCriacao = p.DataCriacao
                }).ToList();
                return TaskResult<List<PedidoResponse>>.Ok(response, "Pedidos obtidos com sucesso.");
            }
            catch
            {
                return TaskResult<List<PedidoResponse>>.Fail("Erro ao obter os pedidos.");
            }
        }

        public async Task<TaskResult<string>> SaveChangesPedido()
        {
            try
            {
                await _pedidoRepository.SaveChangesPedido();

                return TaskResult<string>.Ok("Pedido Salvo!");
            }
            catch
            {
                return TaskResult<string>.Fail("Houve um erro ao salvo o pedido");
            }
        }
    }
}

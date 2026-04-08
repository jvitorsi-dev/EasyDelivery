using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(EasyDeliveryContext context)
    {       
        // ========================
        // USUÁRIOS
        // ========================
        var usuarioCliente = new Usuario
        {
            Email = "cliente@teste.com",
            SenhaHash = "123",
            Role = UserRole.Cliente
        };

        var usuarioRestaurante = new Usuario
        {
            Email = "restaurante@teste.com",
            SenhaHash = "123",
            Role = UserRole.Restaurante
        };

        var usuarioEntregador = new Usuario
        {
            Email = "entregador@teste.com",
            SenhaHash = "123",
            Role = UserRole.Entregador
        };

        context.Usuarios.AddRange(usuarioCliente, usuarioRestaurante, usuarioEntregador);
        await context.SaveChangesAsync();

        // ========================
        // CLIENTE
        // ========================
        var cliente = new Cliente
        {
            Nome = "João Cliente",
            Email = usuarioCliente.Email,
            Endereco = "Rua A, 123",
            UsuarioId = usuarioCliente.Id
        };

        // ========================
        // RESTAURANTE
        // ========================
        var restaurante = new Restaurante
        {
            Nome = "Burger Top",
            Email = usuarioRestaurante.Email,
            Endereco = "Rua B, 456",
            UsuarioId = usuarioRestaurante.Id
        };

        // ========================
        // ENTREGADOR
        // ========================
        var entregador = new Entregador
        {
            Nome = "Carlos Motoboy",
            Email = usuarioEntregador.Email,
            UsuarioId = usuarioEntregador.Id
        };

        context.Clientes.Add(cliente);
        context.Restaurantes.Add(restaurante);
        context.Entregadores.Add(entregador);

        await context.SaveChangesAsync();

        // ========================
        // ITENS DO RESTAURANTE
        // ========================
        var itens = new List<ItemRestaurante>
        {
            new ItemRestaurante { Nome = "Hamburguer", Preco = 25, RestauranteId = restaurante.Id },
            new ItemRestaurante { Nome = "Pizza", Preco = 40, RestauranteId = restaurante.Id },
            new ItemRestaurante { Nome = "Refrigerante", Preco = 8, RestauranteId = restaurante.Id }
        };

        context.ItensRestaurante.AddRange(itens);
        await context.SaveChangesAsync();

        // ========================
        // PEDIDO
        // ========================
        var pedido = new Pedido
        {
            ClienteId = cliente.Id,
            RestauranteId = restaurante.Id,
            EntregadorId = entregador.Id,
            Status = StatusPedido.PagamentoPendente,
            DataCriacao = DateTime.Now,
            ValorTotal = 73
        };

        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();

        // ========================
        // ITENS DO PEDIDO
        // ========================
        var itensPedido = new List<ItemPedido>
        {
            new ItemPedido
            {
                PedidoId = pedido.Id,
                ItemRestauranteId = itens[0].Id,
                Quantidade = 1,
                Preco = 25
            },
            new ItemPedido
            {
                PedidoId = pedido.Id,
                ItemRestauranteId = itens[1].Id,
                Quantidade = 1,
                Preco= 40
            },
            new ItemPedido
            {
                PedidoId = pedido.Id,
                ItemRestauranteId = itens[2].Id,
                Quantidade = 1,
                Preco = 8
            }
        };

        context.ItensPedido.AddRange(itensPedido);

        // ========================
        // PAGAMENTO
        // ========================
        var pagamento = new Pagamento
        {
            PedidoId = pedido.Id,
            Status = StatusPagamento.Pendente
        };

        context.Pagamentos.Add(pagamento);

        await context.SaveChangesAsync();
    }
}
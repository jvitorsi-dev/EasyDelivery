using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.Entregador;
using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace EasyDelivery.Application.Services
{
    public class ValidacaoService
    {
        public bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return regex.IsMatch(email);
        }
        public bool ValidarPedidoRequest(PedidoRequest pedido, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();

            if (pedido.ClienteId <= 0)
                mensagensErro.Add("ClienteId não pode estar vazio.");
            if (pedido.RestauranteId <= 0)
                mensagensErro.Add("RestauranteId não pode estar vazio.");
            if (pedido.Itens == null || pedido.Itens.Count == 0)
                mensagensErro.Add("O pedido deve conter pelo menos um item.");
            if(!Enum.IsDefined(typeof(StatusPedido), pedido.Status))
                mensagensErro.Add("Status do pedido inválido.");
            if(pedido.Status == StatusPedido.EmEntrega || pedido.Status == StatusPedido.Cancelado)
                mensagensErro.Add("O pedido em entrega ou cancelado não pode ser editado.");

            return mensagensErro.Count > 0;
        }

        public bool ValidarAtualizacaoPedidoRequest(AtualizarStatusPedidoRequest pedido, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();

            if (pedido.PedidoId <= 0)
                mensagensErro.Add("PedidoId não pode estar vazio.");
            if (pedido.Status == null)
                mensagensErro.Add("Status do pedido não pode estar vazio.");
            if (pedido.Status != null && !Enum.IsDefined(typeof(StatusPedido), pedido.Status))
                mensagensErro.Add("Status do pedido inválido.");
            if (pedido.EntregadorId != null && pedido.EntregadorId <= 0)
                mensagensErro.Add("EntregadorId deve ser maior que zero, se fornecido.");

            return mensagensErro.Count > 0;
        }

        public bool ValidarRestauranteRequest(RestauranteRequest restaurante, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();

            if (string.IsNullOrEmpty(restaurante.Nome))
                mensagensErro.Add("O nome do restaurante não pode estar vazio.");
            if (restaurante.Nome.Length > 150)
                mensagensErro.Add("O nome do restaurante não pode exceder 150 caracteres.");
            if (string.IsNullOrEmpty(restaurante.Endereco))
                mensagensErro.Add("O endereço do restaurante não pode estar vazio.");
            if (restaurante.Endereco.Length > 250)
                mensagensErro.Add("O endereço do restaurante não pode exceder 250 caracteres.");
            if (string.IsNullOrEmpty(restaurante.Email))
                mensagensErro.Add("O Email do restaurante não pode estar vazio.");
            if (restaurante.Email.Length > 150)
                mensagensErro.Add("O Email do restaurante não pode exceder 150 caracteres.");
            if (EmailValido(restaurante.Email) == false)
                mensagensErro.Add("O Email do restaurante é inválido.");

            return mensagensErro.Count > 0;
        }

        public bool ValidarPagamentoRequest(PagamentoRequest pagamento, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();

            if (pagamento.PedidoId <= 0)
                mensagensErro.Add("PedidoId não pode estar vazio.");
            if (string.IsNullOrEmpty(pagamento.Metodo))
                mensagensErro.Add("Método de pagamento não pode estar vazio.");

            return mensagensErro.Count > 0;
        }

        public bool ValidarClienteRequest(ClienteRequest cliente, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();
            if (string.IsNullOrEmpty(cliente.Nome))
                mensagensErro.Add("O nome do cliente não pode estar vazio.");
            if (cliente.Nome.Length > 150)
                mensagensErro.Add("O nome do cliente não pode exceder 150 caracteres.");
            if (string.IsNullOrEmpty(cliente.Endereco))
                mensagensErro.Add("O endereço do cliente não pode estar vazio.");
            if (cliente.Endereco.Length > 250)
                mensagensErro.Add("O endereço do cliente não pode exceder 250 caracteres.");
            if (string.IsNullOrEmpty(cliente.Email))
                mensagensErro.Add("O email do cliente não pode estar vazio.");
            if (cliente.Email.Length > 150)
                mensagensErro.Add("O email do cliente não pode exceder 150 caracteres.");
            if (EmailValido(cliente.Email) == false)
                mensagensErro.Add("O email do cliente é inválido.");
            return mensagensErro.Count > 0;
        }

        public bool ValidarEntregadorRequest(EntregadorRequest entregador, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();
            if (string.IsNullOrEmpty(entregador.Nome))
                mensagensErro.Add("O nome do entregador não pode estar vazio.");
            if (entregador.Nome.Length > 150)
                mensagensErro.Add("O nome do entregador não pode exceder 150 caracteres.");
            if (string.IsNullOrEmpty(entregador.Email))
                mensagensErro.Add("O Email do entregador não pode estar vazio.");
            if (entregador.Email.Length > 150)
                mensagensErro.Add("O Email do entregador não pode exceder 150 caracteres.");
            if (EmailValido(entregador.Email) == false)
                mensagensErro.Add("O Email do entregador é inválido.");
            return mensagensErro.Count > 0;
        }

        public bool ValidarRegisterRequest(RegisterRequest usuario, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();
            if (string.IsNullOrEmpty(usuario.Email))
                mensagensErro.Add("O email do usuário não pode estar vazio.");
            if (usuario.Email.Length > 150)
                mensagensErro.Add("O email do usuário não pode exceder 150 caracteres.");
            if (EmailValido(usuario.Email) == false)
                mensagensErro.Add("O email do usuário é inválido.");
            if (string.IsNullOrEmpty(usuario.Senha))
                mensagensErro.Add("A senha do usuário não pode estar vazia.");
            if (!Enum.IsDefined(typeof(UserRole), usuario.Role))
                mensagensErro.Add("O papel do usuário é inválido.");

            return mensagensErro.Count > 0;
        }

        public bool ValidarItensRestauranteRequest(List<ItemRestauranteRequest> itens, out List<string> mensagensErro)
        {
            mensagensErro = new List<string>();
            if (itens == null || itens.Count == 0)
                mensagensErro.Add("A lista de itens não pode estar vazia.");

            foreach (var item in itens)
            {
                if (item.IdRestaurante <= 0)
                    mensagensErro.Add("Id do restaurante deve ser maior que zero.");
                if (string.IsNullOrEmpty(item.Nome))
                    mensagensErro.Add("O nome do item não pode estar vazio.");
                if (item.Nome.Length > 150)
                    mensagensErro.Add("O nome do item não pode exceder 150 caracteres.");
                if (item.Quantidade <= 0)
                    mensagensErro.Add("A quantidade do item deve ser maior do que 0.");
                if (item.Preco <= 0)
                    mensagensErro.Add("O preço do item eve ser maior do que 0.");
            }
            return mensagensErro.Count > 0;
        }
    }
}

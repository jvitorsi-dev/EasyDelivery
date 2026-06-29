using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Cliente
{
    public class ClienteRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

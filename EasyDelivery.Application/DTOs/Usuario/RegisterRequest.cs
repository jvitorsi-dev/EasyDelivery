using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Usuario
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string? Endereco { get; set; } 
        public string Nome { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public UserRole Role { get; set; }
    }
}

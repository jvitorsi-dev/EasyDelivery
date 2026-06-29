using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Endereco { get; set; }
        public UserRole Role { get; set; }
        public int RoleId { get; set; }
    }
}

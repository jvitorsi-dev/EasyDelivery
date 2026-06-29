using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Usuario
{
    public class LoginResponse
    {
        public UsuarioResponse Usuario { get; set; } = default!;
        public string Token { get; set; } = string.Empty;
    }
}

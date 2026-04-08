using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Usuario
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}

using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<TaskResult<UsuarioResponse>> Login(LoginRequest login);
    }
}

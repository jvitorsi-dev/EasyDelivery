using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Interfaces
{
    public interface IUsuarioService
    {
        public Task<TaskResult<UsuarioResponse>> GetUsuarioById(int id);
        public Task<TaskResult<UsuarioResponse>> GetUsuarioByEmail(string email);
        public Task<TaskResult<string>> CreateUsuario(RegisterRequest usuario);
    }
}

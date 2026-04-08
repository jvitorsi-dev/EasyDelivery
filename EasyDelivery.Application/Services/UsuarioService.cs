using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IRestauranteRepository _restauranteRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository, 
            IEntregadorRepository entregadorRepository, 
            IRestauranteRepository restauranteRepository)
        {
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _entregadorRepository = entregadorRepository;
            _restauranteRepository = restauranteRepository;
        }

        public async Task<TaskResult<UsuarioResponse>> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetById(id);
                if (usuario == null)
                    return TaskResult<UsuarioResponse>.Fail("Usuário não encontrado.");

                switch(usuario.Role)
                {
                    case UserRole.Cliente:
                        var cliente = await _clienteRepository.GetCliente(usuario.Id);
                        if (cliente != null)
                            usuario.Nome = cliente.Nome;
                        break;
                    case UserRole.Entregador:
                        var entregador = await _entregadorRepository.GetEntregadorById(usuario.Id);
                        if (entregador != null)
                            usuario.Nome = entregador.Nome;
                        break;
                    case UserRole.Restaurante:
                        var restaurante = await _restauranteRepository.GetRestaurante(usuario.Id);
                        if (restaurante != null)
                            usuario.Nome = restaurante.Nome;
                        break;
                }

                var response = new UsuarioResponse
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                };
                return TaskResult<UsuarioResponse>.Ok(response, "Usuário obtido com sucesso!");
            }
            catch
            {
                return TaskResult<UsuarioResponse>.Fail("Erro ao buscar usuário.");
            }

        }

        public async Task<TaskResult<UsuarioResponse>> GetUsuarioByEmail(string email)
        {
            var usuario = await _usuarioRepository.GetByEmail(email);
            if (usuario == null)
                return TaskResult<UsuarioResponse>.Fail("Usuário não encontrado.");
            var response = new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
            return TaskResult<UsuarioResponse>.Ok(response, "Usuário obtido com sucesso!");
        }

        public async Task<TaskResult<UsuarioResponse>> CreateUsuario(RegisterRequest usuario)
        {
            var existingUsuario = await _usuarioRepository.GetByEmail(usuario.Email);
            if (existingUsuario != null)
                return TaskResult<UsuarioResponse>.Fail("Email já cadastrado.");

            var newUsuario = new Usuario
            {
                Email = usuario.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha),
                Role = usuario.Role
            };

            try
            {
                newUsuario = await _usuarioRepository.Add(newUsuario);
                usuario.UsuarioId = newUsuario.Id;
                var result = await AddByRole(usuario);

                if (result == null)
                    return TaskResult<UsuarioResponse>.Fail("Erro ao associar usuário ao perfil.");

                var response = new UsuarioResponse
                {
                    Id = newUsuario.Id,
                    Nome = newUsuario.Nome,
                    Email = newUsuario.Email,
                    Endereco = usuario.Endereco ?? "",
                    Role = newUsuario.Role,
                    RoleId = result switch
                    {
                        Cliente c => c.Id,
                        Entregador e => e.Id,
                        Restaurante r => r.Id,
                        _ => 0
                    }
                };

                return TaskResult<UsuarioResponse>.Ok(response, "Usuário criado com sucesso!");
            }
            catch
            {
                return TaskResult<UsuarioResponse>.Fail("Erro ao criar usuário.");
            }
        }

        public async Task<object?> AddByRole(RegisterRequest request)
        {
            switch(request.Role)
            {
                case UserRole.Cliente:
                    var cliente = new Cliente
                    {
                        Email = request.Email,
                        Nome = request.Nome,
                        Endereco = request.Endereco!,
                        UsuarioId = request.UsuarioId!.Value
                    };
                    await _clienteRepository.AdicionarCliente(cliente);
                    return cliente;
                case UserRole.Entregador:
                    var entregador = new Entregador
                    {
                        Email = request.Email,
                        Nome = request.Nome,
                        UsuarioId = request.UsuarioId!.Value,
                    };
                    await _entregadorRepository.AddEntregador(entregador);
                    return entregador;
                case UserRole.Restaurante:
                    var restaurante = new Restaurante
                    {
                        Email = request.Email,
                        Nome = request.Nome,
                        Endereco = request.Endereco!,
                        UsuarioId = request.UsuarioId!.Value
                    };
                    await _restauranteRepository.AddRestaurante(restaurante);
                    return restaurante;
            }

            return null;
        }
    }
}

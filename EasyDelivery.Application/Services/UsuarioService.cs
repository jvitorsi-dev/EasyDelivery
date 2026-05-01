using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.Entregador;
using EasyDelivery.Application.DTOs.Restaurante;
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
                var response = new UsuarioResponse();
                var usuario = await _usuarioRepository.GetById(id);
                if (usuario == null)
                    return TaskResult<UsuarioResponse>.Fail("Usuário não encontrado.");

                if (usuario.Role == UserRole.Cliente)
                {
                    var cliente = await _clienteRepository.GetClienteByUsuarioId(usuario.Id);
                    if (cliente == null)
                        return TaskResult<UsuarioResponse>.Fail("Nenhum cadastro encontrado!");
                    response = new UsuarioResponse
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome,
                        Endereco = cliente.Endereco,
                        Email = cliente.Email,
                        Role = UserRole.Cliente
                    };
                }

                if (usuario.Role == UserRole.Restaurante)
                {
                    var restaurante = await _restauranteRepository.GetRestauranteByUserId(usuario.Id);
                    if (restaurante == null)
                        return TaskResult<UsuarioResponse>.Fail("Nenhum cadastro encontrado!");
                    response = new UsuarioResponse
                    {
                        Id = restaurante.Id,
                        Nome = restaurante.Nome,
                        Endereco = restaurante.Endereco,
                        Email = restaurante.Email,
                        Role = UserRole.Restaurante
                    };
                }

                if (usuario.Role == UserRole.Entregador)
                {
                    var entregador = await _entregadorRepository.GetEntregadorByUsuarioId(usuario.Id);
                    if (entregador == null)
                        return TaskResult<UsuarioResponse>.Fail("Nenhum cadastro encontrado!");
                    response = new UsuarioResponse
                    {
                        Id = entregador.Id,
                        Nome = entregador.Nome,
                        Email = entregador.Email,
                        Role = UserRole.Entregador
                    };
                }

                return TaskResult<UsuarioResponse>.Ok(response, "Usuário obtido com sucesso!");
            }
            catch
            {
                return TaskResult<UsuarioResponse>.Fail("Erro ao buscar usuário.");
            }

        }

        public async Task<TaskResult<UsuarioResponse>> GetUsuarioByEmail(string email)
        {
            var response = new UsuarioResponse();

            var usuario = await _usuarioRepository.GetByEmail(email);
            if (usuario == null)
                return TaskResult<UsuarioResponse>.Fail("Usuário não encontrado.");

            if(usuario.Role == UserRole.Cliente)
            {
                var cliente = await _clienteRepository.GetClienteByUsuarioId(usuario.Id);
                if (cliente == null)
                    return TaskResult<UsuarioResponse>.Fail("Nenhum cadastro encontrado!");
                response = new UsuarioResponse
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Endereco = cliente.Endereco,    
                    Email = cliente.Email,
                    Role = UserRole.Cliente
                };
            }

            if(usuario.Role == UserRole.Restaurante)
            {
                var restaurante = await _restauranteRepository.GetRestauranteByUserId(usuario.Id);
                if(restaurante == null)
                    return TaskResult<UsuarioResponse>.Fail("Nenhum cadastro encontrado!");
                response = new UsuarioResponse
                {
                    Id = restaurante.Id,
                    Nome = restaurante.Nome,
                    Endereco = restaurante.Endereco,
                    Email = restaurante.Email,
                    Role = UserRole.Restaurante
                };
            }

            if(usuario.Role == UserRole.Entregador)
            {
                var entregador = await _entregadorRepository.GetEntregadorByUsuarioId(usuario.Id);
                if(entregador == null)
                    return TaskResult<UsuarioResponse>.Fail("Nenhum cadastro encontrado!");
                response = new UsuarioResponse
                {
                    Id = entregador.Id,
                    Nome = entregador.Nome,
                    Email = entregador.Email,
                    Role = UserRole.Entregador
                };
            }

            return TaskResult<UsuarioResponse>.Ok(response, "Usuário obtido com sucesso!");
        }

        public async Task<TaskResult<string>> CreateUsuario(RegisterRequest usuario)
        {
            var existingUsuario = await _usuarioRepository.GetByEmail(usuario.Email);
            if (existingUsuario != null)
                return TaskResult<string>.Fail("Email já cadastrado.");

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
                await AddByRole(usuario);

                return TaskResult<string>.Ok("Usuário criado com sucesso!");
            }
            catch
            {
                return TaskResult<string>.Fail("Erro ao criar usuário.");
            }
        }

        public async Task AddByRole(RegisterRequest request)
        {
            try
            {
                switch (request.Role)
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
                        break;
                    case UserRole.Entregador:
                        var entregador = new Entregador
                        {
                            Email = request.Email,
                            Nome = request.Nome,
                            UsuarioId = request.UsuarioId!.Value
                        };
                        await _entregadorRepository.AddEntregador(entregador);
                        break;
                    case UserRole.Restaurante:
                        var restaurante = new Restaurante
                        {
                            Email = request.Email,
                            Nome = request.Nome,
                            Endereco = request.Endereco!,
                            UsuarioId = request.UsuarioId!.Value,
                            CategoriaId = request.CategoriaId!.Value
                        };
                        await _restauranteRepository.AddRestaurante(restaurante);
                        break;
                }
            }
            catch (Exception ex) 
            {
                return;
            }
            
        }
    }
}

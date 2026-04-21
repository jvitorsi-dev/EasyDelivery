using EasyDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Infrastructure.Context
{
    public class EasyDeliveryContext : DbContext
    {
        public EasyDeliveryContext(DbContextOptions<EasyDeliveryContext> options)
            : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<ItemRestaurante> ItensRestaurante { get; set; }
        public DbSet<Entrega> Entregas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Restaurante> Restaurantes { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CategoriaItemRestaurante> CategoriasRestaurantes { get; set; }
        public DbSet<CategoriaItensRestaurante> CategoriasItensRestaurante { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========================
            // PEDIDO
            // ========================
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Status)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(p => p.ValorTotal)
                      .HasColumnType("decimal(10,2)");

                entity.Property(p => p.DataCriacao)
                      .IsRequired();

                // Pedido → Cliente (N:1)
                entity.HasOne<Cliente>()
                      .WithMany()
                      .HasForeignKey(p => p.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Pedido → Restaurante (N:1)
                entity.HasOne<Restaurante>()
                      .WithMany()
                      .HasForeignKey(p => p.RestauranteId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Pedido → Entregador (N:1 opcional)
                entity.HasOne<Entregador>()
                      .WithMany()
                      .HasForeignKey(p => p.EntregadorId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Pedido → Itens (1:N)
                entity.HasMany(p => p.Itens)
                      .WithOne()
                      .HasForeignKey(i => i.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================
            // PAGAMENTO
            // ========================
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Status)
                      .HasConversion<string>()
                      .IsRequired();

                // 1:1 com Pedido
                entity.HasOne<Pedido>()
                      .WithOne()
                      .HasForeignKey<Pagamento>(p => p.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================
            // CLIENTE
            // ========================
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Nome)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(c => c.Endereco)
                      .IsRequired()
                      .HasMaxLength(250);

                entity.Property(c => c.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.HasOne(c => c.Usuario)
                    .WithOne(u => u.Cliente)
                    .HasForeignKey<Cliente>(c => c.UsuarioId);
            });

            // ========================
            // RESTAURANTE
            // ========================
            modelBuilder.Entity<Restaurante>(entity =>
            {
                entity.HasOne(r => r.Usuario)
                .WithOne(u => u.Restaurante)
                .HasForeignKey<Restaurante>(r => r.UsuarioId);

                entity.HasKey(r => r.Id);

                entity.Property(r => r.Nome)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(r => r.Endereco)
                      .IsRequired()
                      .HasMaxLength(250);

                entity.Property(r => r.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.HasOne(r => r.Categoria)
                    .WithMany(c => c.Restaurantes)
                    .HasForeignKey(r => r.CategoriaId);

                // Restaurante → Itens (1:N)
                entity.HasMany(p => p.Itens)
                      .WithOne()
                      .HasForeignKey(i => i.RestauranteId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CategoriaItemRestaurante>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Nome)
                    .HasMaxLength(150)
                    .IsRequired();
            });


            // ========================
            // ENTREGADOR
            // ========================
            modelBuilder.Entity<Entregador>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.HasOne(e => e.Usuario)
                    .WithOne(u => u.Entregador)
                    .HasForeignKey<Entregador>(e => e.UsuarioId);
            });

            // ========================
            //// USUÁRIO
            /// ========================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique(); 

                entity.Property(u => u.SenhaHash)
                    .IsRequired();

                // Enum como string (RECOMENDADO)
                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .IsRequired();

            });

            // ==============================
            //ITENSRESTAURANTE
            //===============================
            modelBuilder.Entity<ItemRestaurante>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.RestauranteId)
                    .IsRequired();

                entity.Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(i => i.QuantidadeEstoque)
                    .IsRequired();

                entity.Property(p => p.Preco)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(r => r.CategoriaItensRestaurante)
                    .WithMany(c => c.ItemRestaurante)
                    .HasForeignKey(r => r.CategoriaId);
            });
        }
    }
}

using EasyDelivery.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyDelivery.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        [NotMapped]
        public int UserRoleId { get; set; }
        public UserRole Role { get; set; }
        [NotMapped]
        public string Nome { get; set; } = string.Empty; 

        public Restaurante Restaurante { get; set; } = default!;
        public Cliente Cliente { get; set; } = default!;
        public Entregador Entregador { get; set; } = default!;

    }
}

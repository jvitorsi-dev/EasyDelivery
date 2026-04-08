
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyDelivery.Infrastructure.Context
{
    public class EasyDeliveryContextFactory
        : IDesignTimeDbContextFactory<EasyDeliveryContext>
    {
        public EasyDeliveryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EasyDeliveryContext>();

            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EasyDeliveryDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");

            return new EasyDeliveryContext(optionsBuilder.Options);
        }
    }
}

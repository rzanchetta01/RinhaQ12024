using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Api
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public Client? GetClientStatementQuery(int id) => getClientStatementQuery(this, id);

        private static readonly Func<AppDbContext, int, Client?> getClientStatementQuery =
            EF.CompileQuery((AppDbContext context, int clientId) =>
                    context.Clients
                    .AsNoTrackingWithIdentityResolution()
                    .Where(e => e.Id == clientId)
                    .Include(e => e.Transactions.OrderByDescending(e => e.Realizada_em).Take(10))
                    .AsSplitQuery()
                    .FirstOrDefault()
                    );

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var client = modelBuilder.Entity<Client>();
            client.HasKey(x => x.Id);
            client.Property(x => x.Id).ValueGeneratedOnAdd();
            client.HasMany(c => c.Transactions)
                .WithOne(t => t.Cliente)
                .HasForeignKey(t => t.IdCliente);

            var transaction = modelBuilder.Entity<Transaction>();
            transaction.HasKey(x => x.Id);
            transaction.Property(x => x.Id).ValueGeneratedOnAdd();
            transaction.Property(p => p.Tipo);
        }
    }
}

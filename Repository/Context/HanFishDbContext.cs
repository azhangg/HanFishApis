using Entities.Account;
using Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Base;
using System.Reflection;
using Utils.Enums;

namespace Repositories.Context
{
    public class HanFishDbContext : DbContext, IUnitOfWork
    {
        public HanFishDbContext(DbContextOptions<HanFishDbContext> options) : base(options) { }

        private IDbContextTransaction Transaction { get; set; }

        public async Task BeginTransactionAsync()
        {
            if (Transaction is null)
            {
                Transaction = await Database.BeginTransactionAsync();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = typeof(BaseEntity).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseEntity)));
            foreach (var entity in entityTypes)
                if (modelBuilder.Model.FindEntityType(entity) is null)
                    modelBuilder.Model.AddEntityType(entity);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(BaseEntity)) ??  GetType().Assembly);

            List<Role> roles = new List<Role>();

            var roleTypes = Enum.GetValues(typeof(RoleTypes));

            foreach (RoleTypes roleType in roleTypes)
                roles.Add(new Role { Id = (int)roleType, Name = roleType.ToString() });

            modelBuilder.Entity<Role>().HasData(roles);
        }

        public bool Commit()
        {
            if (Transaction is null)
            {
                return false;
            }
            Transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }

        public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

using FaultTracking.Data.Dal;
using FaultTracking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FaultTracking.Data.Context
{
    public class DatabaseContext :DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Forms> Form { get; set; }
        public DbSet<FormStatuss> FormStatus { get; set; }
        public DbSet<FaultRoles> FaultRole { get; set; }
        public DbSet<AuthorizedPersons> AuthorizedPerson { get; set; }
        public DbSet<Ubyss> Ubys { get; set; }
        public DbSet<FaultTypes> FaultType { get; set; }
        public DbSet<FormStatusViews> FormStatusView { get; set; }
        public DbSet<RoleViews> RoleView { get; set; }
        public DbSet<Colours> Colour { get; set; }

        public DbSet<AuthorizedUbyss> AuthorizedUbys { get; set; }
        public DbSet<Logs> Log { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormStatusViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("FormStatusView");
            });

            modelBuilder.Entity<RoleViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(nameof(RoleView));
            });
        }
    }
}

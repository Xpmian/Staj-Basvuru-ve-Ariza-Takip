using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aibu.InternshipAutomation.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) 
        {

        }

        public DbSet<Students> Student { get; set; }
        public DbSet<Userss> Users { get; set; }
        public DbSet<Companies> Company { get; set; }
        public DbSet<Departments> Department { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<Cities> City { get; set; }
        public DbSet<States> State { get; set; }
        public DbSet<InternPeriods> InternPeriod { get; set; }
        public DbSet<Logs> Log { get; set; }
        public DbSet<AcceptanceStatuss> AcceptanceStatus { get; set; }
        public DbSet<AuthorizedPersons> AuthorizedPerson { get; set; }
        public DbSet<PastInternShipViews> PastInternShipView { get; set; }
        public DbSet<ApplicantStudentsViews> ApplicantStudentsView { get; set; }
        public DbSet<RoleViews> RoleView { get; set; }
        public DbSet<AuthorizedViews> AuthorizedView { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PastInternShipViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(nameof(PastInternShipView));
            });

            modelBuilder.Entity<ApplicantStudentsViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(nameof(ApplicantStudentsView));
            });

            modelBuilder.Entity<RoleViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(nameof(RoleView));
            });

            modelBuilder.Entity<AuthorizedViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(nameof(AuthorizedView));
            });
        }

    }
}

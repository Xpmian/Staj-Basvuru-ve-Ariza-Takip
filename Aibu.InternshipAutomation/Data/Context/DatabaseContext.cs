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
        //public DbSet<Companies> Company { get; set; }
        public DbSet<CompanyUserss> CompanyUsers { get; set; }
        public DbSet<Departments> Department { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<Cities> City { get; set; }
        public DbSet<States> State { get; set; }
        public DbSet<InternPeriods> InternPeriod { get; set; }
        public DbSet<InternTypes> InternTypes { get; set; }
        public DbSet<Logs> Log { get; set; }
        public DbSet<AcceptanceStatuss> AcceptanceStatus { get; set; }
        public DbSet<AuthorizedPersons> AuthorizedPerson { get; set; }
        public DbSet<Ubyss> Ubys { get; set; }
        public DbSet<PastInternShipViews> PastInternShipView { get; set; }
        public DbSet<ApplicantStudentsViews> ApplicantStudentsView { get; set; }
        public DbSet<RoleViews> RoleView { get; set; }
        public DbSet<AuthorizedViews> AuthorizedView { get; set; }
        public DbSet<StateViews> StateView { get; set; }
        public DbSet<CompanyInfoViews> CompanyInfoView { get; set; }

        public DbSet<AuthorizedUbyss> AuthorizedUbys { get; set; }
        public DbSet<AuthorizedDepartments> AuthorizedDepartments { get; set; }
        public DbSet<AuthorizedPersonDepartmentViews> AuthorizedPersonDepartmentView { get; set; }

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
            modelBuilder.Entity<StateViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("StateView");
            });
            modelBuilder.Entity<CompanyInfoViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("CompanyInfoView");
            });
            modelBuilder.Entity<AuthorizedPersonDepartmentViews>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("AuthorizedPersonDepartmentView");
            });

            modelBuilder.Entity<AuthorizedDepartments>()
       .HasKey(ad => new { ad.AuthorizedPersonId, ad.DepartmentId });

            modelBuilder.Entity<AuthorizedDepartments>()
                .HasOne(ad => ad.AuthorizedPerson)
                .WithMany(ap => ap.AuthorizedDepartments)
                .HasForeignKey(ad => ad.AuthorizedPersonId);

            modelBuilder.Entity<AuthorizedDepartments>()
                .HasOne(ad => ad.Department)
                .WithMany(d => d.AuthorizedDepartments)
                .HasForeignKey(ad => ad.DepartmentId);

        }

    }
}

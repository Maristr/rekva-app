using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Test02.Models;

namespace Test02.Data;

public class Test02Context : IdentityDbContext<IdentityUser>
{
    public Test02Context(DbContextOptions<Test02Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Nový kod jen pro specificke akce
        //builder.Entity<Zakaznik>()
        //    .HasOne(z => z.AspNetUser)
        //    .WithMany()
        //    .HasForeignKey(z => z.AspNetUserId);

        //builder.Entity<Pojisteni>()
        //    .HasOne(p => p.Zakaznik)
        //    .WithMany()
        //    .HasForeignKey(p => p.ZakaznikId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //builder.Entity<Adresa>()
        //    .HasOne(p => p.Zakaznik)
        //    .WithMany()
        //    .HasForeignKey(p => p.ZakaznikId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //builder.Entity<PojistnaUdalost>()
        //    .HasOne(p => p.Zakaznik)
        //    .WithMany()
        //    .HasForeignKey(p => p.ZakaznikId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //builder.Entity<PojistnaUdalost>()
        //    .HasOne(p => p.Pojisteni)
        //    .WithMany()
        //    .HasForeignKey(p => p.PojisteniId)
        //    .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Test02.Models.Zakaznik> Zakaznik { get; set; } = default!;

    public DbSet<Test02.Models.Adresa> Adresa { get; set; } = default!;

    public DbSet<Test02.Models.Pojisteni> Pojisteni { get; set; } = default!;

    public DbSet<Test02.Models.PojistnaUdalost> PojistnaUdalost { get; set; } = default!;
}

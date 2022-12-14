using EliApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EliApp.Models;
using System.Reflection.Emit;

namespace EliApp.Data;

public class EliAppContext : IdentityDbContext<EliAppUser>
{
    public EliAppContext(DbContextOptions<EliAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<EliApp.Models.AccountModel> AccountModel { get; set; }

    public DbSet<EliApp.Models.EntryModel> EntryModel { get; set; }
    public DbSet<EliApp.Models.LedgerModel> LedgerModel { get; set; }
}

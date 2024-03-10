using Microsoft.EntityFrameworkCore;
using PatientTest.Core.Entities;

namespace PatientTest.Infrastructure;

public class Context: DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
        //Database.EnsureCreated();
        Database.Migrate();
        
    }

    public DbSet<Genders> Genders { get; set; }
    public DbSet<Given> Given { get; set; }
    public DbSet<Patient> Patient { get; set; }
    

}
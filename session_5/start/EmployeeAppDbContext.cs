using Microsoft.EntityFrameworkCore;

namespace  training;
public class EmployeeAppDbContext : DbContext
{
    protected EmployeeAppDbContext()
    {
    }

    public EmployeeAppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Employee>()
            .Property(e => e.FullName)
            .HasMaxLength(200);
        
        modelBuilder.Entity<Employee>()
            .Property(e => e.Address)
            .HasMaxLength(500);
    }
}
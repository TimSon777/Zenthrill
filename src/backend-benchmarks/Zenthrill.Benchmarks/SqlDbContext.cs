using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zenthrill.Benchmarks;

public class SqlDbContext : DbContext
{
    public DbSet<Fragment> Fragments => Set<Fragment>();
    public DbSet<Branch> Branches => Set<Branch>();

    public SqlDbContext()
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");
    }
}

public class Fragment
{
    public long Id { get; set; }
    
    public string Content { get; set; }
    
    public ICollection<Branch> Branches { get; set; }
}

public class Branch
{
    public long Id { get; set; }
    
    [ForeignKey("FromFragment")]
    public long FromFragmentId { get; set; }
    
    [ForeignKey("ToFragment")]
    public long ToFragmentId { get; set; }
    
    [Column(TypeName = "varchar(255)")]
    public string Description { get; set; }
    
    public Fragment FromFragment { get; set; }
    
    public Fragment ToFragment { get; set; }
}
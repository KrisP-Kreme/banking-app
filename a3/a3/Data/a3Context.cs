using Microsoft.EntityFrameworkCore;

namespace a3.Models;

public class a3Context : DbContext
{
    public DbSet<Courses> Courses { get; set; }
    public DbSet<Enrolled> Enrolled { get; set; }
    public DbSet<Students> Students { get; set; }

    public a3Context(DbContextOptions<a3Context> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrolled>().HasKey(x => new { x.CourseID, x.StudentID });
    }
}

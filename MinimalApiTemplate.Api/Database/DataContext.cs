using Microsoft.EntityFrameworkCore;
using MinimalApiTemplate.Api.Entities;

namespace MinimalApiTemplate.Api.Database;

public class DataContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) {}
}

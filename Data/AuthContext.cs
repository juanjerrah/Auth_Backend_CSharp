using AuthApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApplication.Data;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
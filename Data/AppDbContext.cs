using Api_Personajas.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Personajas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Personaje> Personajes => Set<Personaje>();
}

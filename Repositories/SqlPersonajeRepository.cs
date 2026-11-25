using Api_Personajas.Data;
using Api_Personajas.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Personajas.Repositories;

public class SqlPersonajeRepository : IPersonajeRepository
{
    private readonly AppDbContext _context;

    public SqlPersonajeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Personaje>> GetAllAsync()
    {
        return await _context.Personajes.AsNoTracking().ToListAsync();
    }

    public async Task<Personaje?> GetByIdAsync(int id)
    {
        return await _context.Personajes.FindAsync(id);
    }

    public async Task<List<Personaje>> GetByVideojuegoAsync(string videojuego)
    {
        var busqueda = videojuego.ToLower();
        return await _context.Personajes
            .Where(p => p.Videojuego.ToLower() == busqueda)
            .ToListAsync();
    }

    public async Task<List<Personaje>> GetByNivelMinimoAsync(int nivel)
    {
        return await _context.Personajes
            .Where(p => p.Nivel >= nivel)
            .ToListAsync();
    }

    public async Task<Personaje> AddAsync(Personaje personaje)
    {
        _context.Personajes.Add(personaje);
        await _context.SaveChangesAsync();
        return personaje;
    }

    public async Task<bool> UpdateAsync(Personaje personaje)
    {
        var existe = await _context.Personajes.AnyAsync(p => p.Id == personaje.Id);
        if (!existe) return false;

        _context.Personajes.Update(personaje);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var personaje = await _context.Personajes.FindAsync(id);
        if (personaje is null) return false;

        _context.Personajes.Remove(personaje);
        await _context.SaveChangesAsync();
        return true;
    }
}

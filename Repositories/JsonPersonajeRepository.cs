using System.Text.Json;
using Api_Personajas.Models;

namespace Api_Personajas.Repositories;

public class JsonPersonajeRepository : IPersonajeRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public JsonPersonajeRepository(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "Data", "personajes.json");
    }

    private async Task<List<Personaje>> LeerArchivoAsync()
    {
        if (!File.Exists(_filePath))
            return new List<Personaje>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Personaje>>(json, _jsonOptions) ?? new List<Personaje>();
    }

    private async Task GuardarArchivoAsync(List<Personaje> personajes)
    {
        var json = JsonSerializer.Serialize(personajes, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<List<Personaje>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try { return await LeerArchivoAsync(); }
        finally { _lock.Release(); }
    }

    public async Task<Personaje?> GetByIdAsync(int id)
    {
        var personajes = await GetAllAsync();
        return personajes.FirstOrDefault(p => p.Id == id);
    }

    public async Task<List<Personaje>> GetByVideojuegoAsync(string videojuego)
    {
        var personajes = await GetAllAsync();
        return personajes
            .Where(p => p.Videojuego.Equals(videojuego, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<List<Personaje>> GetByNivelMinimoAsync(int nivel)
    {
        var personajes = await GetAllAsync();
        return personajes.Where(p => p.Nivel >= nivel).ToList();
    }

    public async Task<Personaje> AddAsync(Personaje personaje)
    {
        await _lock.WaitAsync();
        try
        {
            var personajes = await LeerArchivoAsync();
            personaje.Id = personajes.Count == 0 ? 1 : personajes.Max(p => p.Id) + 1;
            personajes.Add(personaje);
            await GuardarArchivoAsync(personajes);
            return personaje;
        }
        finally { _lock.Release(); }
    }

    public async Task<bool> UpdateAsync(Personaje personaje)
    {
        await _lock.WaitAsync();
        try
        {
            var personajes = await LeerArchivoAsync();
            var index = personajes.FindIndex(p => p.Id == personaje.Id);
            if (index == -1) return false;

            personajes[index] = personaje;
            await GuardarArchivoAsync(personajes);
            return true;
        }
        finally { _lock.Release(); }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _lock.WaitAsync();
        try
        {
            var personajes = await LeerArchivoAsync();
            var personaje = personajes.FirstOrDefault(p => p.Id == id);
            if (personaje is null) return false;

            personajes.Remove(personaje);
            await GuardarArchivoAsync(personajes);
            return true;
        }
        finally { _lock.Release(); }
    }
}

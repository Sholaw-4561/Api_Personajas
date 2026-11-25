using Api_Personajas.Models;

namespace Api_Personajas.Repositories;

public interface IPersonajeRepository
{
    Task<List<Personaje>> GetAllAsync();
    Task<Personaje?> GetByIdAsync(int id);
    Task<List<Personaje>> GetByVideojuegoAsync(string videojuego);
    Task<List<Personaje>> GetByNivelMinimoAsync(int nivel);
    Task<Personaje> AddAsync(Personaje personaje);
    Task<bool> UpdateAsync(Personaje personaje);
    Task<bool> DeleteAsync(int id);
}

using Api_Personajas.DTOs;
using Api_Personajas.Models;

namespace Api_Personajas.Services;

public interface IPersonajeService
{
    Task<List<Personaje>> GetAllAsync();
    Task<Personaje?> GetByIdAsync(int id);
    Task<List<Personaje>> GetByVideojuegoAsync(string videojuego);
    Task<(bool EsValido, string? Error, List<Personaje>? Personajes)> GetByNivelMinimoAsync(int nivel);
    Task<(bool EsValido, string? Error, Personaje? Personaje)> CreateAsync(PersonajeCreateDto dto);
    Task<(bool EsValido, string? Error)> UpdateAsync(int id, PersonajeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

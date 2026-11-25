using Api_Personajas.DTOs;
using Api_Personajas.Models;
using Api_Personajas.Repositories;

namespace Api_Personajas.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IPersonajeRepository _repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Personaje>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Personaje?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public Task<List<Personaje>> GetByVideojuegoAsync(string videojuego) =>
        _repository.GetByVideojuegoAsync(videojuego);

    public async Task<(bool EsValido, string? Error, List<Personaje>? Personajes)> GetByNivelMinimoAsync(int nivel)
    {
        if (nivel < 1 || nivel > 100)
            return (false, "El nivel debe estar entre 1 y 100.", null);

        var personajes = await _repository.GetByNivelMinimoAsync(nivel);
        return (true, null, personajes);
    }

    public async Task<(bool EsValido, string? Error, Personaje? Personaje)> CreateAsync(PersonajeCreateDto dto)
    {
        var error = ValidarCampos(dto.Nombre, dto.Videojuego, dto.Clase, dto.Nivel, dto.Poder, dto.ImagenUrl);
        if (error is not null)
            return (false, error, null);

        var personaje = new Personaje
        {
            Nombre = dto.Nombre,
            Videojuego = dto.Videojuego,
            Clase = dto.Clase,
            Nivel = dto.Nivel,
            Poder = dto.Poder,
            ImagenUrl = dto.ImagenUrl,
            Disponible = true
        };

        var creado = await _repository.AddAsync(personaje);
        return (true, null, creado);
    }

    public async Task<(bool EsValido, string? Error)> UpdateAsync(int id, PersonajeUpdateDto dto)
    {
        var error = ValidarCampos(dto.Nombre, dto.Videojuego, dto.Clase, dto.Nivel, dto.Poder, dto.ImagenUrl);
        if (error is not null)
            return (false, error);

        var existente = await _repository.GetByIdAsync(id);
        if (existente is null)
            return (false, null);

        existente.Nombre = dto.Nombre;
        existente.Videojuego = dto.Videojuego;
        existente.Clase = dto.Clase;
        existente.Nivel = dto.Nivel;
        existente.Poder = dto.Poder;
        existente.Disponible = dto.Disponible;
        existente.ImagenUrl = dto.ImagenUrl;

        await _repository.UpdateAsync(existente);
        return (true, null);
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

    private static string? ValidarCampos(string nombre, string videojuego, string clase, int nivel, int poder, string imagenUrl)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return "El nombre es obligatorio.";
        if (string.IsNullOrWhiteSpace(videojuego))
            return "El videojuego es obligatorio.";
        if (string.IsNullOrWhiteSpace(clase))
            return "La clase es obligatoria.";
        if (nivel < 1 || nivel > 100)
            return "El nivel debe estar entre 1 y 100.";
        if (poder <= 0)
            return "El poder debe ser mayor que 0.";
        if (string.IsNullOrWhiteSpace(imagenUrl) ||
            !Uri.TryCreate(imagenUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            return "La imagenUrl debe ser una URL absoluta válida (http o https).";

        return null;
    }
}

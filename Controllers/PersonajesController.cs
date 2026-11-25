using Api_Personajas.DTOs;
using Api_Personajas.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api_Personajas.Controllers;

[ApiController]
[Route("api/personajes")]
public class PersonajesController : ControllerBase
{
    private readonly IPersonajeService _service;

    public PersonajesController(IPersonajeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var personaje = await _service.GetByIdAsync(id);
        return personaje is null ? NotFound() : Ok(personaje);
    }

    [HttpGet("videojuego/{nombre}")]
    public async Task<IActionResult> GetByVideojuego(string nombre)
    {
        return Ok(await _service.GetByVideojuegoAsync(nombre));
    }

    [HttpGet("nivel/{nivel:int}")]
    public async Task<IActionResult> GetByNivelMinimo(int nivel)
    {
        var (esValido, error, personajes) = await _service.GetByNivelMinimoAsync(nivel);
        return esValido ? Ok(personajes) : BadRequest(new { mensaje = error });
    }

    [HttpPost]
    public async Task<IActionResult> Post(PersonajeCreateDto dto)
    {
        var (esValido, error, personaje) = await _service.CreateAsync(dto);
        if (!esValido)
            return BadRequest(new { mensaje = error });

        return CreatedAtAction(nameof(GetById), new { id = personaje!.Id }, personaje);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, PersonajeUpdateDto dto)
    {
        var existente = await _service.GetByIdAsync(id);
        if (existente is null)
            return NotFound();

        var (esValido, error) = await _service.UpdateAsync(id, dto);
        return esValido ? NoContent() : BadRequest(new { mensaje = error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _service.DeleteAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}

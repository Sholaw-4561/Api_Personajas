namespace Api_Personajas.DTOs;

public class PersonajeUpdateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Videojuego { get; set; } = string.Empty;
    public string Clase { get; set; } = string.Empty;
    public int Nivel { get; set; }
    public int Poder { get; set; }
    public bool Disponible { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
}

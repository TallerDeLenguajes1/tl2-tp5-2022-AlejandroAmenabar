namespace tp5.Models;
public abstract class Persona
{
    public Persona()
    {
    }

    protected Persona(int id, string nombre, string nombreUsuario, string clave, Rol rol, string direccion, string telefono)
    {
        Id = id;
        Nombre = nombre;
        NombreUsuario = nombreUsuario;
        Clave = clave;
        Rol = rol;
        Direccion = direccion;
        Telefono = telefono;
    }
    public int Id {get; set;}
    public string Nombre{get; set;}
    public string NombreUsuario { get; set; }
    public string Clave { get; set; }
    public Rol Rol { get; set; }
    
    public string Direccion{get; set;}
    public string Telefono{get; set;}
    
    public override string ToString()
    {
        return "Código: " + Id + " Nombre: " + Nombre + " Dirección: " + Direccion + " Teléfono: " + Telefono;
    }
}

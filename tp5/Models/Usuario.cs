namespace tp5.Models;

public class Usuario : Persona
{
    public Usuario()
    {
    }

    public Usuario(int id, string nombre, string nombreUsuario, string clave, Rol rol, string direccion, string telefono) : base(id, nombre, nombreUsuario, clave, rol, direccion, telefono)
    {
    }
}
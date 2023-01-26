namespace tp5.Repositories;

public class RepositorioUsuario : IRepositorioUsuario
{
    protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly IConfiguration _configuration;
    protected readonly string? CadenaConexion;

    public RepositorioUsuario(IConfiguration configuration)
    {
        _configuration = configuration;
        CadenaConexion = _configuration.GetConnectionString("ConnectionString");
    }

    public Usuario? Verificar(Usuario usuario)
    {
        var consulta =
            $"select * from Usuario where usuario = '{usuario.NombreUsuario}' and clave = '{usuario.Clave}'";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            using var reader = peticion.ExecuteReader();
            Usuario? salida = null;

            while (reader.Read())
                salida = new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    NombreUsuario = reader.GetString(2),
                    Clave = reader.GetString(3),
                    Rol = (Rol)reader.GetInt32(4),
                    Direccion = reader.GetString(5),
                    Telefono = reader.GetString(6)
                };

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar el usuario {Id} - {Error}", usuario.Id, e.Message);
        }

        return null;
    }

    public IEnumerable<Usuario> BuscarTodosPorRol(Rol rol)
    {
        const string consulta = "select * from Usuario where @rol = rol";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            peticion.Parameters.AddWithValue("@rol", rol);
            conexion.Open();

            var salida = new List<Usuario>();
            using var reader = peticion.ExecuteReader();
            while (reader.Read())
            {
                var usuario = new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    NombreUsuario = reader.GetString(2),
                    Clave = reader.GetString(3),
                    Rol = (Rol)reader.GetInt32(4),
                    Direccion = reader.GetString(5),
                    Telefono = reader.GetString(6)
                };

                salida.Add(usuario);
            }

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar todos los usuarios - {Error}", e.Message);
        }

        return new List<Usuario>();
    }

    public Usuario? BuscarPorId(int id)
    {
        const string consulta = "select * from Usuario where id_usuario = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            peticion.Parameters.AddWithValue("@id", id);
            conexion.Open();

            Usuario salida = null; 
            using var reader = peticion.ExecuteReader();

            while (reader.Read())
                salida = new Usuario()
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    NombreUsuario = reader.GetString(2),
                    Rol = (Rol)reader.GetInt32(4),
                    Direccion = reader.GetString(5),
                    Telefono = reader.GetString(6)
                };
            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar el usuario {Id} - {Error}", id, e.Message);
        }

        return null;
    }

    public IEnumerable<Usuario> BuscarTodos()
    {
        const string consulta = "select * from Usuario";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            var salida = new List<Usuario>();
            using var reader = peticion.ExecuteReader();
            while (reader.Read())
            {
                var usuario = new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    NombreUsuario = reader.GetString(2),
                    Clave = reader.GetString(3),
                    Rol = (Rol)reader.GetInt32(4),
                    Direccion = reader.GetString(5),
                    Telefono = reader.GetString(6)
                };

                salida.Add(usuario);
            }

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar todos los usuarios - {Error}", e.Message);
        }

        return new List<Usuario>();
    }

    public void Insertar(Usuario entidad)
    {
        const string consulta =
            "insert into Usuario (nombre, usuario, clave, rol, direccion, telefono) values (@nombre, @usuario, @clave, @rol, @direccion, @telefono)";
            
        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            peticion.Parameters.AddWithValue("@nombre", entidad.Nombre);
            peticion.Parameters.AddWithValue("@usuario", entidad.NombreUsuario);
            peticion.Parameters.AddWithValue("@clave", entidad.Clave);
            peticion.Parameters.AddWithValue("@rol", entidad.Rol);
            peticion.Parameters.AddWithValue("@direccion", entidad.Direccion);
            peticion.Parameters.AddWithValue("@telefono", entidad.Telefono);
            peticion.ExecuteNonQuery();
            conexion.Close();
        }
        catch (Exception e)
        {
            Logger.Debug("Error al insertar el cadete {Id} - {Error}", entidad.Id, e.Message);
        }
    }

    public void Actualizar(Usuario entidad)
    {
        const string consulta =
            "update Usuario set nombre = @nombre, direccion = @direccion, telefono = @telefono where id_usuario = @id";
        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            peticion.Parameters.AddWithValue("@id", entidad.Id);
            peticion.Parameters.AddWithValue("@nombre", entidad.Nombre);
            peticion.Parameters.AddWithValue("@direccion", entidad.Direccion);
            peticion.Parameters.AddWithValue("@telefono", entidad.Telefono);
            peticion.ExecuteReader();
            conexion.Close();
        }
        catch (Exception e)
        {
            Logger.Debug("Error al actualizar el usuario  {Id} - {Error}", entidad.Id, e.Message);
        }
    }

    public void Eliminar(int id)
    {
        const string consulta =
            "delete from Usuario where id_usuario = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();
            
            peticion.Parameters.AddWithValue("@id", id);
            peticion.ExecuteReader();
            conexion.Close();
        }
        catch (Exception e)
        {
            Logger.Debug("Error al eliminar el usuario {Id} - {Error}", id, e.Message);
        }
    }

    public IEnumerable<Usuario> BuscarTodosPorId(int id)
    {
        throw new NotImplementedException();
    }
}
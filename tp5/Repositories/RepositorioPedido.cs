namespace tp5.Repositories;

public class RepositorioPedido : IRepositorioPedido
{
    protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly IConfiguration _configuration;
    protected readonly string? CadenaConexion;
    
    public RepositorioPedido(IConfiguration configuration) 
    {
        _configuration = configuration;
        CadenaConexion = _configuration.GetConnectionString("ConnectionString");
    }

    public Pedido? BuscarPorId(int id)
    {
        const string consulta = "select * from Pedido where id_pedido = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            peticion.Parameters.AddWithValue("@id", id);
            conexion.Open();

            var salida = new Pedido();
            using var reader = peticion.ExecuteReader();
            while (reader.Read())
                salida = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Cliente = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar el pedido {Id} - {Error}", id, e.Message);
        }

        return null;
    }

    public IEnumerable<Pedido> BuscarTodos()
    {
        const string consulta = "select * from Pedido";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            var salida = new List<Pedido>();
            using var reader = peticion.ExecuteReader();
            while (reader.Read())
            {
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.GetInt32(3),
                    Cliente = reader.GetInt32(4)
                };
                salida.Add(pedido);
            }

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar todos los pedidos - {Error}", e.Message);
        }

        return new List<Pedido>();
    }

    public void Insertar(Pedido entidad)
    {
        const string consulta =
            "insert into Pedido (observacion, estado, id_cadete, id_cliente) values (@observacion, @estado, @cadete, @cliente)";
        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            peticion.Parameters.AddWithValue("@observacion", entidad.Observacion);
            peticion.Parameters.AddWithValue("@estado", entidad.Estado);
            peticion.Parameters.AddWithValue("@cadete", entidad.Cadete);
            peticion.Parameters.AddWithValue("@cliente", entidad.Cliente);
            peticion.ExecuteNonQuery();
            conexion.Close();
        }
        catch (Exception e)
        {
            Logger.Debug("Error al insertar el pedido {Id} - {Error}", entidad.Id, e.Message);
        }
    }

    public void Actualizar(Pedido entidad)
    {
        const string consulta =
            "update Pedido set observacion = @observacion, estado = @estado, id_cadete = @cadete, id_cliente = @cliente where id_pedido = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            peticion.Parameters.AddWithValue("@id", entidad.Id);
            peticion.Parameters.AddWithValue("@observacion", entidad.Observacion);
            peticion.Parameters.AddWithValue("@estado", entidad.Estado);
            peticion.Parameters.AddWithValue("@cadete", entidad.Cadete);
            peticion.Parameters.AddWithValue("@cliente", entidad.Cliente);
            peticion.ExecuteReader();
            conexion.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Logger.Debug("Error al insertar el pedido {Id} - {Error}", entidad.Id, e.Message);
        }
    }

    public void Eliminar(int id)
    {
        const string consulta =
            "delete from Pedido where id_pedido = @id";

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
            Logger.Debug("Error al eliminar el pedido {Id} - {Error}", id, e.Message);
        }
    }

    public IEnumerable<Pedido> BuscarTodosPorId(int id)
    {
        const string consulta = "select * from Pedido where id_cadete = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            peticion.Parameters.AddWithValue("@id", id);
            conexion.Open();

            var salida = new List<Pedido>();
            using var reader = peticion.ExecuteReader();
            while (reader.Read())
            {
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Cliente = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };
                salida.Add(pedido);
            }

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar todos los pedidos del cliente {Id}- {Error}", id, e.Message);
        }

        return new List<Pedido>();
    }


    public IEnumerable<Pedido> BuscarTodosPorUsuarioYRol(int idUsuario, Rol rol)
    {
        string consulta;
        consulta = rol == Rol.Cadete ? "select * from Pedido where id_cadete = @id" : "select * from Pedido where id_cliente = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            peticion.Parameters.AddWithValue("@id", idUsuario);
            conexion.Open();

            var salida = new List<Pedido>();
            using var reader = peticion.ExecuteReader();
            while (reader.Read())
            {
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Cliente = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };
                salida.Add(pedido);
            }

            conexion.Close();
            return salida;
        }
        catch (Exception e)
        {
            Logger.Debug("Error al buscar todos los pedidos del cliente {Id}- {Error}", idUsuario, e.Message);
        }

        return new List<Pedido>();
    }
}
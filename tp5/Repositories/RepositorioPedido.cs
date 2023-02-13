namespace tp5.Repositories;

public interface IRepositorioPedido
{
    Pedido? BuscarPorId(int id);

    IEnumerable<Pedido> BuscarTodos();

    void Insertar(Pedido entidad);

    void Actualizar(Pedido entidad);

    void Eliminar(int id);

    IEnumerable<Pedido> BuscarTodosPorUsuarioYRol(int idUsuario, Rol rol);

    IEnumerable<Pedido> BuscarPedidosPorCadete(int id);
    IEnumerable<Pedido> BuscarPedidosPorCliente(int id);

}

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

    // El método BuscarPorId es utilizado para buscar un pedido en la base de datos por su ID.
    // Se inicia construyendo una cadena de consulta SQL para seleccionar todos los campos de la tabla Pedido donde el ID del pedido sea igual al ID proporcionado como argumento.
    // Luego, se intenta realizar la búsqueda dentro de un bloque try-catch. Dentro de este bloque, se crea una conexión a la base de datos usando SQLite y una petición a través de un objeto SqliteCommand.
    // Se agrega el ID proporcionado como argumento como parámetro en la consulta SQL. Se abre la conexión a la base de datos y se ejecuta la consulta.
    // Si se encuentra un resultado, se crea un nuevo objeto Pedido y se asignan los valores a sus propiedades correspondientes leyendo los datos desde el objeto SqliteDataReader.
    // Se verifica si los campos de Cadete o Cliente son nulos antes de asignarlos, de ser así, se establecen en null.
    // Se cierra la conexión a la base de datos y se devuelve el objeto Pedido encontrado.
    // Si ocurre una excepción, se registra en el registro de depuración con los detalles del error y se devuelve null.

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

    // Metodo que busca todos los Pedidos en la base de datos
    public IEnumerable<Pedido> BuscarTodos()
    {
        // Consulta SQL para obtener todos los registros de la tabla Pedido
        const string consulta = "select * from Pedido";
        
        try
        {
            // Se crea una conexion con la base de datos utilizando SQLite y se pasa la cadena de conexion
            using var conexion = new SqliteConnection(CadenaConexion);
            // Se crea un comando para la conexion y la consulta especificada
            var peticion = new SqliteCommand(consulta, conexion);
            // Se abre la conexion
            conexion.Open();

            // Se crea una lista de Pedidos que sera la salida
            var salida = new List<Pedido>();
            // Se ejecuta la consulta y se obtiene un reader
            using var reader = peticion.ExecuteReader();
            // Se itera sobre los resultados del reader
            while (reader.Read())
            {
                // Se crea un nuevo objeto Pedido para cada resultado
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.GetInt32(3),
                    Cliente = reader.GetInt32(4)
                };
                // Se agrega el objeto Pedido a la lista de salida
                salida.Add(pedido);
            }

            // Se cierra la conexion
            conexion.Close();
            // Se retorna la lista de Pedidos
            return salida;
        }
        catch (Exception e)
        {
            // En caso de un error, se escribe un mensaje en el log
            Logger.Debug("Error al buscar todos los pedidos - {Error}", e.Message);
        }

        // Si ocurre un error, se retorna una lista vacia de Pedidos
        return new List<Pedido>();
    }

    // Este método inserta un nuevo registro en la tabla "Pedido" en la base de datos
    public void Insertar(Pedido entidad)
    {
        // La consulta SQL para insertar un nuevo registro en la tabla "Pedido"
        const string consulta = "insert into Pedido (observacion, estado, id_cadete, id_cliente) values (@observacion, @estado, @cadete, @cliente)";
        
        try
        {
            // Creación de una conexión a la base de datos usando SqliteConnection y la cadena de conexión.
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();

            // Asignación de los valores de los parámetros de la consulta
            peticion.Parameters.AddWithValue("@observacion", entidad.Observacion);
            peticion.Parameters.AddWithValue("@estado", entidad.Estado);
            peticion.Parameters.AddWithValue("@cadete", entidad.Cadete);
            peticion.Parameters.AddWithValue("@cliente", entidad.Cliente);
            
            // Ejecución de la consulta que no retorna ningún resultado
            peticion.ExecuteNonQuery();
            
            // Cierre de la conexión a la base de datos
            conexion.Close();
        }
        catch (Exception e)
        {
            // Log de error en caso de que se produzca una excepción durante la ejecución del método
            Logger.Debug("Error al insertar el pedido {Id} - {Error}", entidad.Id, e.Message);
        }
    }


    public void Actualizar(Pedido entidad)
    {
        // Se define la constante 'consulta' con la sentencia SQL para actualizar un pedido en la base de datos.
        const string consulta =
            "update Pedido set observacion = @observacion, estado = @estado, id_cadete = @cadete, id_cliente = @cliente where id_pedido = @id";
        // Se inicia un bloque try-catch para controlar posibles excepciones al momento de ejecutar la consulta SQL.
        try
        {
            // Se crea una conexión a la base de datos utilizando la clase SqliteConnection y se asigna a la variable 'conexion'.
            using var conexion = new SqliteConnection(CadenaConexion);
            // Se crea una nueva instancia de la clase SqliteCommand para ejecutar la consulta SQL y se asigna a la variable 'peticion'.
            var peticion = new SqliteCommand(consulta, conexion);
            // Se abre la conexión a la base de datos.
            conexion.Open();
            // Se agrega el valor de cada uno de los parámetros de la consulta SQL a la petición
            peticion.Parameters.AddWithValue("@id", entidad.Id);
            peticion.Parameters.AddWithValue("@observacion", entidad.Observacion);
            peticion.Parameters.AddWithValue("@estado", entidad.Estado);
            peticion.Parameters.AddWithValue("@cadete", entidad.Cadete);
            peticion.Parameters.AddWithValue("@cliente", entidad.Cliente);
            // Se ejecuta la consulta SQL y se asigna el resultado a la variable 'peticion'
            peticion.ExecuteReader();
            // Se cierra la conexión a la base de datos.
            conexion.Close();
        }
        catch (Exception e) // En caso de que ocurra una excepción durante la ejecución de la consulta SQL, se captura en el bloque catch.
        {
            Console.WriteLine(e.Message);
            Logger.Debug("Error al insertar el pedido {Id} - {Error}", entidad.Id, e.Message);
        }
    }

    public void Eliminar(int id)
    {
        // Consulta SQL para eliminar un registro en la tabla "Pedido" con el identificador especificado.
        const string consulta =
            "delete from Pedido where id_pedido = @id";

        try
        {
            // Creación de una conexión a la base de datos utilizando SQLite.
            using var conexion = new SqliteConnection(CadenaConexion);
            var peticion = new SqliteCommand(consulta, conexion);
            conexion.Open();
            // Agrega un parámetro a la consulta SQL con el valor de "id" especificado
            peticion.Parameters.AddWithValue("@id", id);
            peticion.ExecuteReader();
            conexion.Close();
        }
        catch (Exception e)
        {
            // Registra un mensaje de error en el registro en caso de que ocurra una excepción.
            Logger.Debug("Error al eliminar el pedido {Id} - {Error}", id, e.Message);
        }
    }

    //Este metodo realiza una consulta a la tabla "Pedido" para buscar todos los pedidos asociados al identificador especificado.
    public IEnumerable<Pedido> BuscarPedidosPorCadete(int id)
    {
        const string consulta = "select * from Pedido where id_cadete = @id";

        try
        {
            //Crea una nueva conexion con la base de datos utilizando SqliteConnection
            using var conexion = new SqliteConnection(CadenaConexion);
            //Crea un nuevo comando Sqlite con la consulta y la conexion
            var peticion = new SqliteCommand(consulta, conexion);
            //Agrega el parametro "@id" con el valor del id especificado al comando
            peticion.Parameters.AddWithValue("@id", id);
            //Abre la conexion con la base de datos
            conexion.Open();
            //Crea una nueva lista para almacenar los resultados de la consulta
            var salida = new List<Pedido>();
            //Ejecuta la consulta y almacena el resultado en un SqliteDataReader
            using var reader = peticion.ExecuteReader();
            //Recorre los resultados del reader
            while (reader.Read())
            {
                //Crea un nuevo objeto Pedido y lo llena con los valores de las columnas de la fila actual
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Cliente = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };
                //Agrega el objeto Pedido a la lista de resultados
                salida.Add(pedido);
            }

            //Cierra la conexion con la base de datos
            conexion.Close();
            //Retorna la lista de resultados
            return salida;
        }
        catch (Exception e)
        {
            //Registra un mensaje de error en el log en caso de haber alguna excepcion
            Logger.Debug("Error al buscar todos los pedidos del cliente {Id}- {Error}", id, e.Message);
        }

        //Retorna una lista vacia en caso de haber una excepcion
        return new List<Pedido>();
    }

    public IEnumerable<Pedido> BuscarPedidosPorCliente(int id)
    {
        const string consulta = "select * from Pedido where id_cliente = @id";

        try
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            //Crea un nuevo comando Sqlite con la consulta y la conexion
            var peticion = new SqliteCommand(consulta, conexion);
            //Agrega el parametro "@id" con el valor del id especificado al comando
            peticion.Parameters.AddWithValue("@id", id);
            //Abre la conexion con la base de datos
            conexion.Open();
            //Crea una nueva lista para almacenar los resultados de la consulta
            var salida = new List<Pedido>();
            //Ejecuta la consulta y almacena el resultado en un SqliteDataReader
            using var reader = peticion.ExecuteReader();
            //Recorre los resultados del reader
            while (reader.Read())
            {
                //Crea un nuevo objeto Pedido y lo llena con los valores de las columnas de la fila actual
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Cliente = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };
                //Agrega el objeto Pedido a la lista de resultados
                salida.Add(pedido);
            }

            //Cierra la conexion con la base de datos
            conexion.Close();
            //Retorna la lista de resultados
            return salida;
        }
        catch (Exception e)
        {
            //Registra un mensaje de error en el log en caso de haber alguna excepcion
            Logger.Debug("Error al buscar todos los pedidos del cliente {Id}- {Error}", id, e.Message);
        }

        //Retorna una lista vacia en caso de haber una excepcion
        return new List<Pedido>();
    }    
    // Método para buscar todos los pedidos por usuario y rol.
    public IEnumerable<Pedido> BuscarTodosPorUsuarioYRol(int idUsuario, Rol rol)
    {
        string consulta;
        // Se establece la consulta a realizar en base al rol del usuario
        consulta = rol == Rol.Cadete ? "select * from Pedido where id_cadete = @id" : "select * from Pedido where id_cliente = @id";

        // Se maneja un bloque try-catch para controlar posibles errores en la ejecución
        try
        {
            // Se crea una nueva conexión a la base de datos utilizando Sqlite
            using var conexion = new SqliteConnection(CadenaConexion);
            // Se crea una nueva petición a la base de datos con la consulta y la conexión
            var peticion = new SqliteCommand(consulta, conexion);
            // Se agrega un parámetro a la petición con el ID del usuario
            peticion.Parameters.AddWithValue("@id", idUsuario);
            // Se abre la conexión a la base de datos
            conexion.Open();

            // Se crea una lista para almacenar los pedidos encontrados
            var salida = new List<Pedido>();
            // Se ejecuta la petición y se almacena el resultado en un objeto reader
            using var reader = peticion.ExecuteReader();
            // Se recorre el reader para obtener los pedidos encontrados
            while (reader.Read())
            {
                // Se crea un objeto Pedido para almacenar la información leída del reader
                var pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    Observacion = reader.GetString(1),
                    Estado = reader.GetString(2),
                    Cadete = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Cliente = reader.IsDBNull(4) ? null : reader.GetInt32(4)
                };
                // Se agrega el pedido a la lista de salida
                salida.Add(pedido);
            }

            // Se cierra la conexión a la base de datos
            conexion.Close();
            // Se retorna la lista de pedidos encontrados
            return salida;
        }
        // En caso de que ocurra un error se captura en el bloque catch
        catch (Exception e)
        {
            Logger.Debug("Error al buscar todos los pedidos del cliente {Id}- {Error}", idUsuario, e.Message);
        }

        return new List<Pedido>();
    }
}
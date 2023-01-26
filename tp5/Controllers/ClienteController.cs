namespace tp5.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositorioUsuario _repositorioUsuario;

    public ClienteController(ILogger<ClienteController> logger, IRepositorioUsuario repositorioUsuario, IMapper mapper)
    {
        _logger = logger;
        _repositorioUsuario = repositorioUsuario;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            var clientes = _repositorioUsuario.BuscarTodosPorRol(Rol.Cliente);
            var clientesViewModel = _mapper.Map<List<UsuarioViewModel>>(clientes);
            return View(clientesViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Index {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult AltaCliente()
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            return View("AltaCliente");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Alta Cliente {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult AltaCliente(UsuarioViewModel clienteViewModel)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var cliente = _mapper.Map<Usuario>(clienteViewModel);
                _repositorioUsuario.Insertar(cliente);
            }
            else
            {
                var errores = ModelState.Values.SelectMany(x => x.Errors);
                Console.WriteLine(errores);
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Alta Cliente {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            var cliente = _repositorioUsuario.BuscarPorId(id);
            if (cliente is null) return RedirectToAction("Index");
            var clienteViewModel = _mapper.Map<UsuarioViewModel>(cliente);
            return View(clienteViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Modificar Cliente {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult ModificarCliente(UsuarioModificadoViewModel clienteViewModel)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var cliente = _mapper.Map<Usuario>(clienteViewModel);
                _repositorioUsuario.Actualizar(cliente);
            }
            else
            {
                var errores = ModelState.Values.SelectMany(x => x.Errors);
                Console.WriteLine(errores);
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Modificar Cliente {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult BajaCliente(int id)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            _repositorioUsuario.Eliminar(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Baja Cliente {Error}", e.Message);
            return View("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
            return RedirectToAction("Index", "Home");

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
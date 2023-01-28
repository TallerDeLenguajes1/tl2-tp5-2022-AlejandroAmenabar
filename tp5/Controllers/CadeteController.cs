namespace tp5.Controllers;
public class CadeteController : Controller
{
    private readonly ILogger<CadeteController> _logger;
    private readonly IMapper _mapper;

    private readonly IRepositorioUsuario _repositorioUsuario;

    private static int id;
    public CadeteController(ILogger<CadeteController> logger, IRepositorioUsuario repositorioUsuario,IMapper mapper)
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
            {
                return RedirectToAction("Index", "Home");
            }

            var cadetes = _repositorioUsuario.BuscarTodosPorRol(Rol.Cadete);
            var cadetesViewModel = _mapper.Map<List<UsuarioViewModel>>(cadetes);
            return View(cadetesViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder al Index {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult AltaCadete()
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("AltaCadete");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Alta de Cadete {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult AltaCadete(UsuarioViewModel cadeteViewModel)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var cadete = _mapper.Map<Usuario>(cadeteViewModel);
                _repositorioUsuario.Insertar(cadete);
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
            _logger.LogError("Error al acceder el Alta de Cadete {Error}", e.Message);
            return View("Error");
        }
    }

     [HttpGet]
    public IActionResult ModificarCadete(int id)
    {
         try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            var cadete = _repositorioUsuario.BuscarPorId(id);
            if (cadete is null) return RedirectToAction("Index");
            var cadeteViewModel = _mapper.Map<UsuarioViewModel>(cadete);
            return View(cadeteViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Modificar Cadete {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult ModificarCadete(UsuarioModificadoViewModel cadeteViewModel)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var cadete = _mapper.Map<Usuario>(cadeteViewModel);
                _repositorioUsuario.Actualizar(cadete);
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
            _logger.LogError("Error al acceder el Modificar Cadete {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult BajaCadete(int id)
    {
        try
        {
            if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            _repositorioUsuario.Eliminar(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Baja Cadete {Error}", e.Message);
            return View("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        if (HttpContext.Session.GetInt32(SessionRol) != (int)Rol.Administrador)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
namespace tp5.Controllers;

public class HomeController : Controller
{
    private const string SessionId = "Id";
    private const string SessionNombre = "Nombre";
    private const string SessionUsuario = "Usuario";
    private const string SessionRol = "Rol";
    private readonly ILogger<HomeController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositorioUsuario _repositorioUsuario;

    public HomeController(ILogger<HomeController> logger, IMapper mapper, IRepositorioUsuario repositorioUsuario)
    {
        _logger = logger;
        _mapper = mapper;
        _repositorioUsuario = repositorioUsuario;
    }

    // Es una acción HTTP GET que muestra una vista con la información de todos los usuarios. Utiliza el repositorio de usuarios para buscar todos los usuarios y luego los mapea a una lista de UsuarioViewModel para mostrarlos en la vista.
    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            var inicioViewModel = new HomeViewModel();
            var usuarios = _repositorioUsuario.BuscarTodos();
            var usuariosViewModel = _mapper.Map <List<UsuarioViewModel>>(usuarios);
            inicioViewModel.UsuarioViewModels = usuariosViewModel;
            return View(inicioViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder al Index {Error}", e.Message);
            return View("Error");
        }
    }

    //  Es una acción HTTP POST que se encarga de verificar las credenciales de inicio de sesión de un usuario. Utiliza el repositorio de usuarios para verificar las credenciales y si son válidas, establece las variables de sesión correspondientes.

    [HttpPost]
    public IActionResult InicioSesion(HomeViewModel homeViewModel)
    {
        try
        {
            var usuario = _mapper.Map<Usuario>(homeViewModel.LoginViewModel);
            usuario = _repositorioUsuario.Verificar(usuario);

            if (usuario is null || usuario.Rol == Rol.Ninguno) return RedirectToAction("Index");

            HttpContext.Session.SetInt32(SessionId, usuario.Id);
            HttpContext.Session.SetString(SessionNombre, usuario.Nombre);
            HttpContext.Session.SetString(SessionUsuario, usuario.NombreUsuario);
            HttpContext.Session.SetInt32(SessionRol, (int)usuario.Rol);

            return RedirectToAction("Index", "Pedido");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Inicio Sesión {Error}", e.Message);
            return View("Error");
        }
    }

    // Es una acción HTTP GET que se encarga de cerrar la sesión actual del usuario y redirigirlo al índice.
    [HttpGet]
    public IActionResult CerrarSesion()
    {
        try
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Cerrar Sesión {Error}", e.Message);
            return View("Error");
        }
    }
    
    // Es una acción que muestra una vista con la política de privacidad del sitio.
    public IActionResult Privacy()
    {
        return View();
    }

    // : Es una acción que muestra una vista de error en caso de algún problema en las acciones anteriores.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

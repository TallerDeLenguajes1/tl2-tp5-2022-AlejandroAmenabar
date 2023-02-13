namespace tp5.Controllers;
public class PedidoController : Controller
{
    private readonly ILogger<PedidoController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositorioPedido _repositorioPedido;
    private readonly IRepositorioUsuario _repositorioUsuario;
    public PedidoController(ILogger<PedidoController> logger, IRepositorioPedido repositorioPedido,
        IRepositorioUsuario repositorioUsuario, IMapper mapper)
    {
        _logger = logger;
        _repositorioPedido = repositorioPedido;
        _repositorioUsuario = repositorioUsuario;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            var sesionRol = (Rol)HttpContext.Session.GetInt32(SessionRol);

            switch (sesionRol)
            {
                case Rol.Administrador:
                {
                    var pedidos = _repositorioPedido.BuscarTodos();
                    var pedidosViewModel = _mapper.Map<List<PedidoViewModel>>(pedidos);

                    foreach (var pedido in pedidosViewModel)
                    {
                        var clienteId = int.Parse(pedido.Cliente);
                        var idCadete = int.Parse(pedido.Cadete);

                        pedido.Cliente = _repositorioUsuario.BuscarPorId(clienteId)?.Nombre;
                        pedido.Cadete = _repositorioUsuario.BuscarPorId(idCadete)?.Nombre;
                    }

                    return View(pedidosViewModel);
                }
                case Rol.Cadete:
                {
                    var idCadete = (int)HttpContext.Session.GetInt32(SessionId);
                    var pedidos = _repositorioPedido.BuscarTodosPorUsuarioYRol(idCadete, Rol.Cadete);
                    var pedidosViewModel = _mapper.Map<List<PedidoViewModel>>(pedidos);

                    foreach (var pedido in pedidosViewModel)
                    {
                        var idCliente = int.Parse(pedido.Cliente);
                        pedido.Cliente = _repositorioUsuario.BuscarPorId(idCliente)?.Nombre;
                    }

                    return View(pedidosViewModel);
                }
                case Rol.Cliente:
                {
                    var idCliente = (int)HttpContext.Session.GetInt32(SessionId);
                    var pedidos = _repositorioPedido.BuscarTodosPorUsuarioYRol(idCliente, Rol.Cliente);
                    var pedidosViewModel = _mapper.Map<List<PedidoViewModel>>(pedidos);

                    foreach (var pedido in pedidosViewModel)
                    {
                        var idCadete = int.Parse(pedido.Cadete);
                        pedido.Cadete = _repositorioUsuario.BuscarPorId(idCadete)?.Nombre;
                    }

                    return View(pedidosViewModel);
                }
                case Rol.Ninguno:
                    return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Index {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult AltaPedido()
    {
        try
        {
            var sesionRol = HttpContext.Session.GetInt32(SessionRol);
            if (sesionRol != (int)Rol.Administrador)
                return RedirectToAction("Index", "Home");

            var cadetes = _repositorioUsuario.BuscarTodosPorRol(Rol.Cadete);
            var clientes = _repositorioUsuario.BuscarTodosPorRol(Rol.Cliente);
            var cadetesViewModel = _mapper.Map<List<UsuarioViewModel>>(cadetes);
            var clientesViewModel = _mapper.Map<List<UsuarioViewModel>>(clientes);
            var pedidoAltaViewModel = new PedidoAltaViewModel(cadetesViewModel, clientesViewModel);
            return View(pedidoAltaViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Alta Pedido {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult AltaPedido(PedidoViewModel pedidoViewModel)
    {
        try
        {
            var sesionRol = HttpContext.Session.GetInt32(SessionRol);
            if (sesionRol != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var pedido = _mapper.Map<Pedido>(pedidoViewModel);
                _repositorioPedido.Insertar(pedido);
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
            _logger.LogError("Error al acceder el Alta Pedido {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult ModificarPedido(int id)
    {
        try
        {
            if(!HttpContext.Session.IsAvailable)
            {
                return RedirectToAction("Index", "Home");
            }

            var pedido = _repositorioPedido.BuscarPorId(id);

            var cadetes = _repositorioUsuario.BuscarTodosPorRol(Rol.Cadete);
            var cadetesViewModel = _mapper.Map<List<UsuarioViewModel>>(cadetes);

            var clientes = _repositorioUsuario.BuscarTodosPorRol(Rol.Cliente);
            var clientesViewModel = _mapper.Map<List<UsuarioViewModel>>(clientes);

            var pedidoModificadoViewModel = _mapper.Map<PedidoModificadoViewModel>(pedido);
            pedidoModificadoViewModel.Cadetes = cadetesViewModel;
            pedidoModificadoViewModel.Clientes = clientesViewModel;

            return View(pedidoModificadoViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Modificar Pedido {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult ModificarPedido(PedidoViewModel pedidoViewModel)
    {
        try
        {
            if(!HttpContext.Session.IsAvailable)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var pedido = _mapper.Map<Pedido>(pedidoViewModel);
                _repositorioPedido.Actualizar(pedido);
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
            _logger.LogError("Error al acceder el Modificar Pedido {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult BajaPedido(int id)
    {
        try
        {
            var sesionRol = HttpContext.Session.GetInt32(SessionRol);
            if (sesionRol != (int)Rol.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            _repositorioPedido.Eliminar(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError("Error al acceder el Baja Pedido {Error}", e.Message);
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult listarPedidosCadete(int idCadete, string nombreCadete){
        try
        {
            string rolUsuario = HttpContext.Session.GetString(SessionRol);
            if(rolUsuario == null) return RedirectToAction("Index","Home");

            var pedidos = _repositorioPedido.BuscarPedidosPorCadete(idCadete);
            var pedidosViewModel = _mapper.Map<List<PedidoViewModel>>(pedidos);

            PedidosCadeteViewModel listarPedidosCadeteViewModel = new PedidosCadeteViewModel(nombreCadete,pedidosViewModel);

            return View(listarPedidosCadeteViewModel);
        }
        catch (System.Exception ex)
        {
            ViewBag.Error = ex.Message; 
            return View(new PedidosCadeteViewModel());
        }
    }

    [HttpGet]
    public IActionResult listarPedidosCliente(int idCliente, string nombreCliente){
        try
        {
            string rolUsuario = HttpContext.Session.GetString(SessionRol);
            if(rolUsuario == null) return RedirectToAction("Index","Home");

            var pedidos = _repositorioPedido.BuscarPedidosPorCliente(idCliente);
            var pedidosViewModel = _mapper.Map<List<PedidoViewModel>>(pedidos);

            PedidosClienteViewModel listarPedidosClienteViewModel = new PedidosClienteViewModel(nombreCliente,pedidosViewModel);

            return View(listarPedidosClienteViewModel);
        }
        catch (System.Exception ex)
        {
            ViewBag.Error = ex.Message; 
            return View(new PedidosCadeteViewModel());
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var sesionRol = HttpContext.Session.GetInt32(SessionRol);
        if (sesionRol != (int)Rol.Administrador)
        {
            return RedirectToAction("Index", "Home");
        }else{
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

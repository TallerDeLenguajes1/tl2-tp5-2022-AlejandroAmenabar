global using tp5.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace tp5.Controllers
{
    
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly IMapper _mapper;
        private static int id;
        public PedidoController(ILogger<PedidoController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        private readonly static List<Pedido> Pedidos = new List<Pedido>();
        public IActionResult Index()
        {
            var PedidosViewModel = _mapper.Map<List<PedidoViewModel>>(Pedidos);
            id = Pedidos.Count;
            return View(PedidosViewModel);
        }

        [HttpGet]
        public IActionResult AltaPedido()
        {
            return View("AltaPedido");
        }
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
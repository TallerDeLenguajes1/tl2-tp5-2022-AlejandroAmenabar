global using tp4.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace tp4.Controllers
{
    
    public class CadeteController : Controller
    {
        private readonly ILogger<CadeteController> _logger;

        public CadeteController(ILogger<CadeteController> logger)
        {
            _logger = logger;
        }

        private readonly static List<Cadete> Cadetes = new List<Cadete>();

        private static int id;

        public IActionResult Index()
        {
            return View(Cadetes);
        }
        public IActionResult AltaCadete()
        {
            return View();
        }
        public void AltaCadeteExito(Cadete cadete)
        {
            cadete.Id = ++id;
            Cadetes.Add(cadete);
            Response.Redirect("/Cadete");
            //return View("Index", Cadetes);
        }

        [HttpGet]
        public void BajaCadete(int id)
        {
            Cadetes.RemoveAll(x => x.Id == id);
            Response.Redirect("/Cadete");
        }


    }
}
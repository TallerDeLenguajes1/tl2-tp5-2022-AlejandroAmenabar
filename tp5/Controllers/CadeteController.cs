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
    
    public class CadeteController : Controller
    {
        private readonly ILogger<CadeteController> _logger;
        private readonly IMapper _mapper;

        private static int id;
        public CadeteController(ILogger<CadeteController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        private readonly static List<Cadete> Cadetes = new List<Cadete>();


        public IActionResult Index()
        {
            var cadetesViewModel = _mapper.Map<List<CadeteViewModel>>(Cadetes);
            id = Cadetes.Count;
            return View(cadetesViewModel);
        }
        public IActionResult AltaCadete()
        {
            return View("AltaCadete");
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

        //fciones de javier
         [HttpGet]
        public IActionResult EditarCadete(int Id)
        {
            Cadete Cadete = Cadetes.Single(x => x.Id == Id);
            CadeteViewModel CadeteViewModel = _mapper.Map<CadeteViewModel>(Cadete);
            return View(CadeteViewModel);
        }

        [HttpPost]
        public IActionResult EditarCadete(CadeteViewModel CadeteViewModel)
        {
            int i = Cadetes.FindIndex(x => x.Id == CadeteViewModel.Id);
            Cadete Cadete = _mapper.Map<Cadete>(CadeteViewModel); //este mapeo crea un nuevo cadete y por lo tanto incrementa el contador.
            //Cadete.Contador--; //sho no tengo contador
            Cadetes[i] = Cadete;
            return RedirectToAction("ListadoDeCadetes");
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tp4.Models
{
    public class Cadete : Persona
    {
        public Cadete() : base(){}
        public Cadete(string nombre, string direccion, long telefono) : base(nombre, direccion, telefono){}
    }
}
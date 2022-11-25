using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tp4.Models
{
    public class Persona
    {
        public int Id {get; set;}
        public string Nombre{get; set;}
        public string Direccion{get; set;}
        public long Telefono{get; set;}
        public Persona(string n, string dir, long t){
            Nombre = n;
            Direccion = dir;
            Telefono = t;
        }
        public Persona(){}
    }
}
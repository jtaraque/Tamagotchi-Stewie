using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamagotchi_IPO2
{
    class Desbloqueable
    {
        private String nombre;
        private bool desbloqueado;


        public Desbloqueable(String n)
        {
            this.Nombre = n;
            this.Desbloqueado = false;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public bool Desbloqueado { get => desbloqueado; set => desbloqueado = value; }
    }
}

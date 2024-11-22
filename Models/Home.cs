using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiAppMovi.Models
{
        public class Home
        {
        public string Nombre { get; }
        public DateTime Fecha { get; }

        // Constructor para inicializar las propiedades
        public Home(string nombre, DateTime fecha)
        {
            Nombre = nombre;
            Fecha = fecha;
        }
    }
}

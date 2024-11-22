using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiAppMovi.Models
{
    public class Event
    {
        public string Nombre { get; }
        public DateTime Fecha { get; }
        public string Ubicacion { get; }
        public string Descripcion { get; }

        // Constructor para inicializar las propiedades
        public Event(string nombre,
                     string ubicacion,
                     string descripcion,
                     DateTime fecha)
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
            Descripcion = descripcion;
            Fecha = fecha;
        }
    }
}

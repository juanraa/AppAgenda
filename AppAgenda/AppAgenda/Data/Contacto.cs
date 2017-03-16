using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAgenda.Data
{
    public class Contacto
    {
        [PrimaryKey AutoIncrement]
        public int IdContacto { get; set; }
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
    }
}

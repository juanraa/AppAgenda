using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppAgenda.Data
{
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Platform, System.IO.Path.Combine(config.DirectoryDB, "db_Contactos.db3"));
            connection.CreateTable<Contacto>();
        }
        public void InsertarContacto(Contacto contacto)
        {
            connection.Insert(contacto);
        }
        public void UpdateContacto(Contacto contacto)
        {
            connection.Update(contacto);
        }
        public void DeleteContacto(Contacto contacto)
        {
            connection.Delete(contacto);
        }
        public Contacto GetContacto(int IdContacto)
        {
            return connection.Table<Contacto>().FirstOrDefault(c => c.IdContacto == IdContacto);
        }
        public List<Contacto> GetContactos()
        {
            return connection.Table<Contacto>().OrderBy(c => c.Nombre).ToList();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}

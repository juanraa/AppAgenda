using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppAgenda.Cells
{
    public class ContactoCell : ViewCell
    {
        public ContactoCell()
        {
            var txtNombre = new Label
            {
                FontSize = 20, 
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            txtNombre.SetBinding(Label.TextProperty, new Binding("Nombre"));

            var txtTelefono = new Label
            {
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            txtTelefono.SetBinding(Label.TextProperty, new Binding("Telefono"));

            var linea1 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    txtNombre
                },
            };
            var linea2 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    txtTelefono
                },
            };

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    linea1, linea2
                },
            };
        }                              

    }
}

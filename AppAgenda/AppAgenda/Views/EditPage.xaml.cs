using AppAgenda.Data;
using Plugin.Media;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        private Contacto contacto;
        string ruta; 
        public EditPage(Contacto contacto)
        {
            InitializeComponent();

            this.contacto = contacto;

            //Se agrega padding personalizadopara cada plataforma. 
            this.Padding = Device.OnPlatform(
                new Thickness(10, 20, 10, 10),
                new Thickness(10),
                new Thickness(10));

            //pasamos la ruta. 
            ruta = contacto.Imagen;

            Foto.Source = ImageSource.FromFile(ruta);
            txtNombre.Text = contacto.Nombre;
            txtCorreo.Text = contacto.Correo;
            txtTelefono.Text = contacto.Telefono;

            btnTomarFoto.Clicked += BtnTomarFoto_Clicked;
            btnSeleccionarFoto.Clicked += BtnSeleccionarFoto_Clicked;
            btnEditar.Clicked += BtnEditar_Clicked;
            btnEliminar.Clicked += BtnEliminar_Clicked;
        }

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            var rta = await DisplayAlert("Confirmación", "¿Desea borrar el contacto?", "Si", "No");
            if (!rta)
            {
                return;
            }
            using (var datos = new DataAccess())
            {
                datos.DeleteContacto(contacto);                
            }

            await DisplayAlert("Confirmación", "Contacto borrado correctamente", "Aceptar");
            await Navigation.PopAsync();


        }

        private async void BtnEditar_Clicked(object sender, EventArgs e)
        {
            //Validaciones para que los campos estén llenados         
            if (string.IsNullOrEmpty(ruta))
            {
                await DisplayAlert("Error", "Debe ingresar una Imágen", "Aceptar");                
                return;
            }
               
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un Nombre", "Aceptar");
                txtNombre.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCorreo.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un Correo", "Aceptar");
                txtCorreo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un Teléfono", "Aceptar");
                return;
            }

            contacto.Imagen = ruta;
            contacto.Nombre = txtNombre.Text;
            contacto.Correo = txtCorreo.Text;
            contacto.Telefono = txtTelefono.Text;

            using (var datos = new DataAccess())
            {
                datos.UpdateContacto(contacto);
            }

            ruta = string.Empty;
            await DisplayAlert("Confirmación", "Contacto actualizado correctamente", "Aceptar");
            await Navigation.PopAsync();

        }

        private async void BtnSeleccionarFoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No soporta fotografías", "No tiene permisos a fotos", "Aceptar");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });
            if (file == null)
            {
                return;
            }

            ruta = file.Path;
            Foto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });            
        }

        private async void BtnTomarFoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camara", "No tiene acceso a camara", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "Sample",
                Name = "img.jpg"
            });
            if (file == null)
            {
                return;
            }

            ruta = file.Path;            

            Foto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}

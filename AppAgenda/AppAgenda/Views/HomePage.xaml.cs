using AppAgenda.Cells;
using AppAgenda.Data;
using Plugin.Media;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        string ruta;        
        public HomePage()
        {
            InitializeComponent();

            //Se agrega padding personalizadopara cada plataforma. 
            this.Padding = Device.OnPlatform(
                new Thickness(10, 20, 10, 10),
                new Thickness(10),
                new Thickness(10));

            //Personalizamos nuestro listview
            lsvFoto.ItemTemplate = new DataTemplate(typeof(ContactoCell));
            lsvFoto.RowHeight = 70;

            btnAgregar.Clicked += BtnAgregar_Clicked;
            btnTomarFoto.Clicked += BtnTomarFoto_Clicked;
            btnSeleccionarFoto.Clicked += BtnSeleccionarFoto_Clicked;

            lsvFoto.ItemSelected += LsvFoto_ItemSelected;            

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Cargar la lista de contactos al inicar la aplicación
            using (var datos = new DataAccess())
            {
                lsvFoto.ItemsSource = datos.GetContactos();
            }
        }
        private async void LsvFoto_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new EditPage((Contacto)e.SelectedItem));
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
                Directory = "Agenda",
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

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            //Validaciones para que los campos estén llenados
            if (string.IsNullOrEmpty(ruta))
            {
                await DisplayAlert("Error", "Debe ingresar una Imágen", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                await DisplayAlert("Error","Debe ingresar un Nombre", "Aceptar");
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
                txtTelefono.Focus();
                return;
            }

            ruta = ruta.Replace(" ", "");
            var contacto = new Contacto
            {
                Imagen = ruta,
                Nombre = txtNombre.Text,
                Correo = txtCorreo.Text,
                Telefono = txtTelefono.Text
            };
            using (var datos = new DataAccess())
            {
                datos.InsertarContacto(contacto);
                lsvFoto.ItemsSource = datos.GetContactos();
            }
            //Limpiamos los campos.  
            Foto.Source = "";
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            ruta = string.Empty;
            await DisplayAlert("Confirmación", "Contacto guardado", "Aceptar");
        }
    }
}

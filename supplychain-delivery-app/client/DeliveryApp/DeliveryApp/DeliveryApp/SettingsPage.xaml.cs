using DeliveryApp.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeliveryApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            truckIdEntry.Text = SimpleIoc.Default.GetInstance<ISessionState>().TruckId;
            datePicker.Date = Convert.ToDateTime(SimpleIoc.Default.GetInstance<ISessionState>().Date) ;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            SimpleIoc.Default.GetInstance<ISessionState>().TruckId = truckIdEntry.Text;
            SimpleIoc.Default.GetInstance<ISessionState>().Date = datePicker.Date.ToString("yyyy-MM-dd");
            await Navigation.PopModalAsync(true);
        }
    }
}
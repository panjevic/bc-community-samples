using DeliveryApp.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace DeliveryApp
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryPageDetail : ContentPage
    {
        ZXingScannerPage scanPage;

        public DeliveryPageDetail()
        {
            InitializeComponent();
            BindingContext = SimpleIoc.Default.GetInstance<DeliveryDetailViewModel>();
        }

        // TODO Implemented in code-behind for the sake of brevity. Should use proper MVVM Navigation Service
        private async void OnSignClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignaturePage(), true);
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                SimpleIoc.Default.GetInstance<DeliveryDetailViewModel>().Barcode = result.Text;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopModalAsync(true);
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            await Navigation.PushModalAsync(scanPage, true);
        }
    }
}
using SignaturePad.Forms;
using System;
using DeliveryApp.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeliveryApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignaturePage : ContentPage
    {
        public SignaturePage()
        {
            InitializeComponent();
        }

        // TODO Implemented in code-behind for the sake of brevity. Should use proper MVVM Navigation Service
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            SimpleIoc.Default.GetInstance<DeliveryDetailViewModel>().SignatureStream = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
            await Navigation.PopModalAsync(true);
        }
    }
}
using System;
using DeliveryApp.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace DeliveryApp
{
    public partial class DeliveryPageMaster : ContentPage
    {
        public ListView ListView;

        public DeliveryPageMaster()
        {
            InitializeComponent();

            BindingContext = SimpleIoc.Default.GetInstance<DeliveryPageMasterViewModel>();
            ListView = MenuItemsListView;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingsPage(), true);
        }
    }
}
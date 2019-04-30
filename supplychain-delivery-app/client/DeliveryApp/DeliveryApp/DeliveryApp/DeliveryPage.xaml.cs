using System;
using DeliveryApp.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace DeliveryApp
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryPage : MasterDetailPage
    {
        public DeliveryPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            IsPresented = true;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as DeliveryPageMenuItem;
            if (item == null)
            {
                return;
            }
            
            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
            SimpleIoc.Default.GetInstance<DeliveryDetailViewModel>().SelectedDelivery = item.Delivery;
            
            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}
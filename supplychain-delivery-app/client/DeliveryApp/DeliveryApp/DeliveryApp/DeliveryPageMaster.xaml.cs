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
    }
}
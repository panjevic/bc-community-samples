using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeliveryApp.Model;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DeliveryApp.ViewModel
{
    public class DeliveryPageMasterViewModel : ViewModelBase
    {
        private HttpClient client;
        private bool isRefreshing;
        private Command refreshCommand;
        private string status;
        private readonly ISessionState sessionState;

        public ObservableCollection<DeliveryPageMenuItem> MenuItems { get; set; } = new ObservableCollection<DeliveryPageMenuItem>();

        public DeliveryPageMasterViewModel(ISessionState sessionState)
        {
            this.sessionState = sessionState;
            this.OnRefresh();
        }

        private HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient();
                }

                return client;
            }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { this.Set(() => IsRefreshing, ref isRefreshing, value); }
        }

        public string Status
        {
            get => status;
            set => Set(() => Status, ref status, value);
        }

        public Command RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(async () =>
                    {
                        IsRefreshing = true;
                        await OnRefresh();
                        IsRefreshing = false;
                    });
                }
                return refreshCommand;
            }
        }

        private async Task<List<DeliveryAttestation>> RetrieveEntriesFromServer()
        {
            var result = new List<DeliveryAttestation>();
            Status = "Please wait...";

            try
            {
                var fullUrl = Settings.GetDeliveriesUrl + $"?truckId={sessionState.TruckId}&date={sessionState.Date}";
                var responseBody = await Client.GetStringAsync(fullUrl);
                result = JsonConvert.DeserializeObject<List<DeliveryAttestation>>(responseBody);
                Status = "Retrieved " + DateTime.Now.ToString("t");
            }
            catch (Exception exception)
            {
                Status = "Error retrieving the info";
            }

            return result;
        }

        private async Task OnRefresh()
        {
            MenuItems.Clear();
            var result = await RetrieveEntriesFromServer();
            foreach (var entry in result.OrderByDescending(e => e.Timeslot))
            {
                MenuItems.Add(new DeliveryPageMenuItem(entry));
            }
        }
    }
}
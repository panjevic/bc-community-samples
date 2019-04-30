using GalaSoft.MvvmLight;
using System;

namespace DeliveryApp.ViewModel
{
    public class SessionState : ViewModelBase, ISessionState
    {
        public string TruckId { get; set; }

        public string Date { get; set; }

        public SessionState()
        {
            TruckId = "123";
            Date = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}

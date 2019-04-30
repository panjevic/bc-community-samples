using System;
using DeliveryApp.Model;
using Xamarin.Forms;

namespace DeliveryApp.ViewModel
{

    public class DeliveryPageMenuItem
    {
        private DeliveryAttestation entry;

        public DeliveryPageMenuItem()
        {
            TargetType = typeof(DeliveryPageDetail);
        }

        public DeliveryPageMenuItem(DeliveryAttestation entry) : this()
        {
            this.entry = entry;
            this.Id = entry.DeliveryId;
            this.Title = entry.DeliveryId.ToString();
            this.Address = $"{entry.Address} {entry.City} {entry.State} {entry.Zipcode}";
            this.Timeslot = entry.Timeslot;
        }

        public DeliveryAttestation Delivery
        {
            get { return entry; }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public string Timeslot { get; set; }

        public Type TargetType { get; set; }
    }
}
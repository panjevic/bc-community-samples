namespace DeliveryApp.Model
{
    public class DeliveryAttestation
    {
        public int DeliveryId { get; set; }

        public string Barcode { get; set; }

        public string Date { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Zipcode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Phone { get; set; }

        public string PhotoUri { get; set; }

        public string SignatureUri { get; set; }

        public string TruckId { get; set; }

        public string Timeslot { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }
    }
}

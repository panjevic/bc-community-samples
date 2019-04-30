using System;
using DeliveryApp.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace Delivery.Functions
{
    public class DeliveryEntity : TableEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Zipcode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Phone { get; set; }

        public string Barcode { get; set; }

        public string PhotoUri { get; set; }

        public string SignatureUri { get; set; }

        public string TruckId { get; set; }

        public string Timeslot { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string DeliveryStatus { get; set; }

        public DeliveryEntity() { }

        public DeliveryEntity(DeliveryAttestation model)
        {
            this.PartitionKey = model.Date;
            this.Name = model.Name;
            this.Phone = model.Phone;
            this.Address = model.Address;
            this.City = model.City;
            this.State = model.State;
            this.Zipcode = model.Zipcode;
            this.RowKey = model.DeliveryId.ToString();
            this.DeliveryStatus = model.DeliveryStatus.ToString();
            this.Latitude = model.Latitude;
            this.Longitude = model.Longitude;
            this.PhotoUri = model.PhotoUri;
            this.SignatureUri = model.SignatureUri;
            this.TruckId = model.TruckId;
            this.Timeslot = model.Timeslot;
            this.Barcode = model.Barcode;
        }

        internal DeliveryAttestation ToDeliveryAttestation()
        {
            return new DeliveryAttestation()
            {
                Date = this.PartitionKey,
                Name = this.Name,
                Phone = this.Phone,
                Address = this.Address,
                City = this.City,
                State = this.State,
                Zipcode = this.Zipcode,
                DeliveryId = Convert.ToInt32(this.RowKey),
                DeliveryStatus = this.DeliveryStatus == null? DeliveryApp.Model.DeliveryStatus.Scheduled : Enum.Parse<DeliveryStatus>(this.DeliveryStatus),
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                PhotoUri = this.PhotoUri,
                SignatureUri = this.SignatureUri,
                TruckId = this.TruckId,
                Timeslot = this.Timeslot,
                Barcode = this.Barcode
            };
        }
    }
}

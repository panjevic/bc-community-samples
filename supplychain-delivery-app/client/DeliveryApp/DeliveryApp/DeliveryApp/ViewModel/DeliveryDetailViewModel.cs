using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DeliveryApp.Model;
using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Plugin.ExternalMaps;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DeliveryApp.ViewModel
{
    public class DeliveryDetailViewModel : ViewModelBase
    {
        private Command acquireCommand;
        private MediaFile photo;
        private bool isBusy;
        private string status;
        private Command uploadCommand;
        private Command locateCommand;
        private Command navigateCommand;
        private Command notifyCommand;
        private DeliveryAttestation selectedDelivery;
        private HttpClient client;
        private string barcode;
        private double latitude;
        private double longitude;
        private bool showDetails;
        private readonly ISessionState sessionState;

        public DeliveryDetailViewModel(ISessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        public Command AcquireCommand
        {
            get
            {
                if (acquireCommand == null)
                {
                    acquireCommand = new Command(async () => await OnAcquireCommand());
                }
                return acquireCommand;
            }
        }

        public Command UploadCommand
        {
            get
            {
                if (uploadCommand == null)
                {
                    uploadCommand = new Command(async () => await OnUploadCommand());
                }
                return uploadCommand;
            }
        }

        public Command LocateCommand
        {
            get
            {
                if (locateCommand == null)
                {
                    locateCommand = new Command(async () => await OnLocateCommand());
                }
                return locateCommand;
            }
        }

        public Command NavigateCommand
        {
            get
            {
                if (navigateCommand == null)
                {
                    navigateCommand = new Command(async () => await OnNavigateCommand());
                }
                return navigateCommand;
            }
        }

        public Command NotifyCommand
        {
            get
            {
                if (notifyCommand == null)
                {
                    notifyCommand = new Command(async () => await OnNotifyCommand());
                }
                return notifyCommand;
            }
        }

        public DeliveryAttestation SelectedDelivery
        {
            get => selectedDelivery;
            set
            {
                this.Set(() => SelectedDelivery, ref selectedDelivery, value);
                if (value != null)
                {
                    ShowDetails = true;
                }
            }
        }

        public bool IsBusy { get => isBusy; set => this.Set(() => this.IsBusy, ref isBusy, value); }

        public bool ShowDetails { get => showDetails; set => this.Set(() => this.ShowDetails, ref showDetails, value); }

        public string Status { get => status; set => this.Set(() => Status, ref status, value); }

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

        public Stream SignatureStream { get; set; }

        public string Barcode { get => barcode; set => Set(() => Barcode, ref barcode, value); }

        public double Latitude { get => latitude; set => Set(() => Latitude, ref latitude, value); }

        public double Longitude { get => longitude; set => Set(() => Longitude, ref longitude, value); }

        private async Task OnNotifyCommand()
        {
            var messageText = $"Your delivery {SelectedDelivery.DeliveryId} is on its way  to {SelectedDelivery.Name} {SelectedDelivery.Address} {SelectedDelivery.City} {SelectedDelivery.State} {SelectedDelivery.Zipcode}";
            await this.SendSms(messageText, SelectedDelivery.Phone);
        }

        private async Task OnNavigateCommand()
        {
            var success = await CrossExternalMaps.Current.NavigateTo(SelectedDelivery.Name, SelectedDelivery.Address, SelectedDelivery.City, SelectedDelivery.State, SelectedDelivery.Zipcode, "USA", "USA");
        }

        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private async Task OnLocateCommand()
        {
            IsBusy = true;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Status = "GPS position acquired";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnUploadCommand()
        {
            if (photo == null)
            {
                Status = "You need to take a photo of the delivery";
                return; //TODO Throw exception
            }
            else
            {
                var uri = await UploadImage(photo.GetStream(), "images", $"{SelectedDelivery.DeliveryId}.png");
                Status = $"Uploaded photo {uri}";
                SelectedDelivery.PhotoUri = uri;
            }

            if (SignatureStream == null)
            {
                Status = "You need to collect a signature from the customer";
                return; //TODO Throw exception
            }
            else
            {
                var uri = await UploadImage(SignatureStream, "signatures", $"{SelectedDelivery.DeliveryId}.png");
                Status = $"Uploaded signature {uri}";
                SelectedDelivery.SignatureUri = uri;
            }

            SelectedDelivery.Barcode = Barcode;
            SelectedDelivery.Longitude = Longitude;
            SelectedDelivery.Latitude = Latitude;
            SelectedDelivery.DeliveryStatus = DeliveryStatus.Delivered;

            await SaveToServer();
        }

        private async Task OnAcquireCommand()
        {
            var init = await CrossMedia.Current.Initialize();
            if (!init)
            {
                Status = "ERROR: Unable to initialize camera!";
                return; // TODO Throw exception
            }

            if (!CrossMedia.Current.IsCameraAvailable && !CrossMedia.Current.IsTakePhotoSupported)
            {
                Status = "ERROR: Taking photos not supported on this device!";
                return; // TODO Throw exception
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Deliveries",
                Name = $"{SelectedDelivery.DeliveryId}.png"
            });

            if (file == null)
            {
                return;
            }

            photo = file;
            Status = $"Photo saved to {photo.Path}";
        }

        private async Task<string> UploadImage(Stream stream, string containerName, string name)
        {
            IsBusy = true;

            try
            {
                var account = CloudStorageAccount.Parse(Settings.StorageConnectionString);
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                var blockBlob = container.GetBlockBlobReference(name);
                await blockBlob.UploadFromStreamAsync(stream);

                return blockBlob.Uri.AbsolutePath;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                IsBusy = false;
            }

            return null;
        }

        private async Task<bool> SaveToServer()
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(SelectedDelivery);
                var response = await Client.PostAsync(Settings.CompleteDeliveryUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Status = $"Saved {DateTime.Now.ToString("t")}";
                    return true;
                }
                else
                {
                    Status = "Please check your network connectivity and try again.";
                }
            }
            catch (Exception exception)
            {
                Status = "Error while saving!";
            }

            return false;
        }
    }
}

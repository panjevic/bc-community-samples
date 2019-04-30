namespace DeliveryApp
{
    public class Settings
    {
        public const string GetDeliveriesUrl = "https://attesteddelivery.azurewebsites.net/api/GetMyDeliveries";
        public const string CompleteDeliveryUrl = "https://prod-56.westeurope.logic.azure.com:443/workflows/d966815e2277494d8eb169af3fb45212/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=PUTycYDoGCbhKCJMTdjCvSiyLfEqkMGpMOL3jSCs7vA";

        public const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=deliverystorage;AccountKey=WXtf9cXHS26SSd3JpxeIaW5RXWIO2RvC75h+j3N/K2AJY49JYZeNIwbrwzSZ8hzQfx7P1crMaPiMJNOSlZqtLA==;EndpointSuffix=core.windows.net";
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using DeliveryApp.Model;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;

namespace Delivery.Functions
{
    public static class DeliveryFunctions
    {
        [FunctionName("GetMyDeliveries")]
        public static async Task<HttpResponseMessage> GetMyDeliveries(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [Table("ScheduledDeliveries")] CloudTable cloudTable,
            ILogger log)
        {
            var truckId = req.Query["truckId"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(truckId))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var date = DateTime.Today.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(req.Query["date"]))
            {
                date = Convert.ToDateTime(req.Query["date"]).ToString("yyyy-MM-dd");
            }

            var rangeQuery = new TableQuery<DeliveryEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, date),
                TableOperators.And,
                    TableQuery.GenerateFilterCondition("TruckId", QueryComparisons.Equal, truckId)));
            var results = await cloudTable.ExecuteQuerySegmentedAsync(rangeQuery, null);

            var deliveryAttestations = new List<DeliveryAttestation>();
            foreach (var entity in results)
            {
                deliveryAttestations.Add(entity.ToDeliveryAttestation());
            }

            var result = JsonConvert.SerializeObject(deliveryAttestations);
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(result) };
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;
using System.Text;

namespace HeroMarketplace
{
    public static class HeroMarketplaceFunctions
    {
        public class Hero
        {
            public int TokenId { get; set; }

            public string Name { get; set; }

            public int Wisdom { get; set; }

            public int Inteligence { get; set; }

            public int Charisma { get; set; }

            public int Speed { get; set; }

            public int Accuracy { get; set; }

            public int Might { get; set; }
        }
        
        public static Random Random = new Random();
        public static int MinAbility = 5;
        public static int MaxAbility = 20;


        [FunctionName("GenerateCharacteristics")]
        public static HttpResponseMessage GenerateCharacteristics(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
                ILogger log)
        {
            var hero = new Hero()
            {
                Name = "Hero",
                Accuracy = Random.Next(MinAbility, MaxAbility),
                Charisma = Random.Next(MinAbility, MaxAbility),
                Inteligence = Random.Next(MinAbility, MaxAbility),
                Speed = Random.Next(MinAbility, MaxAbility),
                Wisdom = Random.Next(MinAbility, MaxAbility),
                Might = Random.Next(MinAbility, MaxAbility)
            };

            var result = JsonConvert.SerializeObject(hero);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(result, Encoding.UTF8, "application/json")
            };
        }
        
        [FunctionName("Offers")]
        public static async Task<HttpResponseMessage> GetOffers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [Table("Offers")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("GetOffers HTTP trigger function");

            TableQuery<OfferEntity> query = new TableQuery<OfferEntity>();

            var offerEntities = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var offerList = offerEntities.Select(e => new Offer(e));
            var result = JsonConvert.SerializeObject(offerList);
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(result) };
        }

        [FunctionName("UserOffers")]
        public static async Task<HttpResponseMessage> GetUserOffers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Offers")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("GetOffers HTTP trigger function");

            var user = req.Query["user"];
            TableQuery<OfferEntity> query = new TableQuery<OfferEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, user)); ;

            var offerEntities = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var offerList = offerEntities.Select(e => new Offer(e));
            var result = JsonConvert.SerializeObject(offerList);
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(result) };
        }

        public class OfferEntity : TableEntity
        {
            public string Price { get; set; }
        }

        public class Offer
        {
            public string hero { get; set; }

            public double price { get; set; }

            public string seller { get; set; }

            public Offer(OfferEntity entity)
            {
                hero = entity.RowKey;
                seller = entity.PartitionKey;
                price = Convert.ToDouble(entity.Price);
            }
        }
    }
}

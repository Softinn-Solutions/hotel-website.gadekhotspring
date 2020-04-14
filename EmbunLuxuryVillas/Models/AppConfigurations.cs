using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbunLuxuryVillas.Models
{
    public class AppConfigurations
    {
        public string StorageConnectionString { get; set; }
        public string AzureTableReference { get; set; }
        public string AzureTableReferenceLocal { get; set; }
        public string LiteDbTablePartitionKey { get; set; }
        public string PrivateKey { get; set; }
        public int HotelId { get; set; }
    }
}

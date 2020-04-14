using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbunLuxuryVillas.Models
{
    public class DatabaseIndex : TableEntity
    {
        public DatabaseIndex() { }
        public DatabaseIndex(string hotelId, string guid)
        {
            this.PartitionKey = hotelId;
            this.RowKey = guid;
        }

        public string DatabaseFileName { get; set; }
    }
}

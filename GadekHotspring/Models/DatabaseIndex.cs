using Microsoft.WindowsAzure.Storage.Table;

namespace GadekHotspring.Models
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

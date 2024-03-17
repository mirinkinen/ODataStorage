using Azure;
using Azure.Data.Tables;

namespace ODataStorage;

public class Customer : ITableEntity
{
    public string? PartitionKey { get; set; }

    public string? RowKey { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
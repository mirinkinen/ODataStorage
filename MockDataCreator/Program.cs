// See https://aka.ms/new-console-template for more information

using Azure.Data.Tables;
using ODataStorage;

var client = new TableServiceClient("UseDevelopmentStorage=true");

var tableClient = client.GetTableClient("Customers");
await tableClient.CreateIfNotExistsAsync();

var totalCount = 0;
var numberOfBatches = 100;
var numberOfEntitiesPerBatch = 100;

for (var i = 0; i < numberOfBatches; i++)
{
    var transactionActions = new List<TableTransactionAction>();

    for (var j = 0; j < numberOfEntitiesPerBatch; j++)
    {
        var customer = new Customer
        {
            PartitionKey = j.ToString(),
            RowKey = i.ToString(),
            FirstName = "First" + i,
            LastName = "Last" + j,
            Timestamp = DateTimeOffset.UtcNow
        };

        transactionActions.Add(new TableTransactionAction(TableTransactionActionType.Add, customer));
    }

    await tableClient.SubmitTransactionAsync(transactionActions);
    transactionActions.Clear();

    totalCount += numberOfEntitiesPerBatch;

    Console.WriteLine("Entities created: " + totalCount);
}

Console.ReadKey();
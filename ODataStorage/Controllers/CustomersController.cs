using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ODataStorage.Controllers;

public class CustomersController : ODataController
{
    private readonly TableServiceClient _client;
    private readonly TableClient _tableClient;

    public CustomersController()
    {
        _client = new TableServiceClient("UseDevelopmentStorage=true");

        _tableClient = _client.GetTableClient("Customers");
    }

    [EnableQuery(PageSize = 5)]
    public IQueryable<Customer> Get()
    {
        return _tableClient.Query<Customer>().AsQueryable();
    }

    public async Task<ActionResult<Customer?>> Get([FromODataUri] string keyPartitionKey,
        [FromODataUri] string keyRowKey)
    {
        var customer = await _tableClient.GetEntityIfExistsAsync<Customer>(keyPartitionKey, keyRowKey);

        if (customer.HasValue)
        {
            return Ok(customer.Value!);
        }

        return NotFound();
    }
}
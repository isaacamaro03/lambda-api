using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lambda_api.Controllers;

[ApiController]
[Route("customer")]
public class CalculatorController : ControllerBase
{
    private readonly IAmazonDynamoDB _dynamoDB;

    public CalculatorController(IAmazonDynamoDB dynamoDB)
    {
        _dynamoDB = dynamoDB;
    }

    [HttpPost]
    public async Task<bool> AddCustomer(CustomerWrite customerWrite)
    {
        Customer customer = new Customer 
        { 
            Name = customerWrite.Name, 
            Id = Guid.NewGuid().ToString() 
        };

        var customerAsJson = JsonSerializer.Serialize(customer);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttribute = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = "minimal-table",
            Item = itemAsAttribute
        };

        var response = await _dynamoDB.PutItemAsync(createItemRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    [HttpGet("{id}")]
    public async Task<Customer?> ReadCustomer(string id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = "minimal-table",
            Key = new Dictionary<string, AttributeValue>
            {
                ["pk"] = new AttributeValue { S = id },
                ["sk"] = new AttributeValue { S = id }
            }
        };

        var response = await _dynamoDB.GetItemAsync(getItemRequest);

        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<Customer>(itemAsDocument.ToJson());
    }
}

public class CustomerWrite 
{
    public string Name { get; set; } = default!;
}

public class Customer
{
    [JsonPropertyName("pk")]
    public string Pk => Id;

    [JsonPropertyName("sk")]
    public string Sk => Pk;

    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
}
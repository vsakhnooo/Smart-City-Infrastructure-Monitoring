using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TrafficController : ControllerBase
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public TrafficController(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        var database = _cosmosClient.GetDatabase("SmartCity");
        _container = database.GetContainer("Sensors");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrafficData>>> GetTrafficData(DateTime? startDate, DateTime? endDate)
    {
        var query = "SELECT * FROM c WHERE c.sensor_id = 'TS001'";

        if (startDate.HasValue && endDate.HasValue)
        {
            query += $" AND c.timestamp >= '{startDate.Value:yyyy-MM-dd}' AND c.timestamp <= '{endDate.Value:yyyy-MM-dd}'";
        }

        var iterator = _container.GetItemQueryIterator<TrafficData>(query);
        List<TrafficData> results = new List<TrafficData>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return Ok(results);
    }
}

public class TrafficData
{
    public string sensor_id { get; set; }
    public string road_name { get; set; }
    public string timestamp { get; set; }
    public int vehicle_count { get; set; }
    public double average_speed { get; set; }
}

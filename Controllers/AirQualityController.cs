using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AirQualityController : ControllerBase
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public AirQualityController(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        var database = _cosmosClient.GetDatabase("SmartCity");
        _container = database.GetContainer("Sensors");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AirQualityData>>> GetAirQualityData(DateTime? startDate, DateTime? endDate)
    {
        var query = "SELECT * FROM c WHERE c.sensor_id = 'AQ001'";

        if (startDate.HasValue && endDate.HasValue)
        {
            query += $" AND c.timestamp >= '{startDate.Value:yyyy-MM-dd}' AND c.timestamp <= '{endDate.Value:yyyy-MM-dd}'";
        }

        var iterator = _container.GetItemQueryIterator<AirQualityData>(query);
        List<AirQualityData> results = new List<AirQualityData>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return Ok(results);
    }
}

public class AirQualityData
{
    public string sensor_id { get; set; }
    public string location { get; set; }
    public string timestamp { get; set; }
    public double pm2_5 { get; set; }
    public double pm10 { get; set; }
    public int co2 { get; set; }
}

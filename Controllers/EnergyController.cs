using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class EnergyController : ControllerBase
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public EnergyController(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        var database = _cosmosClient.GetDatabase("SmartCity");
        _container = database.GetContainer("Sensors");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnergyData>>> GetEnergyData(DateTime? startDate, DateTime? endDate)
    {
        var query = "SELECT * FROM c WHERE c.sensor_id = 'EN001'";

        if (startDate.HasValue && endDate.HasValue)
        {
            query += $" AND c.timestamp >= '{startDate.Value:yyyy-MM-dd}' AND c.timestamp <= '{endDate.Value:yyyy-MM-dd}'";
        }

        var iterator = _container.GetItemQueryIterator<EnergyData>(query);
        List<EnergyData> results = new List<EnergyData>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return Ok(results);
    }
}

public class EnergyData
{
    public string sensor_id { get; set; }
    public string building_name { get; set; }
    public string timestamp { get; set; }
    public double energy_consumed { get; set; }
    public double power_usage { get; set; }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Получение строки подключения из конфигурации
var cosmosDbSettings = builder.Configuration.GetSection("CosmosDb");
string cosmosDbUri = cosmosDbSettings["Uri"]; // Теперь URI
string cosmosDbKey = cosmosDbSettings["Key"]; // По-прежнему используем ключ
string databaseName = cosmosDbSettings["DatabaseName"];
string containerName = cosmosDbSettings["ContainerName"];

// Добавление сервиса CosmosClient с использованием URI
builder.Services.AddSingleton(new CosmosClient(cosmosDbUri, cosmosDbKey));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Добавление контроллеров
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment()) // Включаем Swagger только в режиме разработки
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Интерфейс для просмотра
}
app.UseCors("AllowAllOrigins");
app.UseAuthorization();


app.MapControllers();

app.Run();

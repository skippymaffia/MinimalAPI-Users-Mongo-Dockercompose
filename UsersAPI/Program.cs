using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
var databaseName = builder.Configuration.GetValue(typeof(string), "MongoDBCurrentDbName")!.ToString();
var tableName = builder.Configuration.GetValue(typeof(string), "MongoDBCurrentTableName")!.ToString();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.FillMongoDbAsync(connectionString!, databaseName!, tableName!);

app.AddEndpoints(connectionString!, databaseName!, tableName!);

app.Run();

public partial class Program { }
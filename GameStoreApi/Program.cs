using System.Data.Common;
using GameStoreApi;
using GameStoreApi.Dtos;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreDBContext>(connString);

var app = builder.Build();

// dependency injection
app.GetGameEndPoints();


await app.MigrateDBAsync();
app.Run();

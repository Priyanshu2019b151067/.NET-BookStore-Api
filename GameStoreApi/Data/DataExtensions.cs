using Microsoft.EntityFrameworkCore;

namespace GameStoreApi;

public static class DataExtensions
{
    public static async Task MigrateDBAsync(this WebApplication app){
        using var scope = app.Services.CreateScope();
        var DbContext = scope.ServiceProvider.GetRequiredService<GameStoreDBContext>();
        await DbContext.Database.MigrateAsync();
    }
}

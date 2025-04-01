using CommandsServeice.Models;
using CommandsServeice.SyncDataServices.Grpc;

namespace CommandsServeice.Data;

public static class PrepDB
{
    public static async Task PrepPopulation(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var repository = serviceScope.ServiceProvider.GetService<ICommandsRepository>();

        var platformDataClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

        var platforms = await platformDataClient.ReturnAllPlatforms();

        await SeedData(repository, platforms);

    }

    private static async Task SeedData(ICommandsRepository repository, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding new platforms...");

        List<Platform> newPlatforms = [];

        foreach (var platform in platforms)
            if (!await repository.ExternalPlatformExists(platform.ExternalId))
                newPlatforms.Add(platform);

        await repository.CreatePlatforms(newPlatforms);
    }
}

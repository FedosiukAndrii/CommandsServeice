using AutoMapper;
using CommandsServeice.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsServeice.SyncDataServices.Grpc;

public class PlatformDataClient(IConfiguration configuration, IMapper mapper) : IPlatformDataClient
{
    public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
    {
        Console.WriteLine("--> Attempting to call gRPC Service to get Platforms");

        var channel = GrpcChannel.ForAddress(configuration["GrpcPlatform"]);

        var client = new GrpcPlatform.GrpcPlatformClient(channel);

        var request = new GetAllRequest();

        try
        {
            var reply = await client.GetAllPlatformsAsync(request);

            return mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not gRPC Server {ex.Message}");

            return null;
        }
    }
}

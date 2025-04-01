using CommandsServeice.Models;

namespace CommandsServeice.SyncDataServices.Grpc;

public interface IPlatformDataClient
{
    Task<IEnumerable<Platform>> ReturnAllPlatforms();
}

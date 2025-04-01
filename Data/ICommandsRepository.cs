using CommandsServeice.Models;

namespace CommandsServeice.Data;

public interface ICommandsRepository
{
    #region Platforms
    Task<IEnumerable<Platform>> GetAllPlatforms();

    Task CreatePlatform(Platform platform);
    Task CreatePlatforms(IEnumerable<Platform> platforms);

    Task<bool> PlatformExists(int platformId);
    Task<bool> ExternalPlatformExists (int externalPlatformId);
    #endregion

    #region Commands
    Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId);

    Task<Command> GetCommand(int platformId, int commandId);

    Task CreateCommand(int platformId, Command command);
    #endregion
}

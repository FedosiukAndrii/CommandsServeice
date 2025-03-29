using CommandsServeice.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsServeice.Data;

public class CommandRepository(AppDbContext db) : ICommandsRepository
{
    public async Task CreateCommand(int platformId, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        command.PlatformId = platformId;

        await db.Commands.AddAsync(command);

        await db.SaveChangesAsync();
    }

    public async Task CreatePlatform(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);

        await db.Platforms.AddAsync(platform);

        await db.SaveChangesAsync();
    }

    public Task<bool> ExternalPlatformExists(int externalPlatformId) => db.Platforms.AnyAsync(p => p.ExternalId == externalPlatformId);

    public async Task<IEnumerable<Platform>> GetAllPlatforms() => await db.Platforms.ToListAsync();

    public async Task<Command> GetCommand(int platformId, int commandId) => await db.Commands
        .FirstOrDefaultAsync(c => c.PlatformId == platformId && c.Id == commandId);

    public async Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId) => await db.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.CommandLine)
            .ToListAsync();

    public Task<bool> PlatformExists(int platformId) => db.Platforms.AnyAsync(p => p.Id == platformId);
}

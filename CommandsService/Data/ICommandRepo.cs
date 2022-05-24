using System.Collections.Generic;
using CommandsService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
     

        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        bool PlatformExist(int platformId);

        bool ExternalPlatformExist(int ExternalID);
        IEnumerable<Command> GetCommandForPlatform(int platformId);

        Command getCommand(int platformId,int commandId);

        void CreateCommand(int platformId,Command command);
        
    }
    
}
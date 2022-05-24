
using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{

public interface IPlatformRepo{
    bool DataSaveChanges();

    IEnumerable<Platform> getAllPlatform();

    Platform GetPlatformById(int id);

    void CreatePlatform(Platform plat);
}
}
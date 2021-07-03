using Server.Data;

namespace Server.Features.Management.Services
{
    public class ManagementService : IManagementService
    {
        private readonly ServerDbContext _data;

        public ManagementService(ServerDbContext data)
        {
            _data = data;
        }

    }
}

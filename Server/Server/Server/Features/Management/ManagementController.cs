using Microsoft.AspNetCore.Authorization;
using Server.Features.Management.Services;

namespace Server.Features.Management
{
    [Authorize(Roles = "Admin")]
    public class ManagementController : ApiController
    {
        private readonly ManagementService _managementService;

        public ManagementController(ManagementService managementService)
        {
            _managementService = managementService;
        }

    }
}

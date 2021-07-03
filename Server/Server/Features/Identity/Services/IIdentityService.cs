using System.Collections.Generic;
using Server.Features.Identity.Models;

namespace Server.Features.Identity.Services
{
    public interface IIdentityService
    {
        public LoginResponseModel GenerateJwtToken(string userId, string userName, string appSecret, IEnumerable<string> roles);
    }
}

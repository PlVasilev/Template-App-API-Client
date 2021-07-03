﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Features.Identity.Models;

namespace Server.Features.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        public LoginResponseModel GenerateJwtToken(string userId, string userName, string appSecret, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(appSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
            };

            if (roles != null)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return new LoginResponseModel { Token = encryptedToken };
        }
    }
}

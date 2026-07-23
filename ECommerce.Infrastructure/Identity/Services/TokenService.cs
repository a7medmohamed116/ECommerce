using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    public class TokenService : ITokenService  //package jwtToken bearer
    {
        private readonly JwtSettings _jwtsetting;// the exist of TokenService depend on exist of jwtsettings ? 

        public TokenService(IOptions<JwtSettings> jwtOptions)// asp.net do binding for configuraion to read appsetting and convert it to object by ioptions
        {
            _jwtsetting = jwtOptions.Value;
        }

        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            //claims [payload]
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userId), //userId
                new Claim(ClaimTypes.Email,email),
                new Claim (ClaimTypes.Name,userName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //type /security algo [header] //register first in appsetting
            #region Imporatnt info
            //how to reach appsetting by helper class [JwtSettings] read from appsetting and store his data then register in program.cs then inject above in ctor with ioptions
            // why do not make tokenservice read the appsetting itself like Configuration["JwtSettings:Key"] cause we kill the Dependency Injection , Single Responsibility
            #endregion
            //secretKey
            var secKey = _jwtsetting.SecretKey;
            if (string.IsNullOrWhiteSpace(secKey)) 
                throw new InvalidOperationException("Secret Key Empty");
            if(secKey.Length < 20) 
                throw new InvalidOperationException("Secret Key Too Short");
            //jwt deal with secret key as Byte[]   // string => byte
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
            //type ,Algo signing credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Token
            var token = new JwtSecurityToken
                (
                    issuer: _jwtsetting.Issuer,
                    audience: _jwtsetting.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtsetting.ExpirationMinutes),
                    signingCredentials: credentials



                );
            return new JwtSecurityTokenHandler().WriteToken(token);



        }
    }


    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public int ExpirationMinutes { get; set; } = default!;
        public string Audience { get; set; } = default!;
    }
}

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Spire.License;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utils;

namespace WebSecurity
{
    public static class SecurityExtensions
    {
        public static JwtSecurityToken CreateToken(this List<Claim> claims, bool isApi = false)
        {
            Claim jti;
            Claim issuerClaim;
            Claim audienceClaim;
            Claim issuedAtTimeClaim;
            Claim expireTimeClaim;
            SecurityParam securityParam;
            SigningCredentials signingCredentials;
            SymmetricSecurityKey securityKey;
            DateTime now;
            DateTime expireTime;

            jti = claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti);
            securityParam = new SecurityParam { uniqueValue = jti.Value };
            issuerClaim = claims.SingleOrDefault(c => c.Type ==JwtRegisteredClaimNames.Iss)!;
            audienceClaim = claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)!;
            expireTimeClaim = claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)!;
            issuedAtTimeClaim = claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)!;

            if (issuerClaim == null)
            {
                throw new ArgumentException("Must include issuer claim");
            }

            if (audienceClaim == null)
            {
                throw new ArgumentException("Must include audience claim");
            }

            if (audienceClaim == null)
            {
                throw new ArgumentException("Must include audience claim");
            }

            if (issuedAtTimeClaim == null)
            {
                throw new ArgumentException("Must include issued at claim");
            }

            if (expireTimeClaim == null)
            {
                throw new ArgumentException("Must include expire time claim");
            }

            now = EpochTime.DateTime(long.Parse(issuedAtTimeClaim.Value));
            expireTime = EpochTime.DateTime(long.Parse(expireTimeClaim.Value));

            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerClaim.Value + ":" + audienceClaim.Value));
            signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            if (isApi)
            {
                claims.Remove(issuerClaim);
                claims.Remove(audienceClaim);
            }

            return new JwtSecurityToken(issuerClaim.Value, audienceClaim.Value, claims, now, expireTime, signingCredentials);
        }
    }
}

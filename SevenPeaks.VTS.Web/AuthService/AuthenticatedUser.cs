using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SevenPeaks.VTS.Infrastructure.Interfaces;

namespace SevenPeaks.VTS.Web.AuthService
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) == null
                ? null
                : httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            Username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name) == null
                ? null
                : httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
        }

        public string UserId { get; }
        public string Username { get; }
    }
}
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Services.Interfaces
{
    public interface IAuthenticateService
    {
        LoggedUser Authenticate(string username, string password);
    }
}

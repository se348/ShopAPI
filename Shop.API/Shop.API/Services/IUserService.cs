using Shop.DAL.models;

namespace Shop.API.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);

    }
}

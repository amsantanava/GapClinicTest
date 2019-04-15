using AppClinic.Models;

namespace AppClinic.BusinessLogic
{
    public interface IBusinessUser
    {
        User GetUserByCredentials(LoginRequest login);
    }
}
using System.Threading.Tasks;
using TrmWpfUserInterface.Models;

namespace TrmWpfUserInterface.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}
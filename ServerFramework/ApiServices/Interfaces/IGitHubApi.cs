using System.Threading.Tasks;
using ServerFramework.Dto;

namespace ServerFramework.ApiServices.Interfaces
{
    public interface IGitHubApi
    {
        Task<GitHubUserModel> GetUserAndRepos(string userName);
    }
}
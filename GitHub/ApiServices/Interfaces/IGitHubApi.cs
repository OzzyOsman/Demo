using System.Threading.Tasks;
using GitHubUserModel = GitHub.Dto.GitHubUserModel;

namespace GitHub.ApiServices.Interfaces
{
    public interface IGitHubApi
    {
        Task<GitHubUserModel> GetUserAndRepos(string userName);
    }
}
using System.Collections.Generic;

namespace ServerFramework.Dto
{
    public class GitHubUserModel
    {
        public GitHubUserModel()
        {
            Repos = new List<GitHubUserRepoModel>();
        }

        public long? Id { get; set; }

        public string Avatar_Url { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Login { get; set; }

        public List<GitHubUserRepoModel> Repos { get; set; }
    }
}
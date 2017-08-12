namespace ServerFramework.Dto
{
    public class GitHubUserRepoModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Html_Url { get; set; }

        public int Stargazers_Count { get; set; }
    }
}
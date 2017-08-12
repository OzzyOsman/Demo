using System.Net.Http;

namespace GitHub.ApiServices.Interfaces
{
    public interface IHttpClientOperations
    {
        HttpResponseMessage CallApi(string baseaddress, string apiMethod);
    }
}
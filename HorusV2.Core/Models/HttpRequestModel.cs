using System.Text;

namespace HorusV2.Core.Models;

public class HttpRequestModel
{
    public HttpRequestModel()
    {
        Headers = new Dictionary<string, string>();
        BaseAddress = string.Empty;
        RequestMethod = string.Empty;
        RequestUri = string.Empty;
        JsonContent = string.Empty;
    }

    public string BaseAddress { get; set; }
    public string RequestUri { get; set; }
    public string RequestMethod { get; set; }
    public string JsonContent { get; set; }
    public IDictionary<string, string> Headers { get; set; }

    public void AddBasicAuthentication(string username, string password)
    {
        byte[] encoded = Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}");

        string base64 = Convert.ToBase64String(encoded);

        Headers.Add("Authorization", $"Basic {base64}");
    }

    public void AddJwtAuthorization(string token)
    {
        Headers.Add("Authorization", $"jwt {token}");
    }
}
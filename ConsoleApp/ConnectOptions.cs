using System.Text;

namespace ConsoleApp
{
  public class ConnectOptions
  {
    public string AuthType { get; set; }
    public string Url { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string AppId { get; set; }
    public string RedirectUri { get; set; }
    public bool RequireNewInstance { get; set; }

    public ConnectOptions(string authType, string url, string username, string password, 
      bool requireNewInstance, string appId, string redirectUri)
    {
      AuthType = authType;
      Url = url;
      Username = username;
      Password = password;
      RequireNewInstance = requireNewInstance;
      AppId = appId;
      RedirectUri = redirectUri;
    }

    public string BuildConnectionString()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append($"AuthType={AuthType};");
      sb.Append($"Url={Url};");
      sb.Append($"Username={Username};");
      sb.Append($"Password={Password};");
      sb.Append($"RequireNewInstance={RequireNewInstance};");
      sb.Append($"AppId={AppId};");
      sb.Append($"RedirectUri={RedirectUri};");

      return sb.ToString();
    }
  }
}

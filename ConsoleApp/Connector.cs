using Microsoft.Xrm.Tooling.Connector;
using System;

namespace ConsoleApp
{
  public class Connector
  {
    public CrmServiceClient Service { get; private set; }

    /// <exception cref="Exception">Thrown when client can't connect to CRM</exception>
    public Connector()
    {
      ConnectOptions connectOptions = new ConnectOptions(
        "OAuth",
        Constants.ConnectOptions.URL,
        Constants.ConnectOptions.LOGIN,
        Constants.ConnectOptions.PASSWORD,
        true,
        "51f81489-12ee-4a9e-aaae-a2591f45987d",
        "app://58145B91-0C36-4500-8554-080854F2AC97");

      Service = new CrmServiceClient(connectOptions.BuildConnectionString());

      if (Service.LastCrmException != null)
      {
        throw Service.LastCrmException;
      }
    }
  }
}

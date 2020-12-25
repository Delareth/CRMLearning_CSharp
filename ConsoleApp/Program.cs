using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Net;

namespace ConsoleApp
{
  public class Program
  {
    private static Connector _Connector;
    private static Logger _Logger;

    static void Main(string[] args)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      _Logger = new Logger();

      try
      {
        _Connector = new Connector();
        _Logger.Info("Client succesful connected to CRM");
      }
      catch (Exception ex)
      {
        _Logger.Error("Can't connect to CRM", ex);
        throw;
      }

      ParseCommunications();

      ParseContacts();

      Console.ReadKey();
    }

    private static void ParseCommunications()
    {
      Communication.Parser communicationParser = new Communication.Parser(_Connector.Service);
      Contact.Updater contactUpdater = new Contact.Updater(_Connector.Service, _Logger);

      IEnumerable<Communication.Data> communicationsForUpdate = communicationParser.GetEntitiesForUpdate();

      foreach (Communication.Data data in communicationsForUpdate)
      {
        contactUpdater.Update(data.ConctactRef, data.Field, data.Value);
      }
    }

    private static void ParseContacts()
    {
      Communication.Creator communicationCreator = new Communication.Creator(_Connector.Service, _Logger);
      Contact.Parser contactParser = new Contact.Parser(_Connector.Service);

      DataCollection<Entity> contacts = contactParser.GetAllWithEmptyCommunicationRef();

      foreach (Entity entity in contacts)
      {
        string fullName = entity.GetAttributeValue<string>(Constants.Contact.Fields.NAME);
        Guid contactGuid = entity.GetAttributeValue<Guid>(Constants.Contact.Fields.CONTACTID);

        if (entity.Contains(Constants.Contact.Fields.PHONE))
        {
          string value = entity.GetAttributeValue<string>(Constants.Contact.Fields.PHONE);

          communicationCreator.Create(Communication.Type.Phone, value, fullName, contactGuid);
        }

        if (entity.Contains(Constants.Contact.Fields.EMAIL))
        {
          string value = entity.GetAttributeValue<string>(Constants.Contact.Fields.EMAIL);

          communicationCreator.Create(Communication.Type.Email, value, fullName, contactGuid);
        }
      }
    }
  }
}

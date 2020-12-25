using Microsoft.Xrm.Sdk;

namespace ConsoleApp.Contact
{
  public class Updater
  {
    private readonly IOrganizationService _Service;
    private readonly Logger _Logger;

    public Updater(IOrganizationService service, Logger logger)
    {
      _Service = service;
      _Logger = logger;
    }

    public void Update(EntityReference contactRef, string field, string value)
    {
      Entity contactToUpdate = new Entity(contactRef.LogicalName, contactRef.Id);
      contactToUpdate[field] = value;

      _Service.Update(contactToUpdate);

      _Logger.Info($"Updated contact entity with id: {contactRef.Id}");
    }
  }
}

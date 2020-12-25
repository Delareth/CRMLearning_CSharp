using Microsoft.Xrm.Sdk;

namespace ConsoleApp.Contact
{
  public class Updater
  {
    private readonly IOrganizationService _Service;

    public Updater(IOrganizationService service)
    {
      _Service = service;
    }

    public void Update(EntityReference contactRef, string field, string value)
    {
      Entity contactToUpdate = new Entity(contactRef.LogicalName, contactRef.Id);
      contactToUpdate[field] = value;

      _Service.Update(contactToUpdate);

      Program.Logger.Info($"Updated contact entity with id: {contactRef.Id}");
    }
  }
}

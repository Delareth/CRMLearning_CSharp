using Microsoft.Xrm.Sdk;
using System;

namespace ConsoleApp.Communication
{
  public class Creator
  {
    private readonly IOrganizationService _Service;

    public Creator(IOrganizationService service)
    {
      _Service = service;
    }

    public void NewEntity(Type type, string value, string fullName, Guid conctactGuid)
    {
      Entity entity = new Entity(Constants.Communication.ENTITYNAME);

      string fieldToInsert;

      switch (type)
      {
        case Type.Phone:
        {
          fieldToInsert = Constants.Communication.Fields.PHONE;
          break;
        }
        case Type.Email:
        {
          fieldToInsert = Constants.Communication.Fields.EMAIL;
          break;
        }
        default:
        {
          throw new Exception("Unknown communication type in Creator.NewEntity()");
        }
      }

      entity[Constants.Communication.Fields.NAME] = fullName;
      entity[Constants.Communication.Fields.MAIN] = type == Type.Phone;
      entity[Constants.Communication.Fields.TYPE] = new OptionSetValue((int)type);
      entity[fieldToInsert] = value;

      if (conctactGuid != Guid.Empty)
      {
        EntityReference conctactRef = new EntityReference(Constants.Contact.ENTITYNAME, conctactGuid);

        entity[Constants.Communication.Fields.CONTACTID] = conctactRef;
      }

      _Service.Create(entity);

      Program.Logger.Info($"Created new communication entity with name {fullName}");
    }
  }
}

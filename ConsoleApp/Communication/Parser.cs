using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Communication
{
  public class Parser
  {
    private readonly IOrganizationService _Service;

    public Parser(IOrganizationService service)
    {
      _Service = service;
    }

    /// <exception cref="Exception">Thrown when unknown communication type was finded</exception>
    public IEnumerable<Data> GetEntitiesForUpdate()
    {
      QueryExpression query = new QueryExpression(Constants.Communication.ENTITYNAME)
      {
        ColumnSet = new ColumnSet(
          Constants.Communication.Fields.CONTACTID,
          Constants.Communication.Fields.TYPE,
          Constants.Communication.Fields.PHONE,
          Constants.Communication.Fields.EMAIL),
        NoLock = true,
      };

      LinkEntity contactLink = query.AddLink(
        Constants.Contact.ENTITYNAME, 
        Constants.Communication.Fields.CONTACTID, 
        Constants.Contact.Fields.CONTACTID, 
        JoinOperator.Inner);

      contactLink.Columns = new ColumnSet(
        Constants.Contact.Fields.PHONE, 
        Constants.Contact.Fields.EMAIL);

      contactLink.EntityAlias = Constants.Communication.PARSERALIAS;

      query.Criteria.AddCondition(Constants.Communication.Fields.MAIN, ConditionOperator.Equal, true);

      EntityCollection result = _Service.RetrieveMultiple(query);

      return ParseEntities(result.Entities);
    }

    /// <exception cref="Exception">Thrown when unknown communication type was finded</exception>
    private IEnumerable<Data> ParseEntities(DataCollection<Entity> entities)
    {
      foreach (Entity entity in entities)
      {
        Type cType = (Type)entity.GetAttributeValue<OptionSetValue>(Constants.Communication.Fields.TYPE).Value;
        EntityReference conctactRef = entity.GetAttributeValue<EntityReference>(Constants.Communication.Fields.CONTACTID);

        string communicationField = "";
        string contactField = "";
        string updatingField = "";

        switch (cType)
        {
          case Type.Phone:
          {
            communicationField = Constants.Communication.Fields.PHONE;
            contactField = $"{Constants.Communication.PARSERALIAS}.{Constants.Contact.Fields.PHONE}";
            updatingField = Constants.Contact.Fields.PHONE;

            break;
          }
          case Type.Email:
          {
            communicationField = Constants.Communication.Fields.EMAIL;
            contactField = $"{Constants.Communication.PARSERALIAS}.{Constants.Contact.Fields.EMAIL}";
            updatingField = Constants.Contact.Fields.EMAIL;

            break;
          }
          default:
          {
            throw new Exception("Unknown communication type");
          }
        }

        string newValue = entity.GetAttributeValue<string>(communicationField);
        string currentValue = "";

        if (entity.Contains(contactField))
        {
          currentValue = (string)(entity.GetAttributeValue<AliasedValue>(contactField).Value);
        }

        if (currentValue == newValue) continue;

        yield return new Data(conctactRef, updatingField, newValue);
      }
    }
  }
}

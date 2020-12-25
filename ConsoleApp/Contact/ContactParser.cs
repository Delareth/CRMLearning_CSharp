using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ConsoleApp.Contact
{
  public class Parser
  {
    private readonly IOrganizationService _Service;

    public Parser(IOrganizationService service)
    {
      _Service = service;
    }

    public DataCollection<Entity> GetAllWithEmptyCommunicationRef()
    {
      QueryExpression query = new QueryExpression(Constants.Contact.ENTITYNAME)
      {
        ColumnSet = new ColumnSet(
          Constants.Contact.Fields.EMAIL,
          Constants.Contact.Fields.PHONE,
          Constants.Contact.Fields.NAME),
        NoLock = true,
      };

      FilterExpression filter = new FilterExpression(LogicalOperator.And);
      filter.AddCondition(
        Constants.Communication.ENTITYNAME, 
        Constants.Communication.Fields.CONTACTID, 
        ConditionOperator.Null);

      query.Criteria.AddFilter(filter);

      query.AddLink(
        Constants.Communication.ENTITYNAME, 
        Constants.Contact.Fields.CONTACTID, 
        Constants.Communication.Fields.CONTACTID, 
        JoinOperator.LeftOuter);

      EntityCollection result = _Service.RetrieveMultiple(query);

      return result.Entities;
    }
  }
}

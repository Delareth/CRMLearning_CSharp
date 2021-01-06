using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRMLearning.Plugins.new_agreement.Handlers
{
  public class AgreementService
  {
    private readonly IOrganizationService _OrganizationService;

    public AgreementService(IOrganizationService service)
    {
      _OrganizationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public void CheckForFirstAgreement(Entities.new_agreement target)
    {
      if (IsFirstAgreementForContact(target.new_contact))
      {
        DateTime date = target.new_date ?? throw new NullReferenceException("Agreement date not setted!");

        UpdateContactForNewDate(target.new_contact, date);
      }
    }

    private bool IsFirstAgreementForContact(EntityReference contactRef)
    {
      QueryExpression query = new QueryExpression(Entities.new_agreement.EntityLogicalName)
      {
        Criteria = new FilterExpression(LogicalOperator.And)
      };

      query.Criteria.AddCondition(
        new ConditionExpression(
          Entities.new_agreement.Fields.new_contact, 
          ConditionOperator.Equal, 
          contactRef.Id)
        );

      EntityCollection result = _OrganizationService.RetrieveMultiple(query);

      return result.Entities.Count == 0;
    }

    private void UpdateContactForNewDate(EntityReference contactRef, DateTime date)
    {
      Entity contactToUpdate = new Entity(contactRef.LogicalName, contactRef.Id);
      contactToUpdate["new_date"] = date;

      _OrganizationService.Update(contactToUpdate);
    }
  }
}

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace CRMLearning.Workflows.NewAgreementActivities.Handlers
{
  public class AgreementService
  {
    private readonly IOrganizationService _OrganizationService;

    public AgreementService(IOrganizationService service)
    {
      _OrganizationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public bool IsHasRelatedInvoices(EntityReference agreementRef)
    {
      return GetAllRelatedInvoices(agreementRef).Count != 0;
    }

    public bool IsHasRelatedInvoicesWithPaidState(EntityReference agreementRef)
    {
      ConditionExpression expression = new ConditionExpression(
        Entities.new_invoice.Fields.new_fact,
        ConditionOperator.Equal,
        true);

      DataCollection<Entity> entities = GetAllRelatedInvoices(agreementRef, new List<ConditionExpression> { expression });

      return entities.Count != 0;
    }

    public bool IsHasRelatedInvoicesWithCommonType(EntityReference agreementRef)
    {
      ConditionExpression expression = new ConditionExpression(
        Entities.new_invoice.Fields.new_type,
        ConditionOperator.Equal,
        Entities.new_invoice_new_type.__100000000);

      DataCollection<Entity> entities = GetAllRelatedInvoices(agreementRef, new List<ConditionExpression> { expression });

      return entities.Count != 0;
    }

    public void DeleteRelatedInvoicesWithAutoType(EntityReference agreementRef)
    {
      ConditionExpression expression = new ConditionExpression(
        Entities.new_invoice.Fields.new_type, 
        ConditionOperator.Equal, 
        Entities.new_invoice_new_type.__100000001);

      DataCollection<Entity> entities = GetAllRelatedInvoices(agreementRef, new List<ConditionExpression> { expression });

      foreach (Entity entity in entities)
      {
        _OrganizationService.Delete(entity.LogicalName, entity.Id);
      }
    }

    public void CreatePaymentSchedule(Entities.new_agreement entity)
    {
      int creditPeriod = entity.new_creditperiod ?? throw new ArgumentNullException(nameof(entity.new_creditperiod));
      Money creditAmount = entity.new_creditamount ?? throw new ArgumentNullException(nameof(entity.new_creditamount));
      DateTime agreementStartDate = entity.new_date ?? throw new ArgumentNullException(nameof(entity.new_date));
      DateTime currDate = DateTime.Now;

      // определяем дату первого счета
      DateTime nextMonth = currDate.AddMonths(1);
      DateTime startDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);

      // дата окончания кредита
      DateTime creditEndDate = agreementStartDate.AddYears(creditPeriod);

      // сумма ежемесячного платежа по счету
      decimal amountPerMonth = Math.Ceiling(creditAmount.Value / (creditPeriod * 12));

      while (startDate < creditEndDate)
      {
        Entities.new_invoice newInvoice = new Entities.new_invoice
        {
          new_name = "Счет на оплату договора " + entity.new_name,
          new_date = currDate,
          new_paydate = startDate,
          new_dogovorid = entity.ToEntityReference(),
          new_fact = false,
          new_type = Entities.new_invoice_new_type.__100000001,
          new_amount = new Money(amountPerMonth)
        };

        _OrganizationService.Create(newInvoice);

        startDate = startDate.AddMonths(1);
      }
    }

    public void UpdateAgreementScheduleDate(Entities.new_agreement entity)
    {
      entity.new_paymentplandate = DateTime.Now.AddDays(1);

      _OrganizationService.Update(entity);
    }

    private DataCollection<Entity> GetAllRelatedInvoices(
      EntityReference agreementRef, 
      List<ConditionExpression> expressions = null)
    {
      if (expressions == null) expressions = new List<ConditionExpression>();

      QueryExpression query = new QueryExpression(Entities.new_invoice.EntityLogicalName)
      {
        ColumnSet = new ColumnSet(Entities.new_invoice.Fields.new_invoiceId),
        NoLock = true,
      };

      FilterExpression filter = query.Criteria.AddFilter(LogicalOperator.And);

      filter.AddCondition(Entities.new_invoice.Fields.new_dogovorid, ConditionOperator.Equal, agreementRef.Id);

      foreach (ConditionExpression condition in expressions)
      {
        filter.AddCondition(condition);
      }

      LinkEntity link = query.AddLink(
        Entities.new_agreement.EntityLogicalName,
        Entities.new_invoice.Fields.new_dogovorid,
        Entities.new_agreement.Fields.new_agreementId,
        JoinOperator.LeftOuter);

      link.Columns = new ColumnSet(Entities.new_agreement.Fields.new_name);
      link.EntityAlias = "agreement";

      EntityCollection result = _OrganizationService.RetrieveMultiple(query);

      return result.Entities;
    }
  }
}

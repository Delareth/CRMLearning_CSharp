using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRMLearning.Plugins.new_invoice.Handlers
{
  public class InvoiceService
  {
    private readonly IOrganizationService _OrganizationService;

    public InvoiceService(IOrganizationService service)
    {
      _OrganizationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public void SetPrePayDate(Entities.new_invoice target, DateTime date)
    {
      target.new_paydate = date;
    }

    public void SetPreTypeIfEmpty(Entities.new_invoice target)
    {
      target.new_type = target.new_type ?? Entities.new_invoice_new_type.__100000000;
    }

    public void RecalculateAndChangeAgreementPaid(
      RecalculateType type, 
      Entities.new_invoice target, 
      Entities.new_invoice entityWithData, 
      Entities.new_invoice entityWithOldData = null)
    {
      Entities.new_invoice entityToCheckPaid = target.new_fact == null ? entityWithData : target;

      if (IsPaid(entityToCheckPaid))
      {
        RecalculateAgreementAmount(type, entityWithData, entityWithOldData);

        ChangeAgreementPaidState(entityWithData, IsAgreementFullPaid(entityWithData));
      }
    }

    /// <exception cref="InvalidPluginExecutionException">Thrown when agreement full paid</exception>
    public void ValidatePaidInvoice(Entities.new_invoice target, Entities.new_invoice entityWithData)
    {
      if (IsPaid(target))
      {
        if (IsAgreementFullPaid(entityWithData))
        {
          throw new InvalidPluginExecutionException("Данный договор уже полностью оплачен!");
        }

        SetPrePayDate(target, DateTime.Now);
      }
    }

    private bool IsPaid(Entities.new_invoice targetEntity)
    {
      return targetEntity.new_fact ?? false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity">Entity with agremeent and amount fields</param>
    private void RecalculateAgreementAmount(RecalculateType type, Entities.new_invoice entity, Entities.new_invoice oldEntity)
    {
      Entities.new_agreement agreement = _OrganizationService.Retrieve(
        entity.new_dogovorid.LogicalName,
        entity.new_dogovorid.Id,
        new ColumnSet(
          Entities.new_agreement.Fields.new_factsumma)
        ).ToEntity<Entities.new_agreement>();

      Money agreementMoney = agreement.new_factsumma;
      Money currentMoney = entity.new_amount;
      Money oldMoney = oldEntity == null ? new Money(0) : oldEntity.new_amount;

      Money recalculatedMoney = GetRecalculatedMoneyByType(type, agreementMoney, currentMoney, oldMoney);

      Entities.new_agreement updatedAgreement = new Entities.new_agreement
      {
        Id = agreement.Id,
        new_factsumma = recalculatedMoney
      };

      _OrganizationService.Update(updatedAgreement);
    }

    private Money GetRecalculatedMoneyByType(RecalculateType type, Money agreementMoney, Money currentMoney, Money oldMoney)
    {
      Money outMoney = new Money();

      switch (type)
      {
        case RecalculateType.Create:
        {
          decimal diff = currentMoney.Value + agreementMoney.Value;

          outMoney.Value = diff;
          break;
        }
        case RecalculateType.Delete:
        {
          decimal diff = agreementMoney.Value - currentMoney.Value;

          outMoney.Value = diff;
          break;
        }
        case RecalculateType.Update:
        {
          decimal oldDiff = agreementMoney.Value - oldMoney.Value;
          decimal diff = oldDiff + currentMoney.Value;

          outMoney.Value = diff;
          break;
        }
        default:
        {
          throw new InvalidPluginExecutionException("Not implemented recalculate type");
        }
      }

      return outMoney;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">Entity with agremeent</param>
    private bool IsAgreementFullPaid(Entities.new_invoice target)
    {
      Entities.new_agreement agreement = _OrganizationService.Retrieve(
        target.new_dogovorid.LogicalName,
        target.new_dogovorid.Id,
        new ColumnSet(
          Entities.new_agreement.Fields.new_factsumma,
          Entities.new_agreement.Fields.new_summa)
        ).ToEntity<Entities.new_agreement>();

      Money paid = agreement.new_factsumma;
      Money total = agreement.new_summa;

      return paid.Value >= total.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">Entity with agremeent</param>
    private void ChangeAgreementPaidState(Entities.new_invoice target, bool state)
    {
      Entities.new_agreement updatedAgreement = new Entities.new_agreement
      {
        Id = target.new_dogovorid.Id,
        new_fact = state
      };

      _OrganizationService.Update(updatedAgreement);
    }
  }
}

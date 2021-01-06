using CRMLearning.Plugins.new_invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_invoice
{
  public sealed class PreNewInvoiceDelete : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        InvoiceService service = new InvoiceService(pluginData.OrganizationService);
        Entity preImage = pluginData.PluginExecutionContext.PreEntityImages["preImage"];

        Entities.new_invoice target = new Entities.new_invoice
        {
          Id = pluginData.TargetRef.Id
        };

        Entities.new_invoice dataEntity = preImage.ToEntity<Entities.new_invoice>();

        service.RecalculateAndChangeAgreementPaid(RecalculateType.Delete, target, dataEntity);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

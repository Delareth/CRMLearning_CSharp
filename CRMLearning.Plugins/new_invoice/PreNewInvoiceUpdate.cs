using CRMLearning.Plugins.new_invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_invoice
{
  public sealed class PreNewInvoiceUpdate : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        InvoiceService service = new InvoiceService(pluginData.OrganizationService);
        Entity preImage = pluginData.PluginExecutionContext.PreEntityImages["preImage"];

        Entities.new_invoice target = pluginData.Target.ToEntity<Entities.new_invoice>();
        Entities.new_invoice dataEntity = preImage.ToEntity<Entities.new_invoice>();

        service.ValidatePaidInvoice(target, dataEntity);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

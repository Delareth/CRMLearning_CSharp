using CRMLearning.Plugins.new_invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_invoice
{
  public sealed class PreNewInvoiceCreate : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        InvoiceService service = new InvoiceService(pluginData.OrganizationService);

        Entities.new_invoice target = pluginData.Target.ToEntity<Entities.new_invoice>();

        service.SetPreTypeIfEmpty(target);

        service.ValidatePaidInvoice(target, target);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

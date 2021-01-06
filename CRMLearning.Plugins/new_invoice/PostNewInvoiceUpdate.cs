using CRMLearning.Plugins.new_invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_invoice
{
  public sealed class PostNewInvoiceUpdate : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        InvoiceService service = new InvoiceService(pluginData.OrganizationService);
        Entity preImage = pluginData.PluginExecutionContext.PreEntityImages["preImage"];
        Entity postImage = pluginData.PluginExecutionContext.PostEntityImages["postImage"];

        Entities.new_invoice target = pluginData.Target.ToEntity<Entities.new_invoice>();
        Entities.new_invoice dataEntity = postImage.ToEntity<Entities.new_invoice>();
        Entities.new_invoice oldDataEntity = preImage.ToEntity<Entities.new_invoice>();

        service.RecalculateAndChangeAgreementPaid(RecalculateType.Update, target, dataEntity, oldDataEntity);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

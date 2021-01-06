using CRMLearning.Plugins.new_agreement.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_agreement
{
  public sealed class PreNewAgreementCreate : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        AgreementService service = new AgreementService(pluginData.OrganizationService);

        Entities.new_agreement target = pluginData.Target.ToEntity<Entities.new_agreement>();

        service.CheckForFirstAgreement(target);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

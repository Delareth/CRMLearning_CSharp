using CRMLearning.Plugins.new_communication.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_communication
{
  public sealed class PreNewCommunicationCreate : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        CommunicationService service = new CommunicationService(pluginData.OrganizationService);

        Entities.new_communication target = pluginData.Target.ToEntity<Entities.new_communication>();

        service.ValidatePreCreate(target);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

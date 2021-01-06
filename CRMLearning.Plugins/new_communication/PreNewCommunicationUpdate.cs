using CRMLearning.Plugins.new_communication.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins.new_communication
{
  public sealed class PreNewCommunicationUpdate : AbstractBasicPlugin
  {
    public override void PluginExecuted(PluginData pluginData)
    {
      try
      {
        CommunicationService service = new CommunicationService(pluginData.OrganizationService);
        Entity preImage = pluginData.PluginExecutionContext.PreEntityImages["preImage"];

        Entities.new_communication target = pluginData.Target.ToEntity<Entities.new_communication>();
        Entities.new_communication dataEntity = preImage.ToEntity<Entities.new_communication>();

        service.ValidatePreUpdate(target, dataEntity);
      }
      catch (Exception ex)
      {
        pluginData.TracingService.Trace(ex.ToString());

        throw new InvalidPluginExecutionException(ex.Message);
      }
    }
  }
}

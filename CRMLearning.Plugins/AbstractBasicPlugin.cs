using Microsoft.Xrm.Sdk;
using System;

namespace CRMLearning.Plugins
{
  public abstract class AbstractBasicPlugin : IPlugin
  {
    public AbstractBasicPlugin()
    {

    }

    public void Execute(IServiceProvider serviceProvider)
    {
      var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
      tracingService.Trace("Getted TracingService");

      var pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
      tracingService.Trace("Getted PluginExecutionContext");

      var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
      var organizationService = serviceFactory.CreateOrganizationService(Guid.Empty);
      tracingService.Trace("Getted OrganizationService");

      PluginData pluginData = null;

      if (pluginExecutionContext.InputParameters["Target"].GetType() == typeof(Entity))
      {
        var target = (Entity)pluginExecutionContext.InputParameters["Target"];

        pluginData = new PluginData(tracingService, pluginExecutionContext, organizationService, target, null);
        tracingService.Trace("Getted Target with type Entity");
      }
      else if (pluginExecutionContext.InputParameters["Target"].GetType() == typeof(EntityReference))
      {
        var target = (EntityReference)pluginExecutionContext.InputParameters["Target"];

        pluginData = new PluginData(tracingService, pluginExecutionContext, organizationService, null, target);
        tracingService.Trace("Getted Target with type EntityReference");
      }

      PluginExecuted(pluginData);
    }

    public abstract void PluginExecuted(PluginData pluginData);
  }
}

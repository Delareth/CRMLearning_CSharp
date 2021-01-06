using Microsoft.Xrm.Sdk;

namespace CRMLearning.Plugins
{
  public class PluginData
  {
    public ITracingService TracingService { get; }
    public IPluginExecutionContext PluginExecutionContext { get; }
    public IOrganizationService OrganizationService { get; }
    /// <summary>
    /// Will be empty if plugin type = Delete
    /// </summary>
    public Entity Target { get; }
    /// <summary>
    /// Will be empty if plugin type != Delete
    /// </summary>
    public EntityReference TargetRef { get; }

    public PluginData(ITracingService tracingService, 
      IPluginExecutionContext pluginExecutionContext, 
      IOrganizationService organizationService, 
      Entity target,
      EntityReference targetRef)
    {
      TracingService = tracingService;
      PluginExecutionContext = pluginExecutionContext;
      OrganizationService = organizationService;
      Target = target;
      TargetRef = targetRef;
    }
  }
}

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace CRMLearning.Workflows
{
  public class WorkflowData
  {
    public CodeActivityContext Context { get; }
    public IWorkflowContext WorkflowContext { get; }
    public IOrganizationService OrganizationService { get; }
    public ITracingService TracingService { get; }
    public Entity Target { get; }

    public WorkflowData(
      CodeActivityContext context, 
      IWorkflowContext workflowContext, 
      IOrganizationService organizationService, 
      ITracingService tracingService,
      Entity target)
    {
      Context = context;
      WorkflowContext = workflowContext;
      OrganizationService = organizationService;
      TracingService = tracingService;
      Target = target;
    }
  }
}

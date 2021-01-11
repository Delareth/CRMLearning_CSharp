using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace CRMLearning.Workflows
{
  public abstract class AbstractBasicWorkflow : CodeActivity
  {
    public AbstractBasicWorkflow()
    {

    }

    protected override void Execute(CodeActivityContext context)
    {
      ITracingService traceService = context.GetExtension<ITracingService>();
      traceService.Trace("Getted traceService");

      IWorkflowContext wfContext = context.GetExtension<IWorkflowContext>();
      traceService.Trace("Getted workflowContext");

      var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

      IOrganizationService service = serviceFactory.CreateOrganizationService(null);
      traceService.Trace("Getted organizationService");

      Entity target = (Entity)wfContext.InputParameters["Target"];

      WorkflowData data = new WorkflowData(context, wfContext, service, traceService, target);

      Executed(data);
    }

    public abstract void Executed(WorkflowData data);
  }
}

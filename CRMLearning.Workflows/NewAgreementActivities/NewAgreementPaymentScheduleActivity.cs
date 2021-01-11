using CRMLearning.Workflows.NewAgreementActivities.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace CRMLearning.Workflows.NewAgreementActivities
{
  public sealed class NewAgreementPaymentScheduleActivity : AbstractBasicWorkflow
  {
    [Input("new_agreement")]
    [RequiredArgument]
    [ReferenceTarget("new_agreement")]
    public InArgument<EntityReference> AgreementReference { get; set; }

    public override void Executed(WorkflowData data)
    {
      try
      {
        EntityReference agreementRef = AgreementReference.Get(data.Context);
        Entities.new_agreement entity = data.Target.ToEntity<Entities.new_agreement>();

        AgreementService service = new AgreementService(data.OrganizationService);

        service.DeleteRelatedInvoicesWithAutoType(agreementRef);
        service.CreatePaymentSchedule(entity);
        service.UpdateAgreementScheduleDate(entity);
      }
      catch (Exception ex)
      {
        data.TracingService.Trace(ex.ToString());

        throw new InvalidWorkflowException(ex.Message);
      }
    }
  }
}

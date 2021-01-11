using CRMLearning.Workflows.NewAgreementActivities.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace CRMLearning.Workflows.NewAgreementActivities
{
  public sealed class NewAgreementValidationActivity : AbstractBasicWorkflow
  {
    [Input("new_agreement")]
    [RequiredArgument]
    [ReferenceTarget("new_agreement")]
    public InArgument<EntityReference> AgreementReference { get; set; }

    [Output("Is agreement has related invoices")]
    public OutArgument<bool> IsHasRelatedInvoices { get; set; }

    [Output("Is agreement has related invoices with paid state")]
    public OutArgument<bool> IsHasRelatedInvoicesWithPaidState { get; set; }

    [Output("Is agreement has related invoices with common type")]
    public OutArgument<bool> IsHasRelatedInvoicesWithCommonType { get; set; }

    public override void Executed(WorkflowData data)
    {
      try
      {
        EntityReference agreementRef = AgreementReference.Get(data.Context);

        AgreementService service = new AgreementService(data.OrganizationService);

        IsHasRelatedInvoices.Set(data.Context, service.IsHasRelatedInvoices(agreementRef));
        IsHasRelatedInvoicesWithPaidState.Set(data.Context, service.IsHasRelatedInvoicesWithPaidState(agreementRef));
        IsHasRelatedInvoicesWithCommonType.Set(data.Context, service.IsHasRelatedInvoicesWithCommonType(agreementRef));
      }
      catch (Exception ex)
      {
        data.TracingService.Trace(ex.ToString());

        throw new InvalidWorkflowException(ex.Message);
      }
    }
  }
}

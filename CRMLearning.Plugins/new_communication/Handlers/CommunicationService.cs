using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRMLearning.Plugins.new_communication.Handlers
{
  public class CommunicationService
  {
    private readonly IOrganizationService _OrganizationService;

    public CommunicationService(IOrganizationService service)
    {
      _OrganizationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <exception cref="InvalidPluginExecutionException">Throws when ommunication with selected type already exist</exception>
    public void ValidatePreCreate(Entities.new_communication target)
    {
      if (IsMain(target))
      {
        Entities.new_communication_new_type type = GetCreatingType(target);

        if (IsCommunicationExist(target, type))
        {
          throw new InvalidPluginExecutionException("Communication with selected type already exist!");
        }
      }
    }

    /// <exception cref="InvalidPluginExecutionException">Throws when ommunication with selected type already exist</exception>
    public void ValidatePreUpdate(Entities.new_communication target, Entities.new_communication dataEntity)
    {
      // если изменилось поле основной
      if (target.new_main != null)
      {
        if (IsMain(target))
        {
          Entities.new_communication_new_type type;

          // если изменилось поле с типом связи
          if (target.new_type != null)
          {
            type = GetCreatingType(target);
          }
          else
          {
            type = GetCreatingType(dataEntity);
          }

          if (IsCommunicationExist(dataEntity, type))
          {
            throw new InvalidPluginExecutionException("Communication with selected type already exist!");
          }
        }
      }
      // если изменилось поле с типом связи
      else if (target.new_type != null)
      {
        if (IsMain(dataEntity))
        {
          Entities.new_communication_new_type type = GetCreatingType(target);

          if (IsCommunicationExist(dataEntity, type))
          {
            throw new InvalidPluginExecutionException("Communication with selected type already exist!");
          }
        }
      }
    }

    private bool IsCommunicationExist(Entities.new_communication target, Entities.new_communication_new_type type)
    {
      EntityReference contactRef = target.new_contactid;

      QueryExpression query = new QueryExpression(target.LogicalName);

      FilterExpression filter = new FilterExpression(LogicalOperator.And);
      filter.AddCondition(Entities.new_communication.Fields.new_type, ConditionOperator.Equal, (int)type);
      filter.AddCondition(Entities.new_communication.Fields.new_main, ConditionOperator.Equal, true);
      filter.AddCondition(Entities.new_communication.Fields.new_contactid, ConditionOperator.Equal, contactRef.Id);

      query.Criteria = filter;

      EntityCollection result = _OrganizationService.RetrieveMultiple(query);

      return result.Entities.Count > 0;
    }

    private Entities.new_communication_new_type GetCreatingType(Entities.new_communication target)
    {
      return target.new_type ?? throw new NullReferenceException("Communication type not selected on creating");
    }

    private bool IsMain(Entities.new_communication target)
    {
      return target.new_main ?? false;
    }
  }
}

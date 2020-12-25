using Microsoft.Xrm.Sdk;

namespace ConsoleApp.Communication
{
  public class Data
  {
    public EntityReference ConctactRef { get; }
    public string Field { get; }
    public string Value { get; }

    public Data(EntityReference conctactRef, string field, string value)
    {
      ConctactRef = conctactRef;
      Field = field;
      Value = value;
    }
  }
}

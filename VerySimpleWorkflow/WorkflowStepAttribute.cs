namespace VerySimpleWorkflow;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class WorkflowStepAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
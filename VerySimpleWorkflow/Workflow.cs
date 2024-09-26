using System.Reflection;

namespace VerySimpleWorkflow;

public abstract class Workflow<TResult>
{
    private readonly Dictionary<string, Func<object, object?>> _steps = new Dictionary<string, Func<object, object?>>();
    private readonly Dictionary<string, WorkflowStepResult> _context = new Dictionary<string, WorkflowStepResult>();

    protected Workflow()
    {
    }

    public abstract TResult Run(object input);

    public void RegisterStepsFrom(Type type)
    {
        var methods = type.GetMembers(BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public);
        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes<WorkflowStepAttribute>().ToArray();
            if (attributes.Length == 0) continue;
            
            var methodInfo = type.GetMethod(method.Name)!;
            
            var func = (Func<object, object?>) (input => methodInfo.Invoke(null, [input]));

            foreach (var attribute in attributes)
            {
                _steps.Add(attribute.Name, func);
            }
        }
    }


    private WorkflowStepResult? RestoreState(string stepName) =>
        _context.TryGetValue(stepName, out var state) ? state : null;

    protected void SaveState(string stepName, WorkflowStepResult state) =>
        _context.Add(stepName, state);

    protected WorkflowStepResult ExecuteStep(string stepName, object input)
    {
        var state = RestoreState(stepName);
        if (state.HasValue)
        {
            return state.Value;
        }

        var result = GetStep(stepName).Invoke(input);
        
        return new WorkflowStepResult(true, result, result?.GetType()?.ToString() ?? "null");
    }

    private Func<object, object?> GetStep(string stepName) => _steps.TryGetValue(stepName, out var step) ? step : throw new InvalidOperationException($"Step {stepName} not found");
}
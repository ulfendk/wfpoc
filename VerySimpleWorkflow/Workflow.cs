using System.Reflection;

namespace VerySimpleWorkflow;

public abstract class Workflow<TResult>
{
    private readonly Dictionary<string, Func<object, object?>> _steps = new Dictionary<string, Func<object, object?>>();
    private readonly Dictionary<string, WorkflowStepResult> _context = new Dictionary<string, WorkflowStepResult>();

    protected Workflow()
    {
    }

    protected abstract TResult Implementation(object input);

    public WorkflowResult Execute(object input)
    {
        try
        {
            var result = Implementation(input);

            return new WorkflowResult(true, result, result?.GetType()?.Name ?? "null");
        }
        catch (Exception e)
        {
            Console.WriteLine($"  Workflow stopped stopped prematurely with the steps [{string.Join(", ", _context.Where(kv => kv.Value.HasCompleted).Select(kv => kv.Key))}] completed.");

            return new WorkflowResult(false, null, "null");
        }
    }

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


    private WorkflowStepResult? RestoreState(string stepName)
    {
        if (!_context.TryGetValue(stepName, out var state)) return null;
        
        Console.WriteLine($"  Restored state for {stepName}");

        return state;

    }

    private WorkflowStepResult SaveState(string stepName, WorkflowStepResult state)
    {
        if (_context.TryAdd(stepName, state))
        {
            Console.WriteLine($"  Saved state for {stepName}");
        }

        return state;
    }

    protected WorkflowStepResult ExecuteStep(string stepName, object input)
    {
        var state = RestoreState(stepName);
        if (state.HasValue)
        {
            return state.Value;
        }

        var result = GetStep(stepName).Invoke(input);
        
        var stepResult = new WorkflowStepResult(true, result, result?.GetType()?.ToString() ?? "null");
        
        return SaveState(stepName, stepResult);
    }

    private Func<object, object?> GetStep(string stepName) => _steps.TryGetValue(stepName, out var step) ? step : throw new InvalidOperationException($"Step {stepName} not found");
}
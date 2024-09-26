namespace VerySimpleWorkflow;

public record struct WorkflowStepResult(bool HasCompleted, object? Result, string ResultType)
{
    public TResult ResultAs<TResult>() => (TResult)Result;
};

public record struct WorkflowResult(bool HasCompleted, object? Result, string ResultType);
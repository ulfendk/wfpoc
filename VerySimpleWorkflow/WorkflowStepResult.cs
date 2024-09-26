namespace VerySimpleWorkflow;

public record struct WorkflowStepResult(bool HasCompleted, object? Result, string ResultType)
{
    public TResult ResultAs<TResult>() => (TResult)Result;
};
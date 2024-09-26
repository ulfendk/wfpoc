namespace VerySimpleWorkflow;

public static class AccountSteps
{
    [WorkflowStep(nameof(GetAccounts))]
    public static string[] GetAccounts(string owner)
    {
        return new[] { "1234567890", "9876543210" };
    }
    
    [WorkflowStep(nameof(GetBalances))]
    public static decimal[] GetBalances(string[] accounts)
    {
        return new[] { 666m, 911m };
    }
}
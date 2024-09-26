namespace VerySimpleWorkflow;

public static class AccountSteps
{
    private static int counter = 0;
    [WorkflowStep(nameof(GetAccounts))]
    public static string[] GetAccounts(string owner)
    {
        return new[] { "1234567890", "9876543210" };
    }
    
    [WorkflowStep(nameof(GetBalances))]
    public static decimal[] GetBalances(string[] accounts)
    {
        if (counter++ == 0)
        {
            throw new Exception("Fail on first call");
        }

        return new[] { 666m, 911m };
    }
}
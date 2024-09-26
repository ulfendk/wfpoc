namespace VerySimpleWorkflow;

public class GetBalancesWorkflow() : Workflow<decimal[]>()
{
    protected override decimal[] Implementation(object input)
    {
        var accountsResult = ExecuteStep("GetAccounts", input);
        if (accountsResult.Result is null)
        {
            return Array.Empty<decimal>();
        }

        var balancesResult = ExecuteStep("GetBalances", accountsResult.ResultAs<string[]>());
        return balancesResult.ResultAs<decimal[]>();
    }
}
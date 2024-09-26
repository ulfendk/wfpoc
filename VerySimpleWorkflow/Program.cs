
using VerySimpleWorkflow;

var wf = new GetBalancesWorkflow();
wf.RegisterStepsFrom(typeof(AccountSteps));

var balances = wf.Run("mm");
Console.WriteLine($"Balances: {string.Join(", ", balances)}");

using VerySimpleWorkflow;

var wf = new GetBalancesWorkflow();
wf.RegisterStepsFrom(typeof(AccountSteps));

Console.WriteLine($"Executing workflow in {nameof(GetBalancesWorkflow)} first time");
var balances = wf.Run("mm");
Console.WriteLine($"Balances: {string.Join(", ", balances)}");

Console.WriteLine($"Executing workflow in {nameof(GetBalancesWorkflow)} second time");
balances = wf.Run("mm");
Console.WriteLine($"Balances: {string.Join(", ", balances)}");

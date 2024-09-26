
using VerySimpleWorkflow;

var wf = new GetBalancesWorkflow();
wf.RegisterStepsFrom(typeof(AccountSteps));

Console.WriteLine($"Executing workflow in {nameof(GetBalancesWorkflow)} first time");
var balances = wf.Execute("mm");
Console.WriteLine(balances.HasCompleted
    ? $"Balances: {string.Join(", ", balances)}"
    : "Workflow failed");


Console.WriteLine();

Console.WriteLine($"Executing workflow in {nameof(GetBalancesWorkflow)} second time");
balances = wf.Execute("mm");
Console.WriteLine(balances.HasCompleted
    ? $"Balances: {string.Join(", ", balances)}"
    : "Workflow failed");

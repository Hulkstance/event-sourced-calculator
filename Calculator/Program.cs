using Messages;
using Microsoft.Data.Sqlite;
using Proto;
using Proto.Persistence.Sqlite;

var system = new ActorSystem();
var context = new RootContext(system);
var provider = new SqliteProvider(new SqliteConnectionStringBuilder { DataSource = "states.db" });

var props = Props.FromProducer(() => new Calculator.Calculator(provider));
var pid = context.Spawn(props);

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var command = parts[0].ToLowerInvariant();
    var arg = parts.Length > 1 && double.TryParse(parts[1], out var val) ? val : (double?)null;

    switch (command)
    {
        case "add" when arg.HasValue:
            context.Send(pid, new AddCommand { Value = arg.Value });
            break;

        case "sub" when arg.HasValue:
            context.Send(pid, new SubtractCommand { Value = arg.Value });
            break;

        case "mul" when arg.HasValue:
            context.Send(pid, new MultiplyCommand { Value = arg.Value });
            break;

        case "div" when arg.HasValue:
            context.Send(pid, new DivideCommand { Value = arg.Value });
            break;

        case "clear":
            context.Send(pid, new ClearCommand());
            break;

        case "print":
            context.Send(pid, new PrintResultCommand());
            break;

        case "exit":
            context.Poison(pid);
            return;

        default:
            Console.WriteLine("Unknown command or missing argument.");
            break;
    }
}

// > add 100
// > sub 25
// > print
// Result: 75
// > mul 2
// > div 5
// > print
// Result: 30
// > clear
// > print
// Result: 0
// > exit

using Messages;
using Proto;
using Proto.Persistence;

namespace Calculator;

public class Calculator : IActor
{
    private double _result = 0;
    private readonly Persistence _persistence;

    public Calculator(IProvider provider)
    {
        _persistence = Persistence.WithEventSourcing(
            provider,
            "demo-app-id",
            ApplyEvent);
    }

    private void ApplyEvent(Event @event)
    {
        switch (@event)
        {
            case RecoverEvent msg:
                if (msg.Data is AddedEvent addedEvent)
                {
                    _result += addedEvent.Value;
                }
                else if (msg.Data is SubtractedEvent subtractedEvent)
                {
                    _result -= subtractedEvent.Value;
                }
                else if (msg.Data is DividedEvent dividedEvent)
                {
                    _result /= dividedEvent.Value;
                }
                else if (msg.Data is MultipliedEvent multipliedEvent)
                {
                    _result *= multipliedEvent.Value;
                }
                else if (msg.Data is ResetEvent resetEvent)
                {
                    _result = 0;
                }

                break;
            
            case ReplayEvent msg:
                break;
            
            case PersistedEvent msg:
                break;
        }
    }

    public async Task ReceiveAsync(IContext context)
    {
        switch (context.Message)
        {
            case Started:
                Console.WriteLine("MyPersistenceActor - Started");
                await _persistence.RecoverStateAsync();
                break;

            case AddCommand { Value: var value }:
                await _persistence.PersistEventAsync(new AddedEvent { Value = value });
                _result += value;

                break;

            case SubtractCommand { Value: var value }:
                await _persistence.PersistEventAsync(new SubtractedEvent { Value = value });
                _result -= value;

                break;

            case DivideCommand { Value: var value }:
                await _persistence.PersistEventAsync(new DividedEvent { Value = value });
                _result /= value;

                break;

            case MultiplyCommand { Value: var value }:
                await _persistence.PersistEventAsync(new MultipliedEvent { Value = value });
                _result *= value;

                break;

            case ClearCommand:
                await _persistence.PersistEventAsync(new ResetEvent());
                _result = 0;

                break;

            case PrintResultCommand:
                Console.WriteLine(_result);
                break;
        }
    }
}

using MassTransit;
using Sample.Contracts;

namespace Sample.Api.Consumers;

public class PolandPersonCreatedConsumer:    IConsumer<PersonCreated>
{
  
    public PolandPersonCreatedConsumer()
    {
     
    }
    
    public Task Consume(ConsumeContext<PersonCreated> context)
    {
        Console.WriteLine("PolandPersonCreatedConsumer");
        return Task.CompletedTask;
    }
}
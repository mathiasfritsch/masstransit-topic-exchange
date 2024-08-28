using MassTransit;
using Sample.Contracts;

namespace Sample.Api.Consumers;

public class GermanyPersonCreatedConsumer:    IConsumer<PersonCreated>
{
    readonly IPersonService  _personService;

    public GermanyPersonCreatedConsumer(IPersonService personService)
    {
        _personService = personService;
    }
    
    public GermanyPersonCreatedConsumer()
    {
     
    }
    
    public Task Consume(ConsumeContext<PersonCreated> context)
    {
        Console.WriteLine("GermanyPersonCreatedConsumer");
        return Task.CompletedTask;
    }
}
using MassTransit;
using Sample.Api;
using Sample.Api.Consumers;
using Sample.Contracts;
using RabbitMQ.Client;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// used to check if DI is working in GermanyPersonCreatedConsumer - it isn't
// because x.Consumer<GermanyPersonCreatedConsumer>() can use a consumer
// with parameterless constructor

builder.Services.AddScoped<IPersonService,PersonService>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        
        cfg.Message<PersonCreated>(e =>
            {
                e.SetEntityName("personcreated-exchangename");
            });
        
        cfg.Send<PersonCreated>(x =>
        {
            x.UseRoutingKeyFormatter(context => context.Message.Country);
        });
        
        cfg.Publish<PersonCreated>(x => x.ExchangeType = "topic");

        cfg.ReceiveEndpoint("personcreated-germany", x =>
        {
            x.ConfigureConsumeTopology = false;
            x.Consumer<GermanyPersonCreatedConsumer>();
            x.Bind("personcreated-exchangename", s => 
            {
                s.RoutingKey = "Germany";
                s.ExchangeType = ExchangeType.Topic;
            });
        });
        
        cfg.ReceiveEndpoint("personcreated-poland", x =>
        {
            x.ConfigureConsumeTopology = false;
            x.Consumer<PolandPersonCreatedConsumer>();
            x.Bind("personcreated-exchangename", s => 
            {
                s.RoutingKey = "Poland";
                s.ExchangeType = "topic";
            });
        });
        
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/PersonPoland", async ( IBus bus) =>
{
    await bus.Publish(new PersonCreated( Random.Shared.Next(1, 100) , "Poland", Guid.NewGuid()));
    return Results.Ok("ok");
});

app.MapPost("/PersonGermany", async ( IBus bus) =>
{
    await bus.Publish(new PersonCreated( Random.Shared.Next(1, 100) , "Germany",Guid.NewGuid()));
    return Results.Ok("ok");
});

app.Run();

public partial class Program
{
}
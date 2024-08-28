Creates a Topic Exchange
Consumer receives messages depending on RouteKey.
Not shure if  x.AddConsumer should still be used

In a different Project this was required
Maybe because here its consumer and producer in one project.

Can use DI in Consumer because     x.Consumer<PolandPersonCreatedConsumer>() expects parameterless constructor

This is also different in the other project
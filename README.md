# chaos-service

Chaos strategy is a software development strategy based on the chaos model. Its main rule is to always solve the most important problems first.

The most important issues include the three aspects of big, urgent and strong.

- The big problem provides users with function points.
- Urgent problems need to be solved urgently, otherwise other work may be delayed.
- Strong problems are considered trustworthy after being solved and tested, so that developers can safely look elsewhere.

[![FlashElf.ChaosKit Introduction](http://i3.ytimg.com/vi/L-iCH1w1iKU/hqdefault.jpg)](https://youtu.be/L-iCH1w1iKU "FlashElf.ChaosKit")

## Get Packages

You can get FlashElf.ChaosKit by [grabbing the latest NuGet packages](https://www.nuget.org/packages/FlashElf.ChaosKit/). If you're feeling adventurous, continuous integration builds are on MyGet.

## Get Started

Super-duper quick start for aspnet with Autofac:

Register external dependency interface types with a ContainerBuilder and then build the container.

```CSharp
var builder = new ContainerBuilder();
var autofacStarter = new AutofacStarter(builder);

builder.RegisterControllers(Assembly.GetExecutingAssembly());
builder.RegisterType<MyRepo>().As<IMyRepo>();

//Please add the following chaos code after the production code
builder.AddChaosServices("127.0.0.1:50050");
builder.AddChaosTransient<IMyRepo>();


var container = autofacStarter.Build();

//Start Chaos-Server
container.Resolve<IChaosServer>().Start();
```

Super-duper quick start for aspnet core:

Modify Startup.cs and register external dependency interface types with a IServiceCollection.

```CSharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddControllersWithViews();
	services.AddTransient<IMyRepo, MyRepo>();

    //Please add the following chaos code after the production code
	services.AddChaosServices("127.0.0.1:50050");
	services.AddChaosTransient<IMyRepo>();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IChaosServer chaosServer)
{
    if (env.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}

    //Start Chaos-Server
    chaosServer.Start();
}
```

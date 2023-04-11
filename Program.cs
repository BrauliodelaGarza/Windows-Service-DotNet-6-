using $safeprojectname$;

IHost host = Host.CreateDefaultBuilder(args).UseWindowsService(options =>
                {
                    options.ServiceName = "Send Eco Notifications";
                })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    }).Build();

host.Run();

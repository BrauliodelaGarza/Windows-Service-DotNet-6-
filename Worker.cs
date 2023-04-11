namespace $safeprojectname$;
using Cronos;

public class Worker : BackgroundService
{
    private static readonly int ExecutionHour = 12;
    private readonly CronExpression _cron = CronExpression.Parse("0 " + ExecutionHour + " * * MON-FRI");

    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProductionMode(DateTime.UtcNow);
            await DelayTask();
        }
    }

        /// <summary>
        /// Set the system in TestMode
        /// </summary>
        /// <returns>Awaitable Task</returns>
        private async Task TestMode(DateTime Now)
        {
            await Task.Run(() => {
                // Insert Code Here For The Production Environment
                _logger.LogInformation("Executed At: {utcNow}", Now);

            });
        }

        /// <summary>
        /// Set the system in ProductionMode
        /// </summary>
        /// <returns>Awaitable Task</returns>
        private async Task ProductionMode(DateTime Now)
        {
            //Execute only if the hour is between 
            if (Now.Hour >= ExecutionHour && Now.AddHours(1).Hour <= ExecutionHour)
            {
                await Task.Run(() => {
                    // Insert Code Here For The Production Environment

                });
            }
        }

        /// <summary>
        /// Based on the current UTC Time, adds a delay in the next execution 
        /// </summary>
        /// <returns>a =delay until the next cronos Occurrence</returns>
        private Task DelayTask()
        {
            TimeSpan NextExecution = _cron.GetNextOccurrence(DateTime.UtcNow)!.Value - DateTime.UtcNow;
            _logger.LogInformation("Next Execution in: {delay}", NextExecution);

            return Task.Delay(NextExecution);
        }
}

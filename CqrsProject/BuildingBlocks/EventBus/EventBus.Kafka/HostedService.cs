using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EventBus.Kafka;

public abstract class HostedService : IHostedService
{
    public Task? ExecutingTask { get; private set; }
    private CancellationTokenSource? _cts;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        ExecutingTask = ExecuteAsync(_cts.Token);
        
        return ExecutingTask.IsCompleted ? ExecutingTask : Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();
        
        if (ExecutingTask != null) 
            await Task.WhenAny(ExecutingTask, Task.Delay(-1, cancellationToken));
        
        cancellationToken.ThrowIfCancellationRequested();
    }
    
    protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
}
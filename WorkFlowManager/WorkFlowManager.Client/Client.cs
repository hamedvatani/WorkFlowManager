using Microsoft.Extensions.Hosting;

namespace WorkFlowManager.Client;

public class Client : IHostedService
{
    private readonly ClientConfiguration _configuration;
    private readonly RpcClient _rpcClient;

    public Client(ClientConfiguration configuration, RpcClient rpcClient)
    {
        _configuration = configuration;
        _rpcClient = rpcClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rpcClient.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _rpcClient.Stop();
        return Task.CompletedTask;
    }
}
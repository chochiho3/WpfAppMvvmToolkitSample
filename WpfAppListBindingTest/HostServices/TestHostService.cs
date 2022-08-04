using Microsoft.Extensions.Hosting;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAppListBindingTest.HostServices
{
    internal class TestHostService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("비동기 호스트 서비스가 시작되었습니다.!!!!!!!!!!!!!!!");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("비동기 호스트 서비스가 중단되었습니다?????????????????");
            await Task.CompletedTask;
        }
    }
}

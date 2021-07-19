using System.Threading.Tasks;

namespace BasicHttpServer.HTTP
{
    public interface IHttpServer
    {
        Task StartAsync(int port);
    }
}
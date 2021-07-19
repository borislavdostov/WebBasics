using System.Collections.Generic;
using System.Threading.Tasks;
using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(List<Route> routeTable, int port = 80)
        {
            IHttpServer server = new HttpServer(routeTable);

            await server.StartAsync(port);
        }
    }
}

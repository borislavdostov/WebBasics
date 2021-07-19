﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port = 80)
        {
            var routeTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routeTable);

            IHttpServer server = new HttpServer(routeTable);

            await server.StartAsync(port);
        }
    }
}

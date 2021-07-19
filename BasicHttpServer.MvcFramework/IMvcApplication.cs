using System.Collections.Generic;
using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(List<Route> routeTable);
    }
}

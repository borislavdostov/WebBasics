using System.Collections.Generic;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Data;
using Microsoft.EntityFrameworkCore;

namespace BattleCards
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices()
        {
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}

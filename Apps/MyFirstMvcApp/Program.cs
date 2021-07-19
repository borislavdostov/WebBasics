using System.Collections.Generic;
using System.Threading.Tasks;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using MyFirstMvcApp.Controllers;

namespace MyFirstMvcApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateHostAsync(new Startup());
        }
    }
}
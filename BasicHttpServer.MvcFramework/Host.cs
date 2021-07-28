using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port = 80)
        {
            var routeTable = new List<Route>();

            AutoRegisterStaticFiles(routeTable);
            AutoRegisterRoutes(routeTable, application);

            application.ConfigureServices();
            application.Configure(routeTable);

            Console.WriteLine("All registered routes:");
            foreach (var route in routeTable)
            {
                Console.WriteLine($"{route.Method} {route.Path}");
            }

            IHttpServer server = new HttpServer(routeTable);

            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application)
        {
            var controllerTypes = application.GetType().Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller)));

            foreach (var controllerType in controllerTypes)
            {
                var methods = controllerType.GetMethods()
                    .Where(m => m.IsPublic && !m.IsStatic && m.DeclaringType == controllerType &&
                                !m.IsAbstract && !m.IsConstructor && !m.IsSpecialName);

                foreach (var method in methods)
                {
                    var url = "/" + controllerType.Name.Replace("Controller", string.Empty) + "/" + method.Name;

                    var attribute = method.GetCustomAttributes(false)
                        .FirstOrDefault(ca => ca.GetType().IsSubclassOf(typeof(BaseHttpAttribute))) as BaseHttpAttribute;

                    var httpMethod = HttpMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (!string.IsNullOrEmpty(attribute?.Url))
                    {
                        url = attribute.Url;
                    }


                    routeTable.Add(new Route(url, httpMethod, (request) =>
                    {
                        var instance = Activator.CreateInstance(controllerType) as Controller;
                        instance.Request = request;
                        var response = method.Invoke(instance, new object[] { }) as HttpResponse;
                        return response;
                    }));
                }
            }
        }

        private static void AutoRegisterStaticFiles(List<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);
            foreach (var staticFile in staticFiles)
            {
                var url = staticFile.Replace("wwwroot", string.Empty).Replace("\\", "/");
                routeTable.Add(new Route(url, HttpMethod.Get, (request) =>
                {
                    var fileContent = File.ReadAllBytes(staticFile);
                    var fileExtension = new FileInfo(staticFile).Extension;
                    var contentType = fileExtension switch
                    {
                        ".txt" => "text/plain",
                        ".js" => "text/javascript",
                        ".css" => "text/css",
                        ".jpg" => "image/jpg",
                        ".jpeg" => "image/jpg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".ico" => "image/vnd.microsoft.icon",
                        ".html" => "text/html",
                        _ => "text/plain"
                    };

                    return new HttpResponse(contentType, fileContent, HttpStatusCode.Ok);
                }));
            }
        }
    }
}

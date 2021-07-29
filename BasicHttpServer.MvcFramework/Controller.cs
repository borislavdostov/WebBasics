using System.Runtime.CompilerServices;
using System.Text;
using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public abstract class Controller
    {
        private const string UserIdSessionName = "UserId";
        ViewEngine.ViewEngine viewEngine;

        protected Controller()
        {
            viewEngine = new ViewEngine.ViewEngine();
        }

        public HttpRequest Request { get; set; }

        protected HttpResponse View(object viewModel = null, [CallerMemberName] string viewPath = null)
        {
            var viewContent = System.IO.File.ReadAllText(
                "Views/" +
                GetType().Name.Replace("Controller", string.Empty) +
                "/" + viewPath + ".cshtml");
            viewContent = viewEngine.GetHtml(viewContent, viewModel);

            var responseHtml = PutViewInLayout(viewContent, viewModel);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);
            return response;
        }

        protected HttpResponse File(string filePath, string contentType)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new HttpResponse(contentType, fileBytes);
            return response;
        }

        protected HttpResponse Redirect(string url)
        {
            var response = new HttpResponse(HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", url));
            return response;
        }

        protected HttpResponse Error(string errorText)
        {
            var viewContent = $"<div class=\"alert alert-danger\" role=\"alert\">{errorText}</div>";
            var responseHtml = PutViewInLayout(viewContent);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes, HttpStatusCode.ServerError);
            return response;
        }

        protected void SignIn(string userId)
        {
            Request.Session[UserIdSessionName] = userId;
        }

        protected void SignOut()
        {
            Request.Session[UserIdSessionName] = null;
        }

        protected bool IsUserSignedIn => Request.Session.ContainsKey(UserIdSessionName);

        protected string GetUserId() => Request.Session[UserIdSessionName];

        protected string PutViewInLayout(string viewContent, object viewModel = null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "___VIEW_GOES_HERE___");
            layout = viewEngine.GetHtml(layout, viewModel);
            var responseHtml = layout.Replace("___VIEW_GOES_HERE___", viewContent);
            return responseHtml;
        }
    }
}

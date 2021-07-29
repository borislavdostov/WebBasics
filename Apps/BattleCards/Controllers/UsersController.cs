using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Services;

namespace BattleCards.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (IsUserSignedIn())   
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/");
            }

            var username = Request.FormData["username"];
            var password = Request.FormData["password"];
            var userId = usersService.GetUserId(username, password);

            if (userId == null)
            {
                return Error("Invalid username or password!");
            }

            SignIn(userId);
            return Redirect("/Cards/All");

        }

        public HttpResponse Register()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/");
            }

            var username = Request.FormData["username"];
            var email = Request.FormData["email"];
            var password = Request.FormData["password"];
            var confirmPassword = Request.FormData["confirmPassword"];

            if (username == null || username.Length < 5 || username.Length > 20)
            {
                return Error("Invalid username! The username should be between 5 and 20 characters!");
            }

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9\.]+$"))
            {
                return Error("Invalid username! Only alphanumeric characters are allowed!");
            }

            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return Error("Invalid email!");
            }

            if (password == null || password.Length < 6 || password.Length > 20)
            {
                return Error("Invalid password! The username should be between 6 and 20 characters!");
            }

            if (!password.Equals(confirmPassword))
            {
                return Error("Passwords should be the same!");
            }

            if (!usersService.IsUsernameAvailable(username))
            {
                return Error("Username already taken!");
            }

            if (!usersService.IsEmailAvailable(email))
            {
                return Error("Email already taken!");
            }

            var userId = usersService.CreateUser(username, password, email);

            return Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!IsUserSignedIn())
            {
                return Error("Only logged-in users can logout;");
            }

            SignOut();
            return Redirect("/");
        }
    }
}

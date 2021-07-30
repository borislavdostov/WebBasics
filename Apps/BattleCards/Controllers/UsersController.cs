using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Services;
using BattleCards.ViewModels.Users;

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

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (IsUserSignedIn())
            {
                return Redirect("/");
            }

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

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if (IsUserSignedIn())
            {
                return Redirect("/");
            }

            if (input.Username == null || input.Username.Length < 5 || input.Username.Length > 20)
            {
                return Error("Invalid username! The username should be between 5 and 20 characters!");
            }

            if (!Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                return Error("Invalid username! Only alphanumeric characters are allowed!");
            }

            if (string.IsNullOrWhiteSpace(input.Email) || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return Error("Invalid email!");
            }

            if (input.Password == null || input.Password.Length < 6 || input.Password.Length > 20)
            {
                return Error("Invalid password! The username should be between 6 and 20 characters!");
            }

            if (!input.Password.Equals(input.ConfirmPassword))
            {
                return Error("Passwords should be the same!");
            }

            if (!usersService.IsUsernameAvailable(input.Username))
            {
                return Error("Username already taken!");
            }

            if (!usersService.IsEmailAvailable(input.Email))
            {
                return Error("Email already taken!");
            }

            var userId = usersService.CreateUser(input.Username, input.Password, input.Email);

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

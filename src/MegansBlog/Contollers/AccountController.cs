using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MegansBlog.Models;
using Microsoft.AspNet.Identity;
using MegansBlog.ViewModels;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MegansBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var thisCategory = _db.Categories.FirstOrDefault(posts => posts.CategoryId == id);
            return View(thisCategory);
        }

        //Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult EmailList()
        {
            return View();
        }

        //[HttpGet]
        public async Task<IActionResult> FetchEmails()
        {
            Console.WriteLine("IM RUNNING!");

     


            var client = new RestClient("https://us13.api.mailchimp.com/3.0/")
            {
                Authenticator = new HttpBasicAuthenticator("stemple87", "AUTHNUMBERGITIGNORE")
            };

            var request = new RestRequest("/lists/05e228c8bb/members", Method.GET);

            request.AddParameter("name", "epicodus");

            var queryResult = client.Execute(request);
            Console.WriteLine(queryResult.Content);

            return View("Index");
        }

        
        public IActionResult SubmitEmail()
        {
            string email = Request.Form["email"];
            string firstName = Request.Form["firstName"];
            string lastName = Request.Form["lastName"];
            string subscribed = Request.Form["subscribed"];

            var client = new RestClient("https://us13.api.mailchimp.com/3.0/")
            {
                Authenticator = new HttpBasicAuthenticator("stemple87", "25c5d5915005c2d37ed810de0c897de7-us13")
            };


            RestRequest request = new RestRequest("lists/05e228c8bb/members/", Method.POST);

            var yourobject = new
            {
                email_address = email,
                status = subscribed,
                merge_fields= new
                {
                    FNAM= firstName,
                    LNAME = lastName
                }
            };
            var json = request.JsonSerializer.Serialize(yourobject);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //var queryResult = client.Execute(request);

            Console.WriteLine(response.Content);

            //var request = new RestRequest("/lists/05e228c8bb/members", Method.POST);



            return View("Index");
        }

    }
}
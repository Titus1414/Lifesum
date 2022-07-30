using Lifesum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lifesum.Controllers
{
    public class LoginController : Controller
    {

        public static string apiKey = "AIzaSyDI84FBWnciXiaVq78PXmeGD-Vt6rj6WKs";


        private IJwtAuthenticationManager jwtAuthenticationManager;

        public LoginController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }


        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        //private void SignInUser(string username, string token, bool isPersistent)
        //{
        //    var claims = new List<Claim>();

        //    try
        //    {
        //        claims.Add(new Claim(ClaimTypes.Email, username));
        //        claims.Add(new Claim(ClaimTypes.Authentication, token));

        //        var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

        //        //var ctx = Request.;
        //        //var authenticationManager = ctx.Authentication;

        //        //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        [HttpPost]
        public async Task<IActionResult> Sign(Login model)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));

                try
                {
                var a = await auth.SignInWithEmailAndPasswordAsync(model.username, model.password);
                //string token = a.FirebaseToken;

                var user = a.User;
                HttpContext.Session.SetString("userId", user.LocalId);
                if (user.LocalId != null)
                    {
                        return RedirectToAction("GetPlans", "Plans");
                    }
                    else
                    {
                        TempData["Msg"] = "Email or Password is not valid!";
                        return RedirectToAction("Login", "Login");
                    }
                }
                catch (Exception e)
                {
                    TempData["Msg"] = "Email or Password is not valid!";
                    return RedirectToAction("Login", "Login");
                }
            
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult Authenticate(Login model)
        //{

        //    var token = jwtAuthenticationManager.Authenticate(model.username, model.password);
        //    if (token == null)
        //        return RedirectToAction("Login", "Login");

        //    var sdf = token;
        //    return RedirectToAction("GetFood", "Food");
        //}

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToAction("Login", "Login");
        }
    }
}

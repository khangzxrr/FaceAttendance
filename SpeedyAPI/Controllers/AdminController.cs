﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SpeedyAPI.Models;

namespace SpeedyAPI.Controllers
{
    public class AdminController : Controller
    {
        private readonly string SESSION_ROLE = "role";

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SESSION_ROLE) == null)
            {
                return View("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("username, password")] Admin admin)
        {
            if (HttpContext.Session.GetString(SESSION_ROLE) != null)
            {
                return View("Index");
            }

            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            if (admin.username.Equals("admin") && admin.password.Equals("admin"))
            {
                HttpContext.Session.SetString(SESSION_ROLE, "admin");
                return View("Index");
            }
            else
            {
                ViewBag.error = "Invalid information";
                return View("Login");
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using PAWEventive.ApplicationLogic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAWEventive.Models.Users;

namespace PAWEventive.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

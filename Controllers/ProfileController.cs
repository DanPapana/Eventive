using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using PAWEventive.ApplicationLogic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAWEventive.Models.Users;
using Microsoft.AspNetCore.Authorization;

namespace PAWEventive.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}

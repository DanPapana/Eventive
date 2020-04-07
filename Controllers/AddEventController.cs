using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PAWEventive.Controllers
{
    public class AddEventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
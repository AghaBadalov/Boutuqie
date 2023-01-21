using BoutiqueLoginRegister.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BoutiqueLoginRegister.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
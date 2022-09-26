using EliApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EliApp.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace EliApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        UserManager<EliAppUser> userManager;

        public UserController(UserManager<EliAppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }
    }
}
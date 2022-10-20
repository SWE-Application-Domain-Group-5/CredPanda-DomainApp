using System.Threading.Tasks;
using EliApp.Areas.Identity.Data;
using EliApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EliApp.Controllers
{
    //I can't get this Authorize Statement to work properly with the roles. Don't know why.
    //For my tests, setting it to "Admin Only" restricted access completely
    //Setting it to anything else allowed free access sometimes. It's weird
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        UserManager<EliAppUser> userManager;

        public UserController(UserManager<EliAppUser> userManager)
        {
            this.userManager = userManager;
        }

        //Not function properbly ATM
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.isActive = true;
                userManager.UpdateAsync(user);
                return RedirectToAction("ListUsers");
                //return View("ListUsers");
            }
            else
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
        }

        //Not function properbly ATM
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                //if(user.isActive)
                user.isActive = false;
                await userManager.UpdateAsync(user);

                return RedirectToAction("ListUsers");
            }
            else
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (!user.isActive)
                {
                    return RedirectToAction("ListUsers");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description = "");
                }
                return View("ListUsers");
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var user = userManager.Users;
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            //var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.GeneratedUserName,
                Email = user.Email,
                Address = user.Address,
                DOB = user.DOB,
                RegisterDate = user.RegisterDate,
                isActive = user.isActive,
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Address = model.Address;

                var result = await userManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

                return View(model);
            }
        }
    }
}
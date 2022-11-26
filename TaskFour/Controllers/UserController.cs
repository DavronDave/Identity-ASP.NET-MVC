using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskFour.DataLayer;
using TaskFour.Models;
using TaskFour.ViewModels;

namespace TaskFour.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDbContext _dbContext;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, UserDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var us = new User
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    Name = viewModel.Name,
                    RegisterDate = viewModel.RegisterDate,
                    LoginTime = viewModel.LoginTime,
                    IsActive = viewModel.IsActive
                };

                var result = await _userManager.CreateAsync(us, viewModel.Password);
                if (result.Succeeded)
                {
                    var user1 = await _userManager.FindByNameAsync(viewModel.Email);
                    user1.IsActive = true;
                    await _userManager.UpdateAsync(user1);
                    await _signInManager.SignInAsync(us, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user1 = await _userManager.FindByEmailAsync(viewModel.Email);
                if (_userManager.Users.Contains(user1))
                {
                    if (user1.IsActive == true)
                    {
                        var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password,
                        viewModel.RememberMe, false);
                        if (result.Succeeded)
                        {
                            var user = await _userManager.FindByNameAsync(viewModel.Email);
                            user.LoginTime = DateTime.Now;
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Info", "User");
                        }
                        else
                            ModelState.AddModelError(String.Empty, "Username or password incorrect!");
                    }
                    else
                        ModelState.AddModelError(String.Empty, "You are blocked!");
                }
                else
                    ModelState.AddModelError(String.Empty, "Username not found!");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }


        [HttpGet]
        public IActionResult Info()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            var deletedUser = await _userManager.DeleteAsync(user);
            if (deletedUser.Succeeded)
            {
                return RedirectToAction("Info", "User");
            }

            return RedirectToAction("Info", "User");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(IEnumerable<string> deleteBlock)
        {
            var toCheckUser = await _userManager.GetUserAsync(User);
            var user1 = await _userManager.FindByIdAsync(toCheckUser.Id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var id in deleteBlock)
            {
                var user = await _userManager.FindByIdAsync(id);
                // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (id == userId)
                //{
                //    await _userManager.DeleteAsync(user);
                //    await _signInManager.SignOutAsync();
                //    return RedirectToAction("Login", "User");
                //}
                //else
                    await _userManager.DeleteAsync(user);
            }
            _dbContext.SaveChanges(); 
            var us = await _userManager.FindByEmailAsync(user1.Email);
            if(us == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }
            else
                return RedirectToAction("Info", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Block(IEnumerable<string> deleteBlock)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var id in deleteBlock)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
            var toCheckUser = await _userManager.GetUserAsync(User);
            if (toCheckUser.IsActive == false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Info", "User");
        }

        [HttpPost]
        public async Task<IActionResult> UnBlock(IEnumerable<string> deleteBlock)
        {
            foreach (var id in deleteBlock)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Info", "User");
        }
    }
}

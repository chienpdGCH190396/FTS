using FTS.EF;
using FTS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FTS.Controllers
{
    [Authorize(Roles = SecurityRole.Trainee)]
    public class TraineeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public TraineeController()
        {
        }

        public TraineeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Trainee
        public ActionResult Index()
        {
            GetName();
            return View();
        }

        // GET: /Trainee/UpdatePassword
        public ActionResult UpdatePassword()
        {
            
            return View();
        }

        //
        // POST: /Trainee/UpdatePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index");
            }
            AddErrors(result);
            
            return View(model);
        }

        // GET: /Trainee/UpdateProfile
        [HttpGet]
        public async Task<ActionResult> UpdateProfile()
        {
            var context = new UserContext();
            var store = new UserStore<UserEntity>(context);
            var manager = new UserManager<UserEntity>(store);

            var trainee = await manager.FindByIdAsync(User.Identity.GetUserId());

            if (trainee != null)
            {

                return View(trainee);
            }

            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: /Trainee/UpdateProfile
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UserEntity trainee)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var context = new UserContext();
                var store = new UserStore<UserEntity>(context);
                var manager = new UserManager<UserEntity>(store);

                var user = await manager.FindByIdAsync(User.Identity.GetUserId());

                if (user != null)
                {
                    user.Name = trainee.Name;
                    user.Age = trainee.Age;
                    user.DOB = trainee.DOB;
                    user.Education = trainee.Education;
                    user.MainProgrammingLanguage = trainee.MainProgrammingLanguage;
                    user.TOEIC = trainee.TOEIC;
                    user.Experience = trainee.Experience;
                    user.Department = trainee.Department;
                    user.Location = trainee.Location;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("Index");
            }
        }

        public void GetName()
        {
            var user = User.Identity;
            ViewBag.YourName = user.Name;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
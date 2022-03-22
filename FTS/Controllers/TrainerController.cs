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
    [Authorize(Roles = SecurityRole.Trainer)]
    public class TrainerController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public TrainerController()
        {
        }

        public TrainerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Trainer
        public ActionResult Index()
        {
            GetName();
            return View();
        }

        // GET: /Trainer/UpdatePassword
        public ActionResult UpdatePassword()
        { 
            return View();
        }

        // POST: /Trainer/UpdatePassword
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

        // GET: /Trainer/UpdateProfile
        [HttpGet]
        public async Task<ActionResult> UpdateProfile()
        {
            var context = new UserContext();
            var store = new UserStore<UserEntity>(context);
            var manager = new UserManager<UserEntity>(store);

            var trainer = await manager.FindByIdAsync(User.Identity.GetUserId());

            if (trainer != null)
            {
                
                return View(trainer);
            }

            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: /Trainer/UpdateProfile
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UserEntity trainer)
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
                    user.Name = trainer.Name;
                    user.Education = trainer.Education;
                    user.WorkingPlace = trainer.WorkingPlace;
                    user.Telephone = trainer.Telephone;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("Index", "Trainer");
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
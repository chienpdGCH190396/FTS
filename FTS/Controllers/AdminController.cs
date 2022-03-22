using FTS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FTS.Controllers
{
    [Authorize (Roles = SecurityRole.Admin)]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        ApplicationDbContext context;
        public AdminController()
        {
            context = new ApplicationDbContext();
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Admin
        public ActionResult Index()
        {
            GetName();
            return View();
        }

        //-------------------------- START: MANAGE STAFFS --------------------------
        public ActionResult AllStaffs()
        {
            GetName();
            using (var USCT = new EF.UserContext())
            {
                var st = new List<UserEntity>();
                var staffs = USCT.Users.OrderBy(t => t.Id).ToList();
                foreach (var s in staffs)
                {
                    if (s.Role == "Staff")
                    {
                        st.Add(s);
                    }
                }
                return View(st);
            }
        }

        [HttpGet]
        public ActionResult CreateStaff()
        {
            GetName();
            return View();
        }

        //
        // POST: /Admin/CreateStaff
        [HttpPost]
        public async Task<ActionResult> CreateStaff(CreateAccountForm model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Role = model.Role, Name = model.Name };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    return RedirectToAction("AllStaffs", "Admin");
                }
                GetName();
                AddErrors(result);

            }
            // If we got this far, something failed, redisplay form    
            return View(model);
        }

        [HttpGet]
        public ActionResult UpdatePasswordStaff(string id)
        {
            using (var USCT = new EF.UserContext())
            {
                var staff = USCT.Users.FirstOrDefault(b => b.Id == id);

                if(staff != null)
                {
                    return View(staff);
                }
                else
                {
                    return RedirectToAction("AllStaffs");
                }
            }
        }

        [HttpPost]
        public ActionResult UpdatePasswordStaff(string id, UserEntity staff)
        {
            if(!ModelState.IsValid)
            {
                return View(staff);
            }
            else
            {
                using (var USCT = new EF.UserContext())
                {
                    USCT.Entry<UserEntity>(staff).State = System.Data.Entity.EntityState.Modified;
                    USCT.SaveChanges();
                }
                return RedirectToAction("AllStaffs");

            }
        }

        [HttpPost]
        public ActionResult DeleteStaffAccount(string id)
        {
            using (var USCT = new EF.UserContext())
            {
                var staff = USCT.Users.FirstOrDefault(b => b.Id == id);

                if(staff != null)
                {
                    USCT.Users.Remove(staff);
                    USCT.SaveChanges();
                }
                return RedirectToAction("AllStaffs");
            }
        }
        //-------------------------- END: MANAGE STAFFS --------------------------

        //-------------------------- START: MANAGE TRAINERS --------------------------
        public ActionResult AllTrainers()
        {
            GetName();
            using (var USCT = new EF.UserContext())
            {
                var tn = new List<UserEntity>();
                var trainers = USCT.Users.OrderBy(t => t.Id).ToList();
                foreach (var s in trainers)
                {
                    if (s.Role == "Trainer")
                    {
                        tn.Add(s);
                    }
                }
                return View(tn);
            }
        }

        [HttpGet]
        public ActionResult CreateTrainer()
        {
            GetName();
            return View();
        }

        //
        // POST: /Admin/CreateTrainer
        [HttpPost]
        public async Task<ActionResult> CreateTrainer(CreateAccountForm model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Role = model.Role, Name = model.Name };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    return RedirectToAction("AllTrainers", "Admin");
                }
                GetName();
                AddErrors(result);

            }
            // If we got this far, something failed, redisplay form    
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteTrainerAccount(string id)
        {
            using (var USCT = new EF.UserContext())
            {
                var trainer = USCT.Users.FirstOrDefault(b => b.Id == id);

                if (trainer != null)
                {
                    USCT.Users.Remove(trainer);
                    USCT.SaveChanges();
                }
                return RedirectToAction("AllTrainers");
            }
        }
        //-------------------------- START: MANAGE TRAINERS --------------------------

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public void GetName()
        {
            var user = User.Identity;
            ViewBag.YourName = user.Name;
        }
    }
}
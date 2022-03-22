using FTS.EF;
using FTS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FTS.Controllers
{
    [Authorize(Roles = SecurityRole.Staff)]
    public class StaffController : Controller
    {
        UserContext db = new UserContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public StaffController()
        {
        }

        public StaffController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Staff
        public ActionResult Index()
        {
            GetName();
            return View();
        }

        [HttpGet]
        public ActionResult CreateTrainee()
        {
            return View();
        }

        //-------------------------- START: MANAGE TRAINEES --------------------------
        // POST: /Staff/CreateTrainee
        [HttpPost]
        public async Task<ActionResult> CreateTrainee(CreateTraineeForm model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Role = model.Role, Name = model.Name, Age = model.Age, DOB = model.DOB, Education = model.Education, MainProgrammingLanguage = model.MainProgrammingLanguage, TOEIC = model.TOEIC, Experience = model.Experience, Department = model.Department, Location = model.Location  };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    return RedirectToAction("AllTrainees", "Staff");
                }
                
                AddErrors(result);

            }
            // If we got this far, something failed, redisplay form    
            return View(model);
        }

        [HttpGet]
        public ActionResult AllTrainees()
        {
            using (var USCT = new EF.UserContext())
            {
                var te = new List<UserEntity>();
                var trainees = USCT.Users.OrderBy(t => t.Id).ToList();
                foreach (var s in trainees)
                {
                    if (s.Role == "Trainee")
                    {
                        te.Add(s);
                    }
                }
                return View(te);
            }
        }


        public ActionResult SearchTrainee(string option, string search)
        {
            if (option == "Name")
            {
                return View(db.Users.Where(x => x.Name.Contains(search) || search == null).ToList());
            }
            else if (option == "MainProgrammingLanguage")
            {
                return View(db.Users.Where(x => x.MainProgrammingLanguage.Contains(search) || search == null).ToList());
            }
            else
            {
                return View(db.Users.Where(x => x.TOEIC == search || search == null).ToList());
            }
        }

        [HttpGet]
        public ActionResult UpdateTrainee(string id)
        {
            using (var USCT = new EF.UserContext())
            {
                var trainee = USCT.Users.FirstOrDefault(b => b.Id == id);

                if (trainee != null)
                {
                    return View(trainee);
                }
                else
                {
                    return RedirectToAction("AllTrainees");
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateTrainee(string id, UserEntity trainee)
        {
            if (!ModelState.IsValid)
            {
                return View(trainee);
            }
            else
            {
                using (var USCT = new EF.UserContext())
                {
                    USCT.Entry<UserEntity>(trainee).State = System.Data.Entity.EntityState.Modified;
                    USCT.SaveChanges();
                }
                return RedirectToAction("AllTrainees");
            }
        }

        [HttpPost]
        public ActionResult DeleteTraineeAccount(string id)
        {
            using (var USCT = new EF.UserContext())
            {
                var trainee = USCT.Users.FirstOrDefault(b => b.Id == id);

                if (trainee != null)
                {
                    USCT.Users.Remove(trainee);
                    USCT.SaveChanges();
                }
                return RedirectToAction("AllTrainees");
            }
        }
        //-------------------------- END: MANAGE TRAINEES --------------------------

        //-------------------------- START: MANAGE TRAINERS --------------------------
        public ActionResult AllTrainers()
        {
            
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
        public ActionResult UpdateTrainer(string id)
        {
            using (var USCT = new EF.UserContext())
            {
                var trainer = USCT.Users.FirstOrDefault(b => b.Id == id);

                if (trainer != null)
                {
                    return View(trainer);
                }
                else
                {
                    return RedirectToAction("AllTrainers");
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateTrainer(string id, UserEntity trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            else
            {
                using (var USCT = new EF.UserContext())
                {
                    USCT.Entry<UserEntity>(trainer).State = System.Data.Entity.EntityState.Modified;
                    USCT.SaveChanges();
                }
                return RedirectToAction("AllTrainers");
            }
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
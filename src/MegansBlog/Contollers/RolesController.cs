using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc.Rendering;
using MegansBlog.Models;

namespace MegansBlog2.Controllers
{

    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RolesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        // GET: /<controller>/
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }

        // Get: /Roles/Create
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Post: /Roles/Create
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create(string RoleName)
        {

            try
            {
                _db.Roles.Add(new IdentityRole()
                {
                    Name = RoleName
                });
                _db.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        //Post: Delete
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(string RoleName)
        {
            var thisRole = _db.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            _db.Roles.Remove(thisRole);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Roles/Edit
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(string roleName)
        {
            var thisRole = _db.Roles
                .Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            return View(thisRole);
        }

        //Post: Roles/Edit
        [HttpPost, ActionName("Edit")]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult EditRole(string roleName)
        {
            try
            {
                var currentRole = _db.Roles.FirstOrDefault(m => m.Name == roleName);
                currentRole.Name = Request.Form["Name"];
                _db.Roles.Update(currentRole);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Get: Roles/ManageUserRoles
        //[Authorize(Roles = "Admin")]
        public IActionResult ManageUserRoles()
        {
            var list = _db.Roles.OrderBy(r => r.Name)
                .ToList()
                .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;

            return View();
        }

        //Post: RolesManageUserRoles
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var user = GetUser(UserName);
            var role = _userManager.AddToRoleAsync(user, RoleName).Result;
            return RedirectToAction("ManageUserRoles");
        }

        public ApplicationUser GetUser(string UserName)
        {
            return _userManager.Users
                .FirstOrDefault(m => m.UserName == UserName);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult GetRoles(string UserName)
        {

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = _db.Users
                    .Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = _userManager.GetRolesAsync(user).Result;

                var list = _db.Roles
                    .OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                ViewBag.Roles = list;
            }
            return View("ManageUserRoles");
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string RoleName, string UserName)
        {
            var role = _userManager.RemoveFromRoleAsync(GetUser(UserName), RoleName).Result;

            return RedirectToAction("ManageUserRoles");
        }

    }
}
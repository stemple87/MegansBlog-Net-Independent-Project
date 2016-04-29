using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using MegansBlog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System.Security.Claims;

namespace MegansBlog.Controllers
{
    [Authorize]

    public class CommentController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db
        )
        {
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            ViewBag.PicId = id;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Comment comment, int id)
        {
            comment.PostId = id;
            comment.CommentDate = DateTime.Now;
            //comment.User = _userManager.FindByIdAsync(User.GetUserId());
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return RedirectToAction("Index", "Post");
        }

        public IActionResult Edit(int id)
        {
            var thisComment = _db.Comments.FirstOrDefault(comments => comments.CommentId == id);
            return View(thisComment);
        }

        [HttpPost]
        public IActionResult Edit(Comment comment)
        {
            //var currentUser = await _userManager.FindByIdAsync(User.GetUserId());
            //comment.User = currentUser;
            _db.Entry(comment).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index", "Post");
        }

        public IActionResult Delete(int id)
        {
            var thisComment = _db.Comments.FirstOrDefault(comments => comments.CommentId == id);
            return View(thisComment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            //var currentUser = await _userManager.FindByIdAsync(User.GetUserId());
            //comment.User = currentUser;
            var thisComment = _db.Comments.FirstOrDefault(comments => comments.CommentId == id);
            _db.Comments.Remove(thisComment);
            _db.SaveChanges();
            return RedirectToAction("Index", "Post");
        }
    }
}
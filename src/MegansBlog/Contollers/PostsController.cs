using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MegansBlog.Models;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MegansBlog.Contollers
{
    
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db
        )
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Posts.Include(posts => posts.Category).ToList());
        }
        public IActionResult Details(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            Category newCategory = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
            thisPost.Category = newCategory;
            thisPost.Comments = _db.Comments.Where(x => x.PostId == id).ToList();
            return View(thisPost);
        }
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Post post)
        {
            DateTime timeStamp = DateTime.Now;
            post.PostDate = timeStamp;
            _db.Posts.Add(post);

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View(thisPost);
        }
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            _db.Entry(post).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            _db.Posts.Remove(thisPost);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

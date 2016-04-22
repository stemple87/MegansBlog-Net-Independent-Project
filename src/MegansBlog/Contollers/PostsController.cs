using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MegansBlog.Models;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MegansBlog.Contollers
{
    public class PostsController : Controller
    {
        private MegansBlogContext db = new MegansBlogContext();
        public IActionResult Index()
        {
            return View(db.Posts.Include(posts => posts.Category).ToList());
        }
        public IActionResult Details(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            Category newCategory = db.Categories.FirstOrDefault(x => x.CategoryId == id);
            thisPost.Category = newCategory;
            return View(thisPost);
        }
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Post post)
        {
            DateTime timeStamp = DateTime.Now;
            post.PostDate = timeStamp;
            db.Posts.Add(post);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View(thisPost);
        }
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            db.Posts.Remove(thisPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

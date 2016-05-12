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
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MegansBlog.Contollers
{
    
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;

        public PostsController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db,
            IHostingEnvironment environment
        )
        {
            _userManager = userManager;
            _db = db;
            _environment = environment;
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

        //GET: Post/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        //POST: Post/Create
        //[HttpPost]
        //public async Task<ActionResult> Create(ICollection<IFormFile> files)
        //{
        //    DateTime timeStamp = DateTime.Now;
        //    Post post = new Post();
        //    post.Title = Request.Form["Title"];
        //    post.Body = Request.Form["Body"];
        //    post.PostDate = timeStamp;
        //    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
        //    string fileName;
        //    foreach (var file in files)
        //    {
        //        if (file.Length > 0)
        //        {
        //            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            await file.SaveAsAsync(Path.Combine(uploads, fileName));
        //            post.Image = "/uploads/" + fileName;
        //            break;
        //        }
        //    }
        //    _db.Posts.Add(post);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> Create(ICollection<IFormFile> files)
        {
            DateTime timeStamp = DateTime.Now;
            Post post = new Post();
            post.Title = Request.Form["Title"];
            post.Body = Request.Form["Body"];
            post.PostDate = timeStamp;
            //post.User = await _userManager.FindByIdAsync(User.GetUserId());
            //var currentUser = await _userManager.FindByIdAsync(User.GetUserId());
            //image.User = currentUser;
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            string fileName;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    await file.SaveAsAsync(Path.Combine(uploads, fileName));
                    post.Image = "/uploads/" + fileName;
                    break;
                }
            }

            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
            //return View();
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

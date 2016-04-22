using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MegansBlog.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MegansBlog.Contollers
{
    public class PostsController : Controller
    {
        // GET: /<controller>/
        private MegansBlogContext db = new MegansBlogContext();
        public IActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        public IActionResult Details(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
    }
}

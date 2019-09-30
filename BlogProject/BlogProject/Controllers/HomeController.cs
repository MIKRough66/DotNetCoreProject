using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;

using BlogProject.Data;
using BlogProject.Data.Repository;
using BlogProject.Data.FileManager;
using BlogProject.ViewModels;
using BlogProject.Models.Comments;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public IActionResult Index(string category) => 
            View(string.IsNullOrEmpty(category) ? 
                _repo.GetAllPosts() : 
                _repo.GetAllPosts(category));
        
        public IActionResult Post(int id) =>
            View(_repo.GetPost(id));
        

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image) => 
            new FileStreamResult(
                _fileManager.ImageStream(image), 
                $"image/{image.Substring(image.LastIndexOf('.') + 1)}"
                );


        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return Post(vm.PostId);
            }

            var post = _repo.GetPost(vm.PostId);
            if (vm.MainCommentId > 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });
                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };
            }
            await _repo.SaveChangesAsync();

            return View();
        }





        //public IActionResult Index(string category)
        //{
        //    var posts = string.IsNullOrEmpty(category) ? _repo.GetAllPosts() : _repo.GetAllPosts(category);
        //    return View(posts);
        //}


        //public IActionResult Post(int id)
        //{
        //    var post = _repo.GetPost(id);

        //    return View(post);
        //}

        //[HttpGet("/Image/{image}")]
        //public IActionResult Image(string image)
        //{
        //    var mime = image.Substring(image.LastIndexOf('.') + 1);
        //    return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        //}
    }
}

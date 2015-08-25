using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Home;
using MongoDB.Bson;
using System.Linq.Expressions;
using M101DotNet.WebApp.Services;

namespace M101DotNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
        #region Protected properties

        protected PostService _postService;

        #endregion

        #region HomeController Constructor

        /// <summary>
        /// 
        /// </summary>
        public HomeController()
        {
            _postService = new PostService();
        }

        #endregion

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            // var blogContext = new BlogContext();  // Not neede here any more

            // XXX WORK HERE
            // find the most recent 10 posts and order them
            // from newest to oldest

            var listOfRecentPosts = await _postService.GetRecentPosts();

            var recentPosts = new IndexModel
            {
                RecentPosts = listOfRecentPosts
            };

            return View(recentPosts);
        }

        #endregion

        #region GET NewPost

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        #endregion

        #region POST NewPost

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // var blogContext = new BlogContext();  // Not neede here any more

            // XXX WORK HERE
            // Insert the post into the posts collection
            var newPost = new Post
            {
                Title = model.Title,
                Content = model.Content,
                Author = this.User.Identity.Name,
                CreatedAtUtc = DateTime.UtcNow,
                Tags = model.Tags.Split(',').ToList(),
                Comments = new List<Comment>() // Need to initialise a new list of comments here, as it comes empty from the database when we're adding a new post
            };

            /*
            var tagList = new List<string>();
            tagList.AddRange(model.Tags.Split(',').ToList());
            newPost.Tags = tagList;
            */

            var post = await _postService.InsertNewPost(newPost);

            return RedirectToAction("Post", new {id = post.Id});
        }

        #endregion

        #region GET Post

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Post(string id)
        {
            // var blogContext = new BlogContext();  // Not neede here any more

            // XXX WORK HERE
            // Find the post with the given identifier

            var postId = new ObjectId(id);
            var post = await _postService.FindPost(postId);

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PostModel
            {
                Post = post
            };

            return View(model);
        }

        #endregion

        #region GET Posts

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Posts(string tag = null)
        {
            // var blogContext = new BlogContext();  // Not neede here any more

            // XXX WORK HERE
            // Find all the posts with the given tag if it exists.
            // Otherwise, return all the posts.
            // Each of these results should be in descending order.

            var posts = await _postService.FindPostsByTag(tag);

            //return System.Web.UI.WebControls.View(posts);
            return View(posts);
        }

        #endregion

        #region POST NewComment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> NewComment(NewCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new {id = model.PostId});
            }

            // var blogContext = new BlogContext();  // Not neede here any more

            // XXX WORK HERE
            // add a comment to the post identified by model.PostId.
            // you can get the author from "this.User.Identity.Name"

            var newComment = new Comment()
            {
                Author = this.User.Identity.Name,
                Content = model.Content,
                CreatedAtUtc = DateTime.UtcNow
            };

            var result = await _postService.AddCommentToPost(newComment, new ObjectId(model.PostId));

            return RedirectToAction("Post", new {id = model.PostId});
        }

        #endregion
    }
}
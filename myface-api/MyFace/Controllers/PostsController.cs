using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Models.Database;
using MyFace.Helpers;
using MyFace.Repositories;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using Microsoft.AspNetCore.Http;
using MyFace.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        public PostsController(IAuthService authService, IPostsRepo posts, IUsersRepo users)
        {
            _authService = authService;
            _posts = posts;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] CreatePostRequest newPost,
            [FromHeader] string authorization
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authHeader = Request.Headers["Authorization"];
            var password = PasswordHelper.GetPasswordFromHeader(authHeader);
            var username = PasswordHelper.GetUsernameFromHeader(authHeader);

            if (!_authService.IsValidUsernameAndPassword(username, password))
            {
                return Unauthorized("The username and password are not valid");
            }

            User user = _users.GetByUsername(username);

            if (user.Id != newPost.UserId)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not allowed to create a post for a different user"
                );
            }

            var post = _posts.Create(newPost);

            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authHeader = Request.Headers["Authorization"];
            var password = PasswordHelper.GetPasswordFromHeader(authHeader);
            var username = PasswordHelper.GetUsernameFromHeader(authHeader);

            

            if (!_authService.IsValidUsernameAndPassword(username, password))
            {
                Console.WriteLine(password);
                Console.WriteLine(username);
                return Unauthorized("The username and password are not valid");
            }
            
            User user = _users.GetByUsername(username);
            Post currentPost = _posts.GetById(id);
            
            if (user.Id != currentPost.UserId)
            {
               
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not allowed to update a post for a different user"
                );
            }

           var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {

            var authHeader = Request.Headers["Authorization"];
            var password = PasswordHelper.GetPasswordFromHeader(authHeader);
            var username = PasswordHelper.GetUsernameFromHeader(authHeader);

            

            if (!_authService.IsValidUsernameAndPassword(username, password))
            {
                
                return Unauthorized("The username and password are not valid");
            }
            
            User user = _users.GetByUsername(username);
            Post currentPost = _posts.GetById(id);
            
            if (user.Id != currentPost.UserId)
            {
               
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not allowed to delete a post for a different user"
                );
            }

            _posts.Delete(id);
            return Ok();
        }
    }
}
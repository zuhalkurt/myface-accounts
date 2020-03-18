using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Services;

namespace MyFace.Controllers
{
    [Route("feed")]
    public class FeedController : Controller
    {
        private readonly IPostsRepo _posts;
        private readonly IAuthService _authService;

        public FeedController(IPostsRepo posts, IAuthService authService)
        {
            _posts = posts;
            _authService = authService;
        }
        
        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest)
        {
            if (!_authService.HasValidAuthorization(Request))
            {
                return Unauthorized();
            }
            
            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
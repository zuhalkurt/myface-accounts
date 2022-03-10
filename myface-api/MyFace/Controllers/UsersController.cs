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
    [Route("/users")]
    public class UsersController : ControllerBase
    {
         private readonly IAuthService _authService;
        private readonly IUsersRepo _users;

        public UsersController(IUsersRepo users, IAuthService authService)
        {
            _authService = authService;
            _users = users;
        }
        
        [HttpGet("")]
        public ActionResult<UserListResponse> Search([FromQuery] UserSearchRequest searchRequest)
        {
            var users = _users.Search(searchRequest);
            var userCount = _users.Count(searchRequest);
            return UserListResponse.Create(searchRequest, users, userCount);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetById([FromRoute] int id)
        {
            var user = _users.GetById(id);
            return new UserResponse(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateUserRequest newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _users.Create(newUser);

            var url = Url.Action("GetById", new { id = user.Id });
            var responseViewModel = new UserResponse(user);
            return Created(url, responseViewModel);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<UserResponse> Update([FromRoute] int id, [FromBody] UpdateUserRequest update)
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

            User currentUser = _users.GetByUsername(username);

            if (user.Id != user.Id)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not allowed to create a post for a different user"
                );
            }

            var user = _users.Update(id, update);
            return new UserResponse(user);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _users.Delete(id);
            return Ok();
        }
    }
}
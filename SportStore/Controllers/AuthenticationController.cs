using SportStore.Models.Results;
using SportStore.Models.Dtos;
using SportStore.Managers;
using SportStore.Managers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SportStore.Controllers
{
    [Authorize]
    [Route("/auth")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ControllerName("Authentication Version 1.0 & 2.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(IAuthenticationManager authenticationService)
        {
            _authenticationManager = authenticationService;
        }

        /// <summary>
        /// Login and Request Token
        /// </summary>
        /// <param name="credentials"></param>
        /// <response code="200">User logged in successfully.</response>
        /// <response code="400">The inputs supplied to the API are invalid!.</response> 
        /// <response code="default">Default !.</response> 
        /// <returns>The created <see cref="BadRequestResult"/> for the response.</returns>
        /// <remarks>
        /// Schema:
        /// 
        ///     {
        ///         "username": "user01",
        ///         "password": "user01"
        ///     }
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(LoggedUserResult))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _authenticationManager.UserLoginAsync(credentials);

            if (result.Success is false)
                return NotFound(new { Message = result.ErrorMessage });

            //return Ok(new { Token = await _authenticationManager.CreateToken() });

            var loggedUser = new LoggedUserResult
            {
                Username = result.Username,
                Email = result.Email,
                Token = result.Token
            };

            return Ok(loggedUser);
        }

        /// <summary>
        /// Register new user !..
        /// </summary>
        /// <param name="userDTO"></param>
        /// <response code="201">User created successfully and logged in.</response>
        /// <response code="400">The inputs supplied to the API are invalid!.</response> 
        /// <response code="default">Default !.</response> 
        /// <returns></returns>
        /// <remarks>
        /// Schema:
        /// 
        ///     {
        ///         "username": "user00",
        ///         "password": "123456",
        ///         "email": "example@domain.com"
        ///     }
        /// </remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SignUp([FromBody] UserAddDTO userDTO)
        {
            if (ModelState.IsValid is false) { return BadRequest(); }

            var result = await _authenticationManager.RegistrationAsync(userDTO);

            if (result.Success is false)
            {
                return BadRequest(result);
            }

            var loggedUser = new LoggedUserResult
            {
                Username = result.Username,
                Email = result.Email,
                Token = result.Token
            };

            return CreatedAtAction( nameof(Login), new { Username = userDTO.Username }, loggedUser );
        }
    }
}

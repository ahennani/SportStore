using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportStore.Managers.Repositories;
using SportStore.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportStore.Controllers
{
    [Route("/admins")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public AdministrationController(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: /admins/users
        [HttpGet("users")]
        //[Authorize("Admin")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<UserResult>))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<UserResult>>> ListUsersAsync()
        {
            var users = await _userManager.GetUsers();

            var userResults = _mapper.Map<IEnumerable<UserResult>>(users);
            
            return Ok(userResults);
        }
    
    
    }
}

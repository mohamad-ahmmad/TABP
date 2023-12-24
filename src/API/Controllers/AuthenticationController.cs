using Application.Users.DTOs;
using Application.Users.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AuthUserByCredentials(LoginCredentialsDto loginCredentialsDto, CancellationToken cancellationToken)
        {
            var loginQuery = new LoginUserQuery(loginCredentialsDto);
            var result = await _mediator.Send(loginQuery, cancellationToken);
            if (result.IsFailure)
            {
                return BadRequest(new
                {
                    result.Errors
                });
            }

            return Ok(result.Response);
        }
    }
}

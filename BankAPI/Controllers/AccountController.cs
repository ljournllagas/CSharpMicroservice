using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    public class AccountController : ApiControllerBase
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="command">Info of Account</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAccountAsync([FromBody] CreateAccountCommand command)
        {
            return Ok(await CommandAsync(command));
        }
    }
}
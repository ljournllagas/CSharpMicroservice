using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    public class ClientController : ApiControllerBase
    {
        public ClientController(IMediator mediator) : base(mediator)
        {
        }


        /// <summary>
        /// Create new client
        /// </summary>
        /// <param name="command">Info of client</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateClientAsync([FromBody] CreateClientCommand command)
        {
            return Ok(await CommandAsync(command));
        }
    }
}
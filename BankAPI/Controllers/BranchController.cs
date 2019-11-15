using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    public class BranchController : ApiControllerBase
    {
        public BranchController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Create new branch
        /// </summary>
        /// <param name="command">Info of branch</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateBranchAsync([FromBody] CreateBranchCommand command)
        {
            return Ok(await CommandAsync(command));
        }
    }
}
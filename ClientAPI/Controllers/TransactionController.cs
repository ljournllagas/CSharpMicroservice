using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientAPI.Commands;
using ClientAPI.Dto;
using ClientAPI.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClientAPI.Controllers
{
    public class TransactionController : ApiControllerBase
    {
        public TransactionController(IMediator mediator) : base(mediator)
        {
        }


        [HttpPost]
        [Route("Withdraw")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> WithdrawAsync([FromBody] WithdrawCommand command)
        {
            return Ok(await CommandAsync(command));
        }

        [HttpPost]
        [Route("Deposit")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DepositAsync([FromBody] DepositCommand command)
        {
            return Ok(await CommandAsync(command));
        }


        [HttpPost]
        [Route("TransferFund")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> TransferFundAsync([FromBody] TransferFundCommand command)
        {
            return Ok(await CommandAsync(command));
        }


        [HttpGet("{accountNumber}")]
        [ProducesResponseType(typeof(TransactionHistoryDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TransactionHistoryDto>> GetTransactionHistoryAsync(string accountNumber)
        {
            return Single(await QueryAsync(new GetTransactionHistoryQuery(accountNumber)));
        }


    }
}
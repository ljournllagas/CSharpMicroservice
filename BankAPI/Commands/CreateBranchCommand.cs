using BankAPI.Dto;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Commands
{
    public class CreateBranchCommand : IRequest<BranchDto>
    {
        public CreateBranchCommand()
        {

        }

        [JsonConstructor]
        public CreateBranchCommand(string branchCode, string address)
        {
            BranchCode = branchCode;
            Address = address;
        }

        public string BranchCode { get; set; }

        public string Address { get; set; }
    }
}

using ClientAPI.Dto;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Query
{
    public class GetTransactionHistoryQuery : IRequest<TransactionHistoryDto>
    {
        public GetTransactionHistoryQuery()
        {
        }

        [JsonConstructor]
        public GetTransactionHistoryQuery(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        [Required]
        [MinLength(12)]
        [MaxLength(15)]
        public string AccountNumber { get; set; }
    }
}

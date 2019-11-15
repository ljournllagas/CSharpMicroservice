using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [JsonProperty("firstName")]
        public string ClientFirstName { get; set; }

        [JsonProperty("lastName")]
        public string ClientLastName { get; set; }

        [JsonProperty("address")]
        public string ClientAddress { get; set; }

        [JsonProperty("emailAddress")]
        public string ClientEmail { get; set; }

        [JsonProperty("branchCode")]
        public string BranchBranchCode { get; set; }
        public string TypeOfAccount { get; set; }
        public string AccountNumber { get; set; }
        public double InitialBalance { get; set; }

    }
}

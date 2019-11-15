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
    public class CreateClientCommand : IRequest<ClientDto>
    {
        public CreateClientCommand()
        {

        }

        [JsonConstructor]
        public CreateClientCommand(string firstName, string lastName, string address, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Email = email;
        }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(20)]
        public string Email { get; set; }
    }
}

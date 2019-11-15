using AutoMapper;
using BankAPI.Commands;
using BankAPI.Dto;
using BankAPI.Events;
using BankAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.MapperConfig
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<CreateAccountCommand, Account>();

            CreateMap<Account, AccountCreated>()
              .ForMember(target => target.Id, options => options.MapFrom(source => source.Id));

            CreateMap<Account, AccountDto>()
                .ForMember(target => target.Id, options => options.MapFrom(source => source.Id));
        }
    }
}

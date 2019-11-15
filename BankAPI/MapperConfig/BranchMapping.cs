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
    public class BranchMapping : Profile
    {
        public BranchMapping()
        {
            CreateMap<CreateBranchCommand, Branch>();
            CreateMap<Branch, BranchCreated>();
            CreateMap<Branch, BranchDto>();
        }
    }
}

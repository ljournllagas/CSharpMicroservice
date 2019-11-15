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
    public class ClientMapping : Profile
    {
        public ClientMapping()
        {
            CreateMap<CreateClientCommand, Client>();
            CreateMap<Client, ClientCreated>();
            CreateMap<Client, ClientDto>();
        }
    }
}

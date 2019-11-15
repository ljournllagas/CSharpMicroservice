using AutoMapper;
using ClientAPI.Dto;
using ClientAPI.Events;
using ClientAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.MapperConfig
{
    public class TransactionMapping : Profile
    {
        public TransactionMapping()
        {
            CreateMap<Account, Transaction>()
                .ForMember(dest => dest.RemainingBalance, opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.ClientCode, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Transaction, TransactionCreated>()
                 .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.RemainingBalance))
                 .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Transaction, WithdrawDto>()
                  .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Transaction, DepositDto>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id));
        }
    }
}

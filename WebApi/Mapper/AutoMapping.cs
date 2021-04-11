using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Model;
using WebApi.Utilities;

namespace WebApi.Mapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PaymentModel, CreditCardDetails>(); // means you want to map from User to UserDTO
            CreateMap<PaymentModel, PaymentState>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.ToString()))
                .ForMember(dest => dest.TransactionId, opt => Guid.NewGuid())
                .ForMember(dest => dest.CreditCardNo, opt => opt.MapFrom(src => src.CreditCardNumber.ToString()));
        }
    }
}

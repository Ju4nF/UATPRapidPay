using UATP.RapidPay.Data.Models.DTOs;
using UATP.RapidPay.Data.Models;
using AutoMapper;

namespace UATP.RapidPay.Data
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Card, CardDTO>().ReverseMap();
            CreateMap<CardCreateDTO, Card>();
            CreateMap<RegistrationRequestDTO, LocalUser>();
        }
    }
}

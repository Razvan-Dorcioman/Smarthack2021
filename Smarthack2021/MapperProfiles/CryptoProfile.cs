using AutoMapper;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Dto;

namespace Smarthack2021.MapperProfiles
{
    public class CryptoProfile : Profile
    {
        public CryptoProfile()
        {
            CreateMap<PasswordDto, PasswordObject>()
                .ForMember(p => p.User, from => from.Ignore())
                .ForMember(p => p.EncryptedPassword, from => from.MapFrom(x => x.Password));

            CreateMap<PasswordObject, PasswordDto>()
                .ForMember(p => p.Password, from => from.MapFrom(x => x.EncryptedPassword));

            CreateMap<PasswordGeneratorDto, PasswordGenerator>().ReverseMap();
        }
    }
}
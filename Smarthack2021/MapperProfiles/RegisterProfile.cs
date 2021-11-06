using AutoMapper;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smarthack2021.MapperProfiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterInfoDto, User>()
                .ReverseMap();
        }
    }
}

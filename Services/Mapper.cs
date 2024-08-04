using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BACKEND_ASP.NET_WEB_API.DTOs;
using BACKEND_ASP.NET_WEB_API.Models;

namespace BACKEND_ASP.NET_WEB_API.Services
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
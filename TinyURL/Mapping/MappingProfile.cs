using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyURL.Models;
using TinyURL.ViewModels;

namespace TinyURL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<URLMapViewModel, URLMap>();
        }
    }
}

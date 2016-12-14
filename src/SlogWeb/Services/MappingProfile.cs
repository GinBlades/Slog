using AutoMapper;
using SlogWeb.FormObjects;
using SlogWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlogWeb.Services {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Post, PostFormObject>();
            CreateMap<PostFormObject, Post>();
        }
    }
}

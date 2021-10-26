using AutoMapper;
using LibraryManager.DTOS;
using LibraryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManager.Profiles
{
    public class LibrariesProfile : Profile
    {
        public LibrariesProfile()
        {
            //reading Source -> Target
            CreateMap<Library,LibraryReadDTO>();

            //creating 
            CreateMap<LibraryCreateDTO, Library>();

            //updating
            CreateMap<LibraryUpdateDTO, Library>();

            //patching (partial update)
            CreateMap<Library, LibraryUpdateDTO>();
        }
    }
}

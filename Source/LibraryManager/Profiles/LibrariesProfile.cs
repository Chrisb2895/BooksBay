using AutoMapper;
using DAL.Entities;
using LibraryManager.DTOS;

namespace LibraryManager.Profiles
{
    public class LibrariesProfile : Profile
    {
        public LibrariesProfile()
        {
            //reading Source -> Target
            CreateMap<Library, LibraryReadDTO>();

            //creating 
            CreateMap<LibraryCreateDTO, Library>();

            //updating
            CreateMap<LibraryUpdateDTO, Library>();

            //patching (partial update)
            CreateMap<Library, LibraryUpdateDTO>();
        }
    }
}

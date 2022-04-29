using AutoMapper;
using DAL.DTOS;
using DAL.Entities;


namespace DAL.Profiles
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

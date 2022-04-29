using DAL.Entities;

namespace DAL.Functions.Interfaces
{
    public interface ILibraryOperations
    {
        public Task<Library> AddLibrary(Library library);
    }
}

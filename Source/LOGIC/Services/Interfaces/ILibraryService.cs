using DAL.Entities;
using LOGIC.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOGIC.Services.Interfaces
{
    public interface ILibraryService
    {
        public Task<GenericResultSet<Library>> AddSingleLibrary(Library lib);

        public Task<GenericResultSet<List<Library>>> GetAllLibraries();

        public Task<GenericResultSet<List<Library>>> GetPagedLibraries(int page, int pageSize);

        public Task<GenericResultSet<Library>> GetLibraryByID(int libID);

        public Task<GenericResultSet<Library>> UpdateLibrary(Library lib, int libID);

        public Task<GenericResultSet<Library>> DeleteLibrary(int libID);
    }
}

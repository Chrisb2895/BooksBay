using DAL.Entities;
using LOGIC.Services.Models;
using LOGIC.Services.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Services.Interfaces
{
    public interface ILibraryService
    {
        public Task<GenericResultSet<LibraryResultSet>> AddSingleLibrary(Library lib);

        public Task<GenericResultSet<List<LibraryResultSet>>> GetAllLibraries();

        public Task<GenericResultSet<LibraryResultSet>> GetLibraryByID(int libID);

        public Task<GenericResultSet<LibraryResultSet>> UpdateLibrary(Library lib,int libID);
    }
}

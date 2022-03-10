using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Functions.Interfaces
{
    public interface ILibraryOperations
    {
        public Task<Library> AddLibrary(Library library);
    }
}

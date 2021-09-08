using LibraryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManager.Data
{
    public interface ILibraryRepo
    {
        bool SaveChanges();

        IEnumerable<Library> GetLibraries();

        IEnumerable<Library> GetLibraries(LibraryParameters libParam);

        Library GetLibrariesById(int id);

        void CreateLibrary(Library lib);

        void UpdateLibrary(Library lib);

        void DeleteLibrary(Library lib);
    }
}

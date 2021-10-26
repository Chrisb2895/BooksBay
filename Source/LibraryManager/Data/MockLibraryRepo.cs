using LibraryManager.Helpers;
using LibraryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManager.Data
{
    public class MockLibraryRepo : ILibraryRepo
    {
        public void CreateLibrary(Library lib)
        {
            throw new NotImplementedException();
        }


        public void DeleteLibrary(Library lib)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Library> GetLibraries()
        {
            var libs = new List<Library> {

                new Library() { Id = 1, Name = "Christian's Library", City = "Sondrio" },
                new Library() { Id = 2, Name = "Pio Rajna's Library", City = "Sondrio" },
                new Library() { Id = 3, Name = "New York Public Library", City = "New York" }

            };

            return libs;
        }

        public IEnumerable<Library> GetLibraries(LibraryParameters libParam)
        {
            throw new NotImplementedException();
        }

        public Library GetLibrariesById(int id)
        {
            return new Library()
            {
                Id = 1,
                Name = "Christian's Library",
                City = "Sondrio"
            };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateLibrary(Library lib)
        {
            throw new NotImplementedException();
        }

        PagedList<Library> ILibraryRepo.GetLibraries(LibraryParameters libParam)
        {
            throw new NotImplementedException();
        }
    }
}

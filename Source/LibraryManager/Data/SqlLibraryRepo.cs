using DAL.Entities;
using LibraryManager.Helpers;
using LibraryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManager.Data
{
    public class SqlLibraryRepo : ILibraryRepo
    {
        private readonly LibraryContext _context;

        public SqlLibraryRepo(LibraryContext context)
        {
            _context = context;
        }

        public void CreateLibrary(Library lib)
        {
            if (lib == null)
                throw new ArgumentNullException(nameof(lib));

            _context.Libraries.Add(lib);
        }

        public void DeleteLibrary(Library lib)
        {
            if (lib == null)
                throw new ArgumentNullException(nameof(lib));

            _context.Remove(lib);
        }

        public IEnumerable<Library> GetLibraries()
        {
            return _context.Libraries.ToList();
        }

        public PagedListHelper<Library> GetLibraries(LibraryParameters libParam)
        {
            return PagedListHelper<Library>.ToPagedList(_context.Libraries.OrderBy(l => l.Id),
                libParam.PageNumber, libParam.PageSize);
        }

        public Library GetLibrariesById(int id)
        {
            return _context.Libraries.FirstOrDefault(lib => lib.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateLibrary(Library lib)
        {
            //Nothing
        }
    }
}

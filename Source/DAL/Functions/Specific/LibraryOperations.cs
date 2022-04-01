using DAL.DataContext;
using DAL.Entities;
using DAL.Functions.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Functions.Specific
{
    public class LibraryOperations : ILibraryOperations
    {
        private DatabaseContext _dbContext;

        public LibraryOperations(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Library> AddLibrary(Library library)
        {
            try
            {

                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var addedLibrary = await _dbContext.Libraries.AddAsync(library);
                        await _dbContext.SaveChangesAsync();
                        //il commit e la transazione non servirebbero in quanto sto aggiungendo solo 1 record di 1 tabella
                        //ma per il futuro, appena aumenta la complessità,  ho il codice pronto
                        await transaction.CommitAsync();
                        return library;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

            }
            catch
            {

                throw;
            }
        }


    }
}

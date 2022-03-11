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
        public async Task<Library> AddLibrary(Library library)
        {
            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                {
                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var addedLibrary = await context.Libraries.AddAsync(library);
                            await context.SaveChangesAsync();
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
            }
            catch
            {

                throw;
            }
        }

        
    }
}

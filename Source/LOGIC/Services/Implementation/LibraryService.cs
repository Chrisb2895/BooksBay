using DAL;
using DAL.DataContext;
using DAL.Entities;
using DAL.Functions.CRUD;
using DAL.Functions.Interfaces;
using DAL.Functions.Specific;
using LOGIC.Services.Interfaces;
using LOGIC.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOGIC.Services.Implementation
{
    public class LibraryService : ILibraryService
    {
        private ICRUD _CRUD;
        private ILibraryOperations _op ;
        private DatabaseContext _dbContext;

        public LibraryService(DatabaseContext dbContext)
        {
            _dbContext=dbContext;
            _CRUD = new CRUD(_dbContext);
            _op = new LibraryOperations(_dbContext);
        }

        public async Task<GenericResultSet<Library>> AddSingleLibrary(Library lib)
        {
            GenericResultSet<Library> result = new GenericResultSet<Library>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                Library library = new Library();
                // we are using crud class to add library because we only need to add 1 entity/record in 1 table
                //if things were more complex and we needed to add 2 entities which have a relationship then we should use LibraryOperations class instead
                library = await _CRUD.Create<Library>(lib);

                Library libAdded = new Library
                {
                    Id = library.Id,
                    Name = library.Name,
                    City = library.City,
                    BuiltDate = library.BuiltDate
                };

                result.UserMessage = $"The supplied library {libAdded.Name} was added successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.ResultSet = libAdded;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = "There was an error adding library, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }

        public async Task<GenericResultSet<Library>> DeleteLibrary(int libID)
        {
            GenericResultSet<Library> result = new GenericResultSet<Library>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                bool resultDelete = await _CRUD.Delete<Library>(libID);
                result.UserMessage = $"The supplied library {libID} was deleted successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.Success = resultDelete;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = $"There was an error deleting the {libID} library, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }

        public async Task<GenericResultSet<List<Library>>> GetAllLibraries()
        {
            GenericResultSet<List<Library>> result = new GenericResultSet<List<Library>>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                List<Library> libraries = await _CRUD.ReadAll<Library>();

                result.ResultSet = new List<Library>();
                libraries.ForEach(lib =>
                {
                    result.ResultSet.Add(new Library { Id = lib.Id, Name = lib.Name });
                });


                result.UserMessage = $"All libraries returned successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = "There was an error retrieving all libraries, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }

        public async Task<GenericResultSet<Library>> GetLibraryByID(int libID)
        {
            GenericResultSet<Library> result = new GenericResultSet<Library>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                Library lib = await _CRUD.Read<Library>(libID);
                result.ResultSet = new Library { Id = lib.Id, Name = lib.Name, City = lib.City, BuiltDate = lib.BuiltDate };
                result.UserMessage = $"The supplied library {lib.Name} was returned successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = $"There was an error retrieving the {libID} library, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }

        public async Task<GenericResultSet<List<Library>>> GetPagedLibraries(int page, int pageSize)
        {
            GenericResultSet<List<Library>> result = new GenericResultSet<List<Library>>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                List<Library> libraries = await _CRUD.ReadPaged<Library>(page, pageSize);

                result.ResultSet = new List<Library>();
                libraries.ForEach(lib =>
                {
                    result.ResultSet.Add(new Library { Id = lib.Id, Name = lib.Name });
                });


                result.UserMessage = $"All paged libraries returned successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = "There was an error retrieving all paged libraries, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }

        public async Task<GenericResultSet<Library>> UpdateLibrary(Library lib, int libID)
        {
            GenericResultSet<Library> result = new GenericResultSet<Library>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                Library library = new Library();

                library = await _CRUD.Update<Library>(lib, libID);

                Library libUpdated = new Library
                {
                    Id = library.Id,
                    Name = library.Name
                };

                result.UserMessage = $"The supplied library {libUpdated.Name} was updated successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.ResultSet = libUpdated;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = "There was an error updating library, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }


    }
}

using DAL;
using DAL.Entities;
using DAL.Functions.CRUD;
using LOGIC.Services.Interfaces;
using LOGIC.Services.Models;
using LOGIC.Services.Models.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOGIC.Services.Implementation
{
    public class LibraryService : ILibraryService
    {
        private ICRUD _CRUD = new CRUD();

        public async Task<GenericResultSet<LibraryResultSet>> AddSingleLibrary(Library lib)
        {
            GenericResultSet<LibraryResultSet> result = new GenericResultSet<LibraryResultSet>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                Library library = new Library();
                // we are using crud class to add library because we only need to add 1 entity/record in 1 table
                //if things were more complex and we needed to add 2 entities which have a relationship then we should use LibraryOperations class instead
                library = await _CRUD.Create<Library>(lib);

                LibraryResultSet libAdded = new LibraryResultSet
                {
                    Id = library.Id,
                    Name = library.Name
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

        public async Task<GenericResultSet<List<LibraryResultSet>>> GetAllLibraries()
        {
            GenericResultSet<List<LibraryResultSet>> result = new GenericResultSet<List<LibraryResultSet>>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                List<Library> libraries = await _CRUD.ReadAll<Library>();

                result.ResultSet = new List<LibraryResultSet>();
                libraries.ForEach(lib => { 
                                            result.ResultSet.Add(new LibraryResultSet { Id = lib.Id, Name = lib.Name }); 
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

        public async Task<GenericResultSet<LibraryResultSet>> GetLibraryByID(int libID)
        {
            GenericResultSet<LibraryResultSet> result = new GenericResultSet<LibraryResultSet>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                Library lib = await _CRUD.Read<Library>(libID);
                result.ResultSet = new LibraryResultSet { Id = lib.Id, Name = lib.Name };
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

        public async Task<GenericResultSet<LibraryResultSet>> UpdateLibrary(Library lib,int libID)
        {
            GenericResultSet<LibraryResultSet> result = new GenericResultSet<LibraryResultSet>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                Library library = new Library();

                library = await _CRUD.Update<Library>(lib,libID);

                LibraryResultSet libUpdated= new LibraryResultSet
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

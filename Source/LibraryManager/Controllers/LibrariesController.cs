using AutoMapper;
using LibraryManager.Data;
using LibraryManager.DTOS;
using LibraryManager.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManager.Helpers;
using Microsoft.AspNetCore.DataProtection;

namespace LibraryManager.Controllers


{
    // api/Libraries
    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : Controller
    {
        private readonly ILibraryRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<LibrariesController> _logger;
        public EphemeralDataProtectionProvider dataProtectionProvider;
        public IDataProtector _protector;

        public LibrariesController(ILibraryRepo repository, IMapper mapper, ILogger<LibrariesController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            dataProtectionProvider = new EphemeralDataProtectionProvider();
            _protector = dataProtectionProvider.CreateProtector("Crypto");
        }

        //GET api/libraries
        [HttpGet]
        public ActionResult<IEnumerable<LibraryReadDTO>> GetAllLibraries()
        {
            _logger.LogDebug("Libraries Controlller GetAllLibraries Method Start");
            var libraryItems = _repository.GetLibraries();

            if (libraryItems != null)
                return Ok(_mapper.Map<IEnumerable<LibraryReadDTO>>(libraryItems));

            return NotFound();
        }

        //GET api/libraries/librariesPaged
        [HttpGet]
        [Route("libraries/librariesPaged")]
        public ActionResult<IEnumerable<LibraryReadDTO>> GetAllLibrariesPaged([FromQuery] LibraryParameters libParams)
        {
            _logger.LogDebug("Libraries Controlller GetAllLibrariesPaged Method Start");
            var libraryItems = _repository.GetLibraries(libParams);

            if (libraryItems != null)
            {
                var metadata = new
                {
                    libraryItems.TotalCount,
                    libraryItems.PageSize,
                    libraryItems.CurrentPage,
                    libraryItems.HasNext,
                    libraryItems.HasPrevious,

                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInformation($"Returned {libraryItems.TotalCount} libraries from db");


                return Ok(_mapper.Map<IEnumerable<LibraryReadDTO>>(libraryItems));

            }

            return NotFound();
        }

        //GET api/libraries/{id}
        [HttpGet("{id}", Name = "GetLibraryByID")]
        public ActionResult<LibraryReadDTO> GetLibraryByID(int id)
        {
            var libraryItem = _repository.GetLibrariesById(id);
            if (libraryItem != null)
                return Ok(_mapper.Map<LibraryReadDTO>(libraryItem));

            return NotFound();
        }

        //POST api/libraries
        [HttpPost]
        public ActionResult<LibraryReadDTO> CreateLibrary(LibraryCreateDTO lib)
        {
            var librarymodel = _mapper.Map<Library>(lib);
            _repository.CreateLibrary(librarymodel);
            _repository.SaveChanges();

            var libraryReadDTO = _mapper.Map<LibraryReadDTO>(librarymodel);


            return CreatedAtRoute(nameof(GetLibraryByID), new { Id = libraryReadDTO.Id, libraryReadDTO });

        }

        //PUT api/libraries/{id}
        [HttpPut("{id}")]
        public ActionResult<LibraryReadDTO> UpdateLibrary(int id, LibraryUpdateDTO lib)
        {
            var librarymodel = _repository.GetLibrariesById(id);
            if (librarymodel == null)
                return NotFound();

            _mapper.Map(lib, librarymodel);

            _repository.UpdateLibrary(librarymodel);

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/libraries/{id}
        [HttpPatch("{id}")]
        public ActionResult<LibraryReadDTO> PartialLibraryUpdate(int id, JsonPatchDocument<LibraryUpdateDTO> patchDoc)
        {
            var librarymodel = _repository.GetLibrariesById(id);
            if (librarymodel == null)
                return NotFound();

            var libraryToPatch = _mapper.Map<LibraryUpdateDTO>(librarymodel);
            patchDoc.ApplyTo(libraryToPatch, ModelState);
            if (!TryValidateModel(libraryToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(libraryToPatch, librarymodel);

            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/libraries/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteLibrary(int id)
        {
            var librarymodel = _repository.GetLibrariesById(id);
            if (librarymodel == null)
                return NotFound();

            _repository.DeleteLibrary(librarymodel);

            _repository.SaveChanges();

            return NoContent();
        }

        //GET api/libraries/{input}
        [HttpGet("GetCrytedString/{input}")]
        public ActionResult<string> GetCrytedString(string input)
        {
            return Ok(GetCrypted(input));
        }

        [NonAction]
        public string GetCrypted(string fromS)
        {
            return _protector.Protect(fromS);
        }

        [NonAction]
        public string GetUnCrypted(string fromS)
        {
            return _protector.Protect(fromS);
        }

    }
}

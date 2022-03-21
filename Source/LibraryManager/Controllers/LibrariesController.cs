using AutoMapper;
using DAL.CustomProviders;
using DAL.DTOS;
using DAL.Entities;
using DAL.Helpers;
using LOGIC.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager.Controllers
{

    // api/Libraries
    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LibrariesController> _logger;
        private readonly CustomConfigProvider _configProvider;
        private ILibraryService _LibraryService;

        public LibrariesController(IMapper mapper, ILogger<LibrariesController> logger,
                                    CustomConfigProvider configuration, ILibraryService libraryService)
        {
            _mapper = mapper;
            _logger = logger;
            _configProvider = configuration;
            _LibraryService = libraryService;
        }

        //GET api/libraries
        [HttpGet]
        public async Task<IActionResult> GetAllLibraries()
        {
            var result = await _LibraryService.GetAllLibraries();
            if (result.Success)
            {
                var libraryReadDTO = _mapper.Map<IEnumerable<LibraryReadDTO>>(result.ResultSet);
                return Ok(libraryReadDTO);
            }
            else
                return StatusCode(500, result);

        }

        //GET api/libraries/librariesPaged
        [HttpGet]
        [Route("librariesPaged")]
        public async Task<IActionResult> GetAllLibrariesPaged([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await _LibraryService.GetPagedLibraries(page, pageSize);
            if (result.Success)
            {
                var metadata = new
                {
                    page,
                    pageSize
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                var libraryReadDTO = _mapper.Map<IEnumerable<LibraryReadDTO>>(result.ResultSet);
                return Ok(libraryReadDTO);
            }
            else
                return StatusCode(500, result);

        }

        //GET api/libraries/{id}
        [HttpGet("{id}", Name = "GetLibraryByID")]
        public async Task<IActionResult> GetLibraryByID(int id)
        {
            var result = await _LibraryService.GetLibraryByID(id);
            if (result.Success)
            {
                var libraryReadDTO = _mapper.Map<LibraryReadDTO>(result.ResultSet);
                return Ok(libraryReadDTO);
            }
            else
                return StatusCode(500, result);
        }

        //POST api/libraries
        [HttpPost]
        public async Task<IActionResult> CreateLibrary(LibraryCreateDTO lib)
        {
            var librarymodel = _mapper.Map<Library>(lib);
            var result = await _LibraryService.AddSingleLibrary(librarymodel);

            if (result.Success)
            {
                var libraryReadDTO = _mapper.Map<LibraryReadDTO>(result.ResultSet);
                return CreatedAtRoute(nameof(GetLibraryByID), new { Id = libraryReadDTO.Id, libraryReadDTO });
            }
            else
                return StatusCode(500, result);

        }

        //PUT api/libraries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLibrary(int id, LibraryUpdateDTO lib)
        {

            var result = await _LibraryService.GetLibraryByID(id);

            if (result.Success)
            {
                //TODOCODE: gestione del notFound
                var librarymodel = _mapper.Map<Library>(result.ResultSet);
                _mapper.Map(lib, librarymodel);
                var updateResult = await _LibraryService.UpdateLibrary(librarymodel, id);

                if (updateResult.Success)
                    return Ok(updateResult);
                else
                    return StatusCode(500, updateResult);
            }
            else
                return StatusCode(500, result);

        }

        //PATCH api/libraries/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialLibraryUpdate(int id, JsonPatchDocument<LibraryUpdateDTO> patchDoc)
        {
            var result = await _LibraryService.GetLibraryByID(id);

            if (result.Success)
            {
                //TODOCODE: gestione del notFound
                var librarymodel = _mapper.Map<Library>(result.ResultSet);
                var libraryToPatch = _mapper.Map<LibraryUpdateDTO>(librarymodel);
                patchDoc.ApplyTo(libraryToPatch, ModelState);
                if (!TryValidateModel(libraryToPatch))
                    return ValidationProblem(ModelState);

                _mapper.Map(libraryToPatch, librarymodel);
                var updateResult = await _LibraryService.UpdateLibrary(librarymodel, id);

                if (updateResult.Success)
                    return Ok(updateResult);
                else
                    return StatusCode(500, updateResult);
            }
            else
                return StatusCode(500, result);

        }

        //DELETE api/libraries/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibrary(int id)
        {
            var result = await _LibraryService.DeleteLibrary(id);

            if (result.Success)
            {
                //TODOCODE: gestione del notFound               
                return Ok(result);

            }
            else
                return StatusCode(500, result);
        }

        //GET api/libraries/{input}
        [HttpGet("GetCryptedString/{input}")]
        public ActionResult<string> GetCryptedString(string input)
        {
            return Ok(GetCrypted(input));
        }

        //GET api/libraries/{input}
        [HttpGet("GetUnCryptedString/{input}")]
        public ActionResult<string> GetUnCryptedString(string input)
        {
            return Ok(GetUnCrypted(input));
        }

        [NonAction]
        public string GetCrypted(string fromS)
        {
            return CryptoHelper.GetCrypted(Encoding.Default.GetString(Convert.FromBase64String(fromS)), _configProvider._configuration["MasterPWD"]);

        }

        [NonAction]
        public string GetUnCrypted(string fromS)
        {
            return CryptoHelper.GetUnCrypted(Encoding.Default.GetString(Convert.FromBase64String(fromS)), _configProvider._configuration["MasterPWD"]);
        }

    }
}

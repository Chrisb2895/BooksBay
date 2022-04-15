using AutoMapper;
using DAL.CoreAdminExtensions;
using DAL.DTOS;
using LOGIC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseGenericController : ControllerBase
    {
        private ILibraryService _LibraryService;
        private readonly IMapper _mapper;
        private readonly ILogger<DatabaseGenericController> _logger;
        private readonly IEnumerable<DiscoveredDbSetEntityType> dbSetEntities;
        public DatabaseGenericController(IEnumerable<DiscoveredDbSetEntityType> dbContexts, ILibraryService libraryService, 
                                            IMapper mapper, ILogger<DatabaseGenericController> logger)
        {
            this.dbSetEntities = dbContexts;
        }

        //GET api/DatabaseGeneric
        [HttpGet]
        public async Task<IActionResult> GetAllDbContextsAsync()
        {
            return Ok(await Task.FromResult(this.dbSetEntities));
            //TO CONTINUE, CALL THIS METHOD IN FRONT END
        }
    }
}

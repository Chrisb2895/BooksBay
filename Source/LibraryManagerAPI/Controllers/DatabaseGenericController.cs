using AutoMapper;
using DAL.CoreAdminExtensions;
using LOGIC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseGenericController : ControllerBase
    {
        private IDBGenericService _dbService;
        private readonly ILogger<DatabaseGenericController> _logger;
        private readonly IEnumerable<DiscoveredDbSetEntityType> dbSetEntities;
        public DatabaseGenericController(IEnumerable<DiscoveredDbSetEntityType> dbContexts, IDBGenericService dbService,
                                             ILogger<DatabaseGenericController> logger)
        {
            this.dbSetEntities = dbContexts;
            _dbService = dbService;
        }

        //GET api/DatabaseGeneric
        [HttpGet]
        public async Task<IActionResult> GetAllDbContextsAsync()
        {
            return Ok(await Task.FromResult(this.dbSetEntities));           
        }

        //GET api/DatabaseGeneric
        [HttpGet]
        [Route("GetDatabaseContext")]
        public IActionResult GetDatabaseContext()
        {
            return Ok(_dbService.GetDBContext());
        }

        //GET api/DatabaseGeneric/CoreAdminDataIndex
        [HttpPost]
        [Route("CoreAdminDataIndex")]
        public IActionResult CoreAdminDataIndex([FromBody] Type type)
        {
            return Ok(_dbService.Set(type));

        }
    }
}

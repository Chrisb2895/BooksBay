using AutoMapper;
using DAL.CoreAdminExtensions;
using LOGIC.Services.Interfaces;
using LOGIC.Services.Models;
using LOGIC.Services.Models.CoreAdminDataModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult CoreAdminDataIndex([FromBody] string type)
        {          
            Type _type = Type.GetType(type);
            var ret = JsonConvert.SerializeObject(_dbService.CoreAdminDataIndex(_type), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            });
            return Ok(ret);

        }
    }
}

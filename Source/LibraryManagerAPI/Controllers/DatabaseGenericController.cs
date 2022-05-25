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
        private readonly IMapper _mapper;
        private readonly ILogger<DatabaseGenericController> _logger;
        private readonly IEnumerable<DiscoveredDbSetEntityType> dbSetEntities;
        public DatabaseGenericController(IEnumerable<DiscoveredDbSetEntityType> dbContexts, IDBGenericService dbService,
                                            IMapper mapper, ILogger<DatabaseGenericController> logger)
        {
            this.dbSetEntities = dbContexts;
            _dbService = dbService;
        }

        //GET api/DatabaseGeneric
        [HttpGet]
        public async Task<IActionResult> GetAllDbContextsAsync()
        {
            return Ok(await Task.FromResult(this.dbSetEntities));
            //TO CONTINUE, CALL THIS METHOD IN FRONT END
        }

        //GET api/DatabaseGeneric/Set
        [HttpPost]
        [Route("Set")]
        public  IActionResult Set(Type type)
        {
            return Ok(_dbService.Set(type));

        }
    }
}

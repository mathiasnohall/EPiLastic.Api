using EpiLastic.Filters;
using EpiLastic.Querying;
using EpiLastic.Services;
using EPiLastic.Api.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace EPiLastic.Api.Search
{
    public class TypesController : ApiController
    {
        private readonly ISearchClient _searchClient;
        private readonly ISearchResponseMapper _mapper;
        private readonly ILoggerWrapper _logger;
        
        public TypesController(ISearchClient searchClient, ISearchResponseMapper mapper, ILoggerWrapper logger)
        {
            _searchClient = searchClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IHttpActionResult> GetTypes([FromUri] TypesQueryFilter filter)
        {
            var types = await _searchClient.GetTypesAsync(filter);

            if (types.OriginalException != null)
            {
                _logger.LogError<SubTypesController>(types.OriginalException.Message, types.OriginalException);
                return InternalServerError(types.OriginalException);
            }

            var response = _mapper.MapTypeResponse(types);

            return Ok(response);
        }
    }
}

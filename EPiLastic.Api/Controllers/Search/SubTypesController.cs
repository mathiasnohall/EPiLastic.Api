using EpiLastic.Filters;
using EpiLastic.Querying;
using EpiLastic.Services;
using EPiLastic.Api.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace EPiLastic.Api.Search
{
    public class SubTypesController : ApiController
    {
        private readonly ISearchClient _searchClient;
        private readonly ISearchResponseMapper _mapper;
        private readonly ILoggerWrapper _logger;
        
        public SubTypesController(ISearchClient searchClient, ISearchResponseMapper mapper, ILoggerWrapper logger)
        {
            _searchClient = searchClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IHttpActionResult> GetSubTypes([FromUri] SubTypesQueryFilter filter)
        {
            var subTypes = await _searchClient.GetSubTypesAsync(filter);

            if (subTypes.OriginalException != null)
            {
                _logger.LogError<SubTypesController>(subTypes.OriginalException.Message, subTypes.OriginalException);
                return InternalServerError(subTypes.OriginalException);
            }

            var response = _mapper.MapSubTypeResponse(subTypes);

            return Ok(response);
        }
    }
}

using EpiLastic.Filters;
using EpiLastic.Querying;
using EpiLastic.Services;
using EPiLastic.Api.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace EPiLastic.Api.Search
{
    public class PagesController : ApiController
    {
        private readonly ISearchClient _searchClient;
        private readonly ISearchResponseMapper _mapper;
        private readonly ILoggerWrapper _logger;

        public PagesController(ISearchClient searchClient, ISearchResponseMapper mapper, ILoggerWrapper logger)
        {
            _searchClient = searchClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IHttpActionResult> GetPages([FromUri] PagesQueryFilter filter)
        {
            var pages = await _searchClient.GetPagesAsync(filter);

            if (pages.OriginalException != null)
            {
                _logger.LogError<PagesController>(pages.OriginalException.Message, pages.OriginalException);
                return InternalServerError(pages.OriginalException);
            }

            var response = _mapper.Map(pages);

            return Ok(response);
        }
    }
}
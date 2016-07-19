using EpiLastic.Filters;
using EpiLastic.Querying;
using EpiLastic.Services;
using EPiLastic.Api.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace EPiLastic.Api.Search
{
    public class AutoCompleteController : ApiController
    {
        private readonly ISearchClient _searchClient;
        private readonly ISearchResponseMapper _searchResponseMapper;
        private readonly ILoggerWrapper _logger;

        public AutoCompleteController(ISearchClient searchClient, ISearchResponseMapper searchResponseMapper, ILoggerWrapper logger)
        {
            _searchClient = searchClient;
            _searchResponseMapper = searchResponseMapper;
            _logger = logger;
        }

        public async Task<IHttpActionResult> GetSuggestions([FromUri] AutoCompleteQueryFilter filter)
        {
            var suggestions = await _searchClient.GetAutoCompleteSuggestionsAsync(filter);

            if (suggestions.OriginalException != null)
            {
                _logger.LogError<AutoCompleteController>(suggestions.OriginalException.Message, suggestions.OriginalException);
                return InternalServerError(suggestions.OriginalException);
            }
            var autoCompleteResponse = _searchResponseMapper.Map(suggestions);

            return Ok(autoCompleteResponse);
        }
    }
}

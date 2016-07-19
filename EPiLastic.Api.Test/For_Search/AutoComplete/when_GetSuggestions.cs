using FakeItEasy;
using NUnit.Framework;
using EPiLastic.Api.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using EpiLastic.Querying;
using EpiLastic.Services;
using EpiLastic.Filters;
using System.Web.Http.Results;
using EPiLastic.Api.Services;
using EpiLastic.Models.Responses;

namespace EPiLastic.Api.Test.For_Search.AutoComplete
{
    [TestFixture]
    public class when_GetSuggestions
    {
        private AutoCompleteController _controller;
        private ISearchClient _searchClient;
        private ISearchResponseMapper _searchResponseMapper;

        public when_GetSuggestions()
        {
            _searchClient = A.Fake<ISearchClient>();
            var fakedResponse = A.Fake<Nest.ISuggestResponse>();
            
            A.CallTo(() => fakedResponse.OriginalException).Returns(null);
            A.CallTo(() => _searchClient.GetAutoCompleteSuggestionsAsync(A<AutoCompleteQueryFilter>.Ignored)).Returns(fakedResponse);
            _searchResponseMapper = A.Fake<ISearchResponseMapper>();

            _controller = new AutoCompleteController(_searchClient, _searchResponseMapper, A.Fake<ILoggerWrapper>());
        }

        [Test]
        public async Task AutoCompleteController_when_getSuggestions_it_should_return_suggestions()
        {
            var result = await _controller.GetSuggestions(new AutoCompleteQueryFilter()) as OkNegotiatedContentResult<List<AutoCompleteResponse>>;

            Assert.NotNull(result.Content);
        }
    }
}

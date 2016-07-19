using FakeItEasy;
using NUnit.Framework;
using EPiLastic.Api.Search;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using EpiLastic.Querying;
using EpiLastic.Services;
using EPiLastic.Api.Services;
using EpiLastic.Filters;

namespace EPiLastic.Api.Test.For_Search.AutoComplete
{
    [TestFixture]
    public class when_GetSuggestions_when_elastic_fails
    {
        private AutoCompleteController _controller;
        private ISearchClient _searchClient;
        private ISearchResponseMapper _searchResponseMapper;
        private ILoggerWrapper _logger;

        public when_GetSuggestions_when_elastic_fails()
        {
            _searchClient = A.Fake<ISearchClient>();
            var fakedResponse = A.Fake<Nest.ISuggestResponse>();
            var exception = new Exception("Elastic failed", new WebException("Elastic failed", WebExceptionStatus.ConnectFailure));
            A.CallTo(() => fakedResponse.OriginalException).Returns(exception);
            A.CallTo(() => _searchClient.GetAutoCompleteSuggestionsAsync(A<AutoCompleteQueryFilter>.Ignored)).Returns(fakedResponse);
            _searchResponseMapper = A.Fake<ISearchResponseMapper>();
            _logger = A.Fake<ILoggerWrapper>();
            _controller = new AutoCompleteController(_searchClient, _searchResponseMapper, _logger);
        }

        [Test]
        public async Task AutoCompleteController_when_GetSuggestions_when_elastic_fails_it_should_return_ExceptionResult()
        {
            var result = await _controller.GetSuggestions(new AutoCompleteQueryFilter()) as ExceptionResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task for_AutoCompleteController_when_GetSubTypes_when_elastic_fails_it_should_log()
        {
            var result = await _controller.GetSuggestions(new AutoCompleteQueryFilter()) as ExceptionResult;

            A.CallTo(() => _logger.LogError<AutoCompleteController>(A<string>.Ignored, A<Exception>.Ignored)).MustHaveHappened();
        }
    }
}

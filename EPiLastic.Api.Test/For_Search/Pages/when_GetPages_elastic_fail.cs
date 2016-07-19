using FakeItEasy;
using NUnit.Framework;
using EPiLastic.Api.Search;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using EpiLastic.Services;
using EpiLastic.Models;
using EPiLastic.Api.Services;
using EpiLastic.Filters;
using EpiLastic.Querying;

namespace EPiLastic.Api.Test.For_Search.Pages
{
    [TestFixture]
    public class when_GetPages_elastic_fail
    {
        private PagesController _controller;

        private ISearchClient _searchClient;
        private ISearchResponseMapper _responseMapper;
        private ILoggerWrapper _logger;

        public when_GetPages_elastic_fail()
        {
            _searchClient = A.Fake<ISearchClient>();

            var fakedResponse = A.Fake<Nest.ISearchResponse<Page>>();

            A.CallTo(() => _searchClient.GetPagesAsync(A<PagesQueryFilter>.Ignored)).Returns(fakedResponse);
            var exception = new Exception("Elastic failed", new WebException("Elastic failed", WebExceptionStatus.ConnectFailure));
            A.CallTo(() => fakedResponse.OriginalException).Returns(exception);
            _responseMapper = A.Fake<ISearchResponseMapper>();
            _logger = A.Fake<ILoggerWrapper>();

            _controller = new PagesController(_searchClient, _responseMapper, _logger);
        }

        [Test]
        public async Task when_pagescontroller_getPages_when_elastic_fails_it_should_return_badrequest()
        {
            var result = await _controller.GetPages(new PagesQueryFilter()) as ExceptionResult;
            
            Assert.NotNull(result);            
        }

        [Test]
        public async Task when_pagescontroller_getPages_when_elastic_fails_it_should_log()
        {
            var result = await _controller.GetPages(new PagesQueryFilter()) as ExceptionResult;

            A.CallTo(() => _logger.LogError<PagesController>(A<string>.Ignored, A<Exception>.Ignored)).MustHaveHappened();
        }
    }
}

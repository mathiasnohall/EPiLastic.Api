using FakeItEasy;
using NUnit.Framework;
using EPiLastic.Api.Search;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using EpiLastic.Querying;
using EPiLastic.Api.Services;
using EpiLastic.Services;
using EpiLastic.Models;
using EpiLastic.Filters;

namespace EPiLastic.Api.Test.For_Search.For_SubTypes
{
    [TestFixture]
    public class when_GetTypes_when_elastic_fails
    {
        private TypesController _controller;

        private ISearchClient _searchClient;
        private ISearchResponseMapper _responseMapper;
        private ILoggerWrapper _logger;

        public when_GetTypes_when_elastic_fails()
        {
            _searchClient = A.Fake<ISearchClient>();
            var fakedResponse = A.Fake<Nest.ISearchResponse<Page>>();
            var exception = new Exception("Elastic failed", new WebException("Elastic failed", WebExceptionStatus.ConnectFailure));
            A.CallTo(() => fakedResponse.OriginalException).Returns(exception);
            A.CallTo(() => _searchClient.GetTypesAsync(A<TypesQueryFilter>.Ignored)).Returns(fakedResponse);
            _responseMapper = A.Fake<ISearchResponseMapper>();
            _logger = A.Fake<ILoggerWrapper>();
            
            _controller = new TypesController(_searchClient, _responseMapper, _logger);
        }

        [Test]
        public async Task for_TypesController_when_GetTypes_when_elastic_fails_it_should_return_exceptionresult()
        {
            var result = await _controller.GetTypes(new TypesQueryFilter()) as ExceptionResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task for_TypesController_when_GetTypes_when_elastic_fails_it_should_log()
        {
            var result = await _controller.GetTypes(new TypesQueryFilter()) as ExceptionResult;

            A.CallTo(() => _logger.LogError<SubTypesController>(A<string>.Ignored, A<Exception>.Ignored)).MustHaveHappened();
        }
    }
}

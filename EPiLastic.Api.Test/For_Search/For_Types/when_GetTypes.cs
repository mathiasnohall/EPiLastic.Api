using FakeItEasy;
using NUnit.Framework;
using EPiLastic.Api.Search;
using System.Threading.Tasks;
using System.Web.Http.Results;
using EpiLastic.Querying;
using EpiLastic.Services;
using EpiLastic.Filters;
using EpiLastic.Models;
using EPiLastic.Api.Services;
using EpiLastic.Models.Responses;

namespace EPiLastic.Api.Test.For_Search.For_SubTypes
{
    [TestFixture]
    public class when_GetTypes
    {
        private TypesController _controller;

        private ISearchClient _searchClient;
        private ISearchResponseMapper _responseMapper;

        public when_GetTypes()
        {
            _searchClient = A.Fake<ISearchClient>();
            var fakedResponse = A.Fake<Nest.ISearchResponse<Page>>();
            A.CallTo(() => fakedResponse.OriginalException).Returns(null);
            A.CallTo(() => _searchClient.GetTypesAsync(A<TypesQueryFilter>.Ignored)).Returns(fakedResponse);
            _responseMapper = A.Fake<ISearchResponseMapper>();

            _controller = new TypesController(_searchClient, _responseMapper, A.Fake<ILoggerWrapper>());
        }

        [Test]
        public async Task for_TypesController_when_getTypes_it_should_return_aggregationresultcontainer()
        {
            var result = await _controller.GetTypes(new TypesQueryFilter()) as OkNegotiatedContentResult<AggregationResultContainer>;

            Assert.NotNull(result.Content);
        }
    }
}

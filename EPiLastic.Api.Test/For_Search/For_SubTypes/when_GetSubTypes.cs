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
    public class when_GetSubTypes
    {
        private SubTypesController _controller;

        private ISearchClient _searchClient;
        private ISearchResponseMapper _responseMapper;

        public when_GetSubTypes()
        {
            _searchClient = A.Fake<ISearchClient>();
            var fakedResponse = A.Fake<Nest.ISearchResponse<Page>>();
            A.CallTo(() => fakedResponse.OriginalException).Returns(null);
            A.CallTo(() => _searchClient.GetSubTypesAsync(A<SubTypesQueryFilter>.Ignored)).Returns(fakedResponse);
            _responseMapper = A.Fake<ISearchResponseMapper>();

            _controller = new SubTypesController(_searchClient, _responseMapper, A.Fake<ILoggerWrapper>());
        }

        [Test]
        public async Task for_SubTypesController_when_getSubTypes_it_should_return_aggregationresultcontainer()
        {
            var result = await _controller.GetSubTypes(new SubTypesQueryFilter()) as OkNegotiatedContentResult<AggregationResultContainer>;

            Assert.NotNull(result.Content);
        }
    }
}

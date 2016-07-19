using FakeItEasy;
using NUnit.Framework;
using EPiLastic.Api.Search;
using System.Threading.Tasks;
using System.Web.Http.Results;
using EpiLastic.Querying;
using EpiLastic.Services;
using EpiLastic.Models;
using EPiLastic.Api.Services;
using EpiLastic.Filters;
using EpiLastic.Models.Responses;

namespace EPiLastic.Api.Test.For_Search.Pages
{
    [TestFixture]
    public class when_GetPages
    {
        private PagesController _controller;

        private ISearchClient _searchClient;
        private ISearchResponseMapper _responseMapper;

        public when_GetPages()
        {
            _searchClient = A.Fake<ISearchClient>();
            var fakedResponse = A.Fake<Nest.ISearchResponse<Page>>();
            A.CallTo(() => fakedResponse.OriginalException).Returns(null);
            A.CallTo(() => _searchClient.GetPagesAsync(A<PagesQueryFilter>.Ignored)).Returns(fakedResponse);
            _responseMapper = A.Fake<ISearchResponseMapper>();

            _controller = new PagesController(_searchClient, _responseMapper, A.Fake<ILoggerWrapper>());
        }

        [Test]
        public async Task when_pagescontroller_getPages_it_should_return_pages()
        {
            var result = await _controller.GetPages(new PagesQueryFilter()) as OkNegotiatedContentResult<PagesSearchResponse>;

            Assert.NotNull(result.Content);
        }
    }
}

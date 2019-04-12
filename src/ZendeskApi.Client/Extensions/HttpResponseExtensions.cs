using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZendeskApi.Client.Exceptions;

namespace ZendeskApi.Client.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task ThrowIfUnsuccessful(
            this HttpResponseMessage response, 
            string helpDocLink,
            string helpDocLinkPrefix = "/support")
        {
            if (!response.IsSuccessStatusCode)
            {
                await response.ThrowZendeskRequestException(helpDocLink);
            }
        }

        public static async Task ThrowZendeskRequestException(
            this HttpResponseMessage response, 
            string helpDocLink,
            HttpStatusCode? expected = null,
            string helpDocLinkPrefix = "/support"
        )
        {
            var builder = new ZendeskRequestExceptionBuilder()
                .WithResponse(response)
                .WithHelpDocsLink($"{helpDocLinkPrefix}/{helpDocLink}");

            if (expected.HasValue)
                builder.WithExpectedHttpStatus(expected.Value);
                
            throw await builder.Build();
        }
    }
}

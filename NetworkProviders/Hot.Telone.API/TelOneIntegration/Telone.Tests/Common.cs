using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telone.Tests
{
    public static class Common
    {

        public static IHttpClientFactory GetFakeFactory(string Content)
        {
            var mockFactory = new Mock<IHttpClientFactory>(); 
            var client = GetFakeHttpClient(Content);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(client);
            return mockFactory.Object;
        } 

        private static HttpClient GetFakeHttpClient(string Content)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Content),
                });
            var client = new HttpClient(mockHttpMessageHandler.Object);
            client.BaseAddress = new Uri("http://127.0.0.1/");
            return client; 
        }
    }
}

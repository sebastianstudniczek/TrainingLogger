using System.Net;

namespace TrainingLogger.Shared.Tests;

public class MockHttpClientBuilder
{
    private HttpStatusCode _responseCode = HttpStatusCode.OK;
    private HttpContent? _responseContent;
    private Uri _baseUri = new("https://training-logger.io");

    public MockHttpClientBuilder WithResponseCode(HttpStatusCode responseCode)
    {
        _responseCode = responseCode;
        return this;
    }

    public MockHttpClientBuilder WithReponseContent(HttpContent content)
    {
        _responseContent = content;
        return this;
    }

    public MockHttpClientBuilder WithBaseUri(Uri baseUri)
    {
        _baseUri = baseUri;
        return this;
    }

    public MockHttpClientBuilder WithBaseUri(string baseUri)
    {
        _baseUri = new Uri(baseUri);
        return this;
    }

    public MockHttpClient Build()
    {
        var messageHandler = new MockHttpMessageHandler(_responseCode, _responseContent);
        var httpClient = new HttpClient(messageHandler)
        {
            BaseAddress = _baseUri
        };

        return new MockHttpClient
        {
            MessageHandler = messageHandler,
            Client = httpClient
        };
    }
}

public class MockHttpClient
{
    public required HttpClient Client { get; set; }
    public required MockHttpMessageHandler MessageHandler { get; set; }
}

public class MockHttpMessageHandler(
    HttpStatusCode responseCode = HttpStatusCode.OK,
    HttpContent? responseContent = null
    ) : HttpMessageHandler
{
    private readonly HttpStatusCode _responseCode = responseCode;
    private readonly HttpContent? _responsContent = responseContent;
    public HttpRequestMessage? InvokedWithRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        InvokedWithRequest = request;
        return _responsContent is null
            ? Task.FromResult(new HttpResponseMessage(_responseCode))
            : Task.FromResult(new HttpResponseMessage(_responseCode)
            {
                Content = _responsContent
            });
    }
}

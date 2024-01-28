using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.UnitTests.Strava;

public class TokenStoreTests : IDisposable
{
    private readonly TokenStore _store;
    private readonly SqliteConnection _sqliteConnection;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly TimeProvider _timeProvider = Substitute.For<TimeProvider>();
    private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
    private readonly StravaOptionsFaker _optionsFaker = new();
    private readonly ApplicationDbContext _dbContext;
    private readonly TrainingLoggerFixture _fixture = new();

    public TokenStoreTests()
    {
        _sqliteConnection = new(Utils.SqliteInMemoryConnectionString);
        _sqliteConnection.Open();
        _dbContext = Utils.CreateInMemoryContext(_sqliteConnection);
        var stravaOptions = Options.Create(_optionsFaker.Generate());
        var sampleCredentials = Options.Create(_fixture.Create<ClientCredentials>());

        _store = new TokenStore(
            _dbContext,
            _memoryCache,
            _timeProvider,
            _httpClientFactory,
            stravaOptions,
            sampleCredentials);
    }

    [Fact]
    public async Task Should_Fail_When_There_Is_No_Token_Saved_In_Database()
    {
        var act = async () => await _store.GetTokenAsync(default);

        await act.Should().ThrowAsync<StravaAuthTokenNotFound>();
    }

    [Fact]
    public async Task Should_Not_Fail_If_There_Is_A_Token_Saved_In_Database()
    {
        var savedToken = _fixture.Create<ApiAccessToken>();
        _dbContext.RefreshTokens.Add(savedToken);
        _dbContext.SaveChanges();

        var act = async () => await _store.GetTokenAsync(default);

        await act.Should().NotThrowAsync<StravaAuthTokenNotFound>();
    }

    [Fact]
    public async Task Should_Return_Saved_Access_Token_If_Is_Not_Expired()
    {
        var now = DateTimeOffset.UtcNow;
        _timeProvider.GetUtcNow().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var savedToken = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt + 1)
            .Create();
        _dbContext.RefreshTokens.Add(savedToken);
        _dbContext.SaveChanges();

        string actualToken = await _store.GetTokenAsync(default);

        actualToken.Should().BeEquivalentTo(savedToken.AccessToken);
    }

    [Fact]
    public async Task Should_Refresh_Token_If_Saved_One_Is_Expired_With_Refresh_Token_From_Db()
    {
        var now = DateTimeOffset.UtcNow;
        _timeProvider.GetUtcNow().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var tokenToCache = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt)
            .Create();
        _dbContext.RefreshTokens.Add(tokenToCache);
        _dbContext.SaveChanges();
        var httpClient = new MockHttpClientBuilder()
            .WithReponseContent(JsonContent.Create(tokenToCache))
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(httpClient.Client);

        _ = await _store.GetTokenAsync(default);

        var actualRequestContent = httpClient.MessageHandler.InvokedWithRequest?.Content;
        actualRequestContent.Should().NotBeNull();
        var tokenRequest = (actualRequestContent as JsonContent)?.Value as TokenExchangeRequest;
        tokenRequest?.RefreshToken.Should().Be(tokenToCache.RefreshToken);
    }

    [Fact(Skip = "In memory provider does not support ExecuteUpdate method")]
    public async Task Should_Replace_Saved_Token_With_Refreshed_Token_If_Saved_One_Is_Expired()
    {
        var now = DateTimeOffset.UtcNow;
        _timeProvider.GetUtcNow().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var tokenToCache = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt)
            .Create();
        _dbContext.RefreshTokens.Add(tokenToCache);
        _dbContext.SaveChanges();
        var refreshedToken = _fixture.Create<ApiAccessToken>();
        var httpClient = new MockHttpClientBuilder()
            .WithReponseContent(JsonContent.Create(refreshedToken))
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(httpClient.Client);

        _ = await _store.GetTokenAsync(default);

        var replacedToken = _dbContext
            .RefreshTokens
            .Single();
        replacedToken.Should().NotBeNull();
        replacedToken.Should().BeEquivalentTo(refreshedToken);
    }

    [Fact]
    public async Task Should_Return_Refreshed_Token_If_Saved_One_Is_Expired()
    {
        var now = DateTimeOffset.UtcNow;
        _timeProvider.GetUtcNow().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var tokenToCache = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt)
            .Create();
        _dbContext.RefreshTokens.Add(tokenToCache);
        _dbContext.SaveChanges();
        var refreshedToken = _fixture.Create<ApiAccessToken>();
        var httpClient = new MockHttpClientBuilder()
            .WithReponseContent(JsonContent.Create(refreshedToken))
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(httpClient.Client);

        string actualToken = await _store.GetTokenAsync(default);

        actualToken.Should().BeEquivalentTo(refreshedToken.AccessToken);
    }

    public void Dispose() => _sqliteConnection.Dispose();
}

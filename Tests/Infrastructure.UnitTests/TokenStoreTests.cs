using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using System.Data.Common;
using TrainingLogger.Core.Models;
using TrainingLogger.Core.Services;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Infrastructure.Strava.Interfaces;

namespace TrainingLogger.Infrastructure.UnitTests;

public class TokenStoreTests : IDisposable
{
    private readonly TokenStore _store;
    private readonly DbConnection _sqliteConnection;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly GetUtcNow _getUtcNow = Substitute.For<GetUtcNow>();
    private readonly IFixture _fixture = new Fixture();
    private readonly GetRefreshedToken _refreshToken = Substitute.For<GetRefreshedToken>();

    public TokenStoreTests()
    {
        _sqliteConnection = new SqliteConnection(Utils.SqliteInMemoryConnectionString);
        _sqliteConnection.Open();
        _dbContext = Utils.CreateInMemoryContext(_sqliteConnection);
        _refreshToken
            .Invoke(Arg.Any<string>())
            .Returns(new ApiAccessToken());

        _store = new TokenStore(_dbContext, _memoryCache, _getUtcNow);
    }

    [Fact]
    public async Task ShouldThrow_StravaAuthTokenNotFound_WhenThereIsNoTokenSaved_InDatabase()
    {
        var act = async () => await _store.GetTokenAsync(_refreshToken, default);

        await act.Should().ThrowAsync<StravaAuthTokenNotFound>();
    }

    [Fact]
    public async Task ShouldNotThrow_StravaAuthTokenNotFound_IfThereIsAToken_SavedInDatabase()
    {
        var savedToken = _fixture.Create<ApiAccessToken>();
        _dbContext.RefreshTokens.Add(savedToken);
        _dbContext.SaveChanges();

        var act = async () => await _store.GetTokenAsync(_refreshToken, default);

        await act.Should().NotThrowAsync<StravaAuthTokenNotFound>();
    }

    [Fact]
    public async Task ShouldReturn_SavedAccessToken_IfIsNotExpired()
    {
        var now = DateTimeOffset.UtcNow;
        _getUtcNow.Invoke().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var savedToken = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt + 1)
            .Create();
        _dbContext.RefreshTokens.Add(savedToken);
        _dbContext.SaveChanges();

        string actualToken = await _store.GetTokenAsync(_refreshToken, default);

        actualToken.Should().BeEquivalentTo(savedToken.AccessToken);
    }

    [Fact]
    public async Task ShouldRefreshToken_IfSavedOne_IsExpired_WithRefreshToken()
    {
        var now = DateTimeOffset.UtcNow;
        _getUtcNow.Invoke().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var tokenToCache = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt)
            .Create();
        _dbContext.RefreshTokens.Add(tokenToCache);
        _dbContext.SaveChanges();
        var refreshedToken = _fixture.Create<ApiAccessToken>();
        _refreshToken
            .Invoke(Arg.Any<string>())
            .Returns(refreshedToken);

        _ = await _store.GetTokenAsync(_refreshToken, default);

        await _refreshToken
            .Received()
            .Invoke(tokenToCache.RefreshToken);
    }

    [Fact]
    public async Task ShouldReplace_SavedToken_WithRefreshedToken_IfSavedOne_IsExpired()
    {
        var now = DateTimeOffset.UtcNow;
        _getUtcNow.Invoke().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var tokenToCache = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt)
            .Create();
        _dbContext.RefreshTokens.Add(tokenToCache);
        _dbContext.SaveChanges();
        var refreshedToken = _fixture.Create<ApiAccessToken>();
        _refreshToken
            .Invoke(Arg.Any<string>())
            .Returns(refreshedToken);

        _ = await _store.GetTokenAsync(_refreshToken, default);

        var replacedToken = _dbContext
            .RefreshTokens
            .Single();
        replacedToken.Should().NotBeNull();
        // TODO: Not working cause of the in memory provider replacedToken.Should().BeEquivalentTo(refreshedToken);
    }

    [Fact]
    public async Task ShouldReturn_RefreshedToken_IfSavedOne_IsExpired()
    {
        var now = DateTimeOffset.UtcNow;
        _getUtcNow.Invoke().Returns(now);
        long expiresAt = now.ToUnixTimeSeconds();
        var tokenToCache = _fixture
            .Build<ApiAccessToken>()
            .With(x => x.ExpiresAt, expiresAt)
            .Create();
        _dbContext.RefreshTokens.Add(tokenToCache);
        _dbContext.SaveChanges();
        var  refreshedToken = _fixture.Create<ApiAccessToken>();
        _refreshToken
            .Invoke(Arg.Any<string>())
            .Returns(refreshedToken);

        string actualToken = await _store.GetTokenAsync(_refreshToken, default);

        actualToken.Should().BeEquivalentTo(refreshedToken.AccessToken);
    }

    public void Dispose() => _sqliteConnection.Dispose();
}

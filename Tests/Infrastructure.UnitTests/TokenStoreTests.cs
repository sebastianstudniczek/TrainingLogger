using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using System.Data.Common;
using TrainingLogger.Core.Models;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Implementations;

namespace TrainingLogger.Infrastructure.UnitTests;

public class TokenStoreTests : IDisposable
{
    private readonly TokenStore _store;
    private readonly DbConnection _sqliteConnection;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();

    public TokenStoreTests()
    {
        _sqliteConnection = new SqliteConnection("Filename=:memory:");
        _sqliteConnection.Open();
        _dbContext = Utils.CreateInMemoryContext(_sqliteConnection);

        _store = new TokenStore(_dbContext, _memoryCache, () => DateTime.UtcNow);
    }

    [Fact]
    public async Task ShouldThrow_StravaAuthTokenNotFound_WhenThereIsNoTokenSaved_InDatabase_AndThereIsNoCachedToken()
    {
        var fetchToken = (string _) => Task.FromResult(new ApiAccessToken());

        var act = async () => await _store.GetTokenAsync(fetchToken, default);

        await act.Should().ThrowAsync<StravaAuthTokenNotFound>();
    }

    public void Dispose() => _sqliteConnection.Dispose();
}

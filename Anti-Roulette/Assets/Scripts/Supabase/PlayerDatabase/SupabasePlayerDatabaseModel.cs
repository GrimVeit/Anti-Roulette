using UnityEngine;
using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Supabase;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Models;
using System.Collections.Generic;
using Supabase.Postgrest.Attributes;

public sealed class SupabasePlayerDatabaseModel : IDisposable
{
    private const int RequestTimeoutSeconds = 7;

    private readonly Client _client;
    private readonly IAuthenticationInfoProvider _authenticationInfo;
    private readonly CancellationTokenSource _disposeCts = new();
    private bool _disposed;

    public SupabasePlayerDatabaseModel(
        Client client,
        IAuthenticationInfoProvider authenticationInfo)
    {
        _client = client;
        _authenticationInfo = authenticationInfo;
    }

    public async UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data)
    {
        if (!_authenticationInfo.IsAuthorized)
            return DatabaseResult.NotAuthorized;

        if (string.IsNullOrWhiteSpace(data.Nickname))
            return DatabaseResult.InvalidData;

        try
        {
            var row = new PlayerSupabaseProfile
            {
                UserId = _authenticationInfo.UserId,
                Nickname = data.Nickname,
                Money = data.Money
            };

            await _client
                .From<PlayerSupabaseProfile>()
                .Upsert(row)
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            return DatabaseResult.Success;
        }
        catch (OperationCanceledException)
        {
            return DatabaseResult.Cancelled;
        }
        catch (TimeoutException)
        {
            return DatabaseResult.Timeout;
        }
        catch (PostgrestException exception)
        {
            Debug.LogException(exception);
            return DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return DatabaseResult.UnknownError;
        }
    }

    public async UniTask<(DatabaseResult Result, PlayerData Data)> LoadPlayer()
    {
        if (!_authenticationInfo.IsAuthorized)
            return (DatabaseResult.NotAuthorized, null);

        try
        {
            var response = await _client
                .From<PlayerSupabaseProfile>()
                .Where(p => p.UserId == _authenticationInfo.UserId)
                .Get()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            var profile = response.Models.FirstOrDefault();

            if (profile == null)
                return (DatabaseResult.NotFound, null);

            return (DatabaseResult.Success, new PlayerData(profile.Nickname, profile.Money));
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            return (DatabaseResult.Timeout, null);
        }
        catch (PostgrestException exception)
        {
            Debug.LogException(exception);
            return (DatabaseResult.UnknownError, null);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return (DatabaseResult.UnknownError, null);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _disposeCts.Cancel();
        _disposeCts.Dispose();
    }
}

public sealed class PlayerData
{
    public string Nickname { get; }
    public int Money { get; }

    public PlayerData(string nickname, int money)
    {
        Nickname = nickname;
        Money = money;
    }
}

public enum DatabaseResult
{
    Success,
    Cancelled,
    Timeout,
    NotAuthorized,
    NotFound,
    InvalidData,
    UnknownError
}

[Table("players")]
public class PlayerSupabaseProfile : BaseModel
{
    [PrimaryKey("user_id", false)]
    public string UserId { get; set; }

    [Column("nickname")]
    public string Nickname { get; set; }

    [Column("money")]
    public int Money { get; set; }
}

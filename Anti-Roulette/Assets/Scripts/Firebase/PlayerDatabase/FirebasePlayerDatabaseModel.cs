using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public sealed class FirebasePlayerDatabaseModel : IDisposable
{
    private const string UsersNode = "Users";
    private const int RequestTimeoutSeconds = 7;

    private readonly FirebaseDatabase _database;
    private readonly IAuthenticationInfoProvider _authenticationInfo;
    private readonly CancellationTokenSource _disposeCts = new();
    private bool _disposed;

    public FirebasePlayerDatabaseModel(
        FirebaseDatabase database,
        IAuthenticationInfoProvider authenticationInfo)
    {
        _database = database;
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
            DatabaseReference reference = GetPlayerReference();

            Dictionary<string, object> values = CreateValues(data);

            await reference
                .UpdateChildrenAsync(values)
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
        catch (DatabaseException exception)
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
            DatabaseReference reference = GetPlayerReference();

            DataSnapshot snapshot = await reference
                .GetValueAsync()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            if (!snapshot.Exists)
                return (DatabaseResult.NotFound, null);

            PlayerData data = ReadPlayerData(snapshot);
            return (DatabaseResult.Success, data);
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            return (DatabaseResult.Timeout, null);
        }
        catch (DatabaseException exception)
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

    private DatabaseReference GetPlayerReference()
    {
        return _database.RootReference
            .Child(UsersNode)
            .Child(_authenticationInfo.UserId);
    }

    private static Dictionary<string, object> CreateValues(PlayerData data)
    {
        return new Dictionary<string, object>
        {
            ["Nickname"] = data.Nickname,
            ["Money"] = data.Money
        };
    }

    private static PlayerData ReadPlayerData(DataSnapshot snapshot)
    {
        string nickname = snapshot.Child("Nickname").Value?.ToString() ?? string.Empty;
        int level = ReadInt(snapshot.Child("Money").Value);

        return new PlayerData(nickname, level);
    }

    internal static int ReadInt(object value, int fallback = 0)
    {
        return value switch
        {
            null => fallback,
            int i => i,
            long l => (int)Math.Clamp(l, int.MinValue, int.MaxValue),
            string s when int.TryParse(s, out int parsed) => parsed,
            _ => fallback
        };
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

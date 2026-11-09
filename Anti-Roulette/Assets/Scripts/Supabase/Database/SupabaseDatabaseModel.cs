using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Supabase.Postgrest;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

public sealed class SupabaseDatabaseModel : IDisposable
{
    private const int RequestTimeoutSeconds = 9;

    private readonly Supabase.Client _client;
    private readonly CancellationTokenSource _disposeCts = new();
    private bool _disposed;

    public SupabaseDatabaseModel(Supabase.Client client)
    {
        _client = client;
    }

    #region Players

    public async UniTask<(DatabaseResult Result, List<PlayerData> Players)> GetTopPlayers(int count)
    {
        OnGetTopPlayersStarted?.Invoke();

        if (count <= 0)
        {
            OnGetTopPlayers?.Invoke(DatabaseResult.InvalidData, null);
            return (DatabaseResult.InvalidData, null);
        }

        DatabaseResult result;
        List<PlayerData> players = null;

        try
        {
            var response = await _client
                .From<PlayerSupabaseProfile>()
                .Order("money", Constants.Ordering.Descending)
                .Limit(count)
                .Get()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            players = response.Models
                .Select(p => new PlayerData(p.Nickname, p.Money))
                .ToList();

            result = DatabaseResult.Success;
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = DatabaseResult.Timeout;
        }
        catch (PostgrestException exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }

        OnGetTopPlayers?.Invoke(result, players);
        return (result, players);
    }

    public async UniTask<(DatabaseResult Result, PlayerData Player)> GetPlayerByPlace(int place)
    {
        OnGetPlayerByPlaceStarted?.Invoke();

        if (place <= 0)
        {
            OnGetPlayerByPlace?.Invoke(DatabaseResult.InvalidData, null);
            return (DatabaseResult.InvalidData, null);
        }

        DatabaseResult result;
        PlayerData player = null;

        try
        {
            var response = await _client
                .From<PlayerSupabaseProfile>()
                .Order("money", Constants.Ordering.Descending)
                .Range(place - 1, place - 1)
                .Get()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            var profile = response.Models.FirstOrDefault();

            if (profile == null)
            {
                result = DatabaseResult.NotFound;
            }
            else
            {
                player = new PlayerData(profile.Nickname, profile.Money);
                result = DatabaseResult.Success;
            }
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = DatabaseResult.Timeout;
        }
        catch (PostgrestException exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }

        OnGetPlayerByPlace?.Invoke(result, player);
        return (result, player);
    }

    #endregion

    #region Others

    public async UniTask<(DatabaseResult Result, LinkGeoData Data)> GetLinkGeoData()
    {
        OnGetLinkDataStarted?.Invoke();

        DatabaseResult result;
        LinkGeoData data = null;

        try
        {
            var response = await _client
                .From<LinkGeoRow>()
                .Limit(1)
                .Get()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            var row = response.Models.FirstOrDefault();

            if (row == null)
            {
                result = DatabaseResult.NotFound;
            }
            else
            {
                string link = string.IsNullOrWhiteSpace(row.Link) ? null : row.Link;

                List<string> geo = row.Geo?
                    .Where(g => !string.IsNullOrWhiteSpace(g))
                    .ToList() ?? new List<string>();

                data = new LinkGeoData(link, geo);
                result = DatabaseResult.Success;
            }
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = DatabaseResult.Timeout;
        }
        catch (PostgrestException exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }

        OnGetLinkData?.Invoke(result, data);
        return (result, data);
    }

    #endregion

    public event Action OnGetTopPlayersStarted;
    public event Action<DatabaseResult, List<PlayerData>> OnGetTopPlayers;

    public event Action OnGetPlayerByPlaceStarted;
    public event Action<DatabaseResult, PlayerData> OnGetPlayerByPlace;

    public event Action OnGetLinkDataStarted;
    public event Action<DatabaseResult, LinkGeoData> OnGetLinkData;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _disposeCts.Cancel();
        _disposeCts.Dispose();
    }
}

public sealed class LinkGeoData
{
    public string Link { get; }
    public List<string> Geo { get; }

    public LinkGeoData(string link, List<string> geo)
    {
        Link = link;
        Geo = geo;
    }
}

[Table("link_geo")]
public class LinkGeoRow : BaseModel
{
    [PrimaryKey("id", true)]
    public int Id { get; set; }

    [Column("link")]
    public string Link { get; set; }

    [Column("geo")]
    public List<string> Geo { get; set; }
}

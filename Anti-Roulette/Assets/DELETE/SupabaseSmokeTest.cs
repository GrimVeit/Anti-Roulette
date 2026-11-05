using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using UnityEngine;

//https://xrnztbsbvqrurvxtvcqi.supabase.co
//sb_publishable_WzMp-gGcp9TEyXX4cipYrQ_3k0vyQqh

using System.Linq;

public class SupabaseSmokeTest : MonoBehaviour
{
    private const string Url = "https://xrnztbsbvqrurvxtvcqi.supabase.co";
    private const string Key = "sb_publishable_WzMp-gGcp9TEyXX4cipYrQ_3k0vyQqh";

    private Client _client;

    private async void Start()
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            SessionHandler = new FileSessionHandler()
        };

        _client = new Client(Url, Key, options);
        await _client.InitializeAsync();

        // Если сессия загружена из файла — этот вызов ничего не создаст
        if (_client.Auth.CurrentSession == null)
        {
            await _client.Auth.SignInAnonymously();
            Debug.Log("[Auth] new anonymous user created");
        }
        else
        {
            Debug.Log("[Auth] session restored from file");
        }

        string userId = _client.Auth.CurrentUser?.Id;
        Debug.Log($"[Auth] UserId: {userId}");

        // Upsert профиля
        var row = new Profile
        {
            UserId = userId,
            Nickname = "TestPlayer",
            Money = 100
        };
        await _client.From<Profile>().Upsert(row);
        Debug.Log("[DB] Upsert OK");

        // Чтение своего профиля
        var mine = await _client.From<Profile>()
            .Where(p => p.UserId == userId)
            .Get();
        var me = mine.Models.FirstOrDefault();
        Debug.Log($"[DB] Loaded: {me?.Nickname}, money={me?.Money}");

        // Лидерборд (топ-5 по money)
        var top = await _client.From<Profile>()
            .Order("money", Supabase.Postgrest.Constants.Ordering.Descending)
            .Limit(5)
            .Get();
        Debug.Log($"[DB] Top players: {top.Models.Count}");
        foreach (var p in top.Models)
            Debug.Log($"  {p.Nickname} — {p.Money}");
    }
}

[Table("players")]
public class Profile : BaseModel
{
    [PrimaryKey("user_id", false)] public string UserId { get; set; }
    [Column("nickname")] public string Nickname { get; set; }
    [Column("money")] public int Money { get; set; }
}

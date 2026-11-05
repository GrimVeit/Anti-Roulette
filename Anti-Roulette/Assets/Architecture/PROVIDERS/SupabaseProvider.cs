using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Supabase;

public interface ISupabaseProvider : IAppService
{
    Client Client { get; }
}

public sealed class SupabaseProvider : ISupabaseProvider, IAsyncDisposable
{
    private readonly SupabaseOptions _options;
    private readonly string _url;
    private readonly string _key;

    public Client Client { get; private set; }

    public SupabaseProvider(string url, string key)
    {
        _url = url;
        _key = key;
        _options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            SessionHandler = new FileSessionHandler()
        };
    }

    public async UniTask InitializeAsync()
    {
        if (Client != null) return;

        Client = new Client(_url, _key, _options);
        await Client.InitializeAsync();
    }

    public ValueTask DisposeAsync()
    {
        // если у Client есть Dispose Ч вызвать
        Client = null;
        return default;
    }
}

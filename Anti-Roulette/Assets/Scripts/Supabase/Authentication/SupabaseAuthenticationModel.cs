using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Supabase;
using Supabase.Gotrue.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaCon;

public sealed class SupabaseAuthenticationModel : IDisposable
{
    private const int RequestTimeoutSeconds = 10;

    private readonly Client _client;
    private readonly CancellationTokenSource _disposeCts = new();
    private bool _disposed;

    public SupabaseAuthenticationModel(Client client)
    {
        _client = client;
    }

    public bool IsAuthorized => _client.Auth.CurrentSession != null;
    public string UserId => _client.Auth.CurrentUser?.Id;

    public event Action<AuthenticationResult> OnAuthenticationResult;

    public async UniTask<AuthenticationResult> EnsureSignedInAsync()
    {
        if (IsAuthorized)
            return AuthenticationResult.Success;

        AuthenticationResult result;

        try
        {
            await _client.Auth
                .SignInAnonymously()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            result = AuthenticationResult.Success;
        }
        catch (OperationCanceledException)
        {
            return AuthenticationResult.Cancelled;
        }
        catch (TimeoutException)
        {
            result = AuthenticationResult.Timeout;
        }
        catch (GotrueException exception)
        {
            result = MapException(exception);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = AuthenticationResult.UnknownError;
        }

        OnAuthenticationResult?.Invoke(result);
        return result;
    }

    public void SignOut()
    {
        _client.Auth.SignOut();
    }

    private static AuthenticationResult MapException(GotrueException exception)
    {
        // GotrueException содержит StatusCode (HTTP) и ErrorCode
        int status = (int)exception.StatusCode;

        if (status == 429)
            return AuthenticationResult.TooManyRequests;

        if (status >= 500)
            return AuthenticationResult.NetworkError;

        Debug.LogWarning(exception);
        return AuthenticationResult.UnknownError;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _disposeCts.Cancel();
        _disposeCts.Dispose();
    }
}

public enum AuthenticationResult
{
    Success,
    Cancelled,
    NetworkError,
    Timeout,
    TooManyRequests,
    NotAuthorized,
    UnknownError
}

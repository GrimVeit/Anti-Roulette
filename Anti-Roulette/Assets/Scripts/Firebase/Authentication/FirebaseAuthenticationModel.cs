using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using System;
using UnityEngine;
using System.Threading;

public sealed class FirebaseAuthenticationModel : IDisposable
{
    private const int RequestTimeoutSeconds = 10;

    private readonly FirebaseAuth _auth;
    private readonly CancellationTokenSource _disposeCts = new();
    private bool _disposed;

    public FirebaseAuthenticationModel(FirebaseAuth auth)
    {
        _auth = auth;
    }

    public bool IsAuthorized => _auth.CurrentUser != null;
    public string UserId => _auth.CurrentUser?.UserId;

    public event Action<AuthenticationResult> OnAuthenticationResult;

    /// <summary>
    /// Гарантирует, что пользователь авторизован (анонимно).
    /// Если сессия уже есть — возвращает Success без сетевого запроса.
    /// </summary>
    public async UniTask<AuthenticationResult> EnsureSignedInAsync()
    {
        if (IsAuthorized)
            return AuthenticationResult.Success;

        AuthenticationResult result;

        try
        {
            await _auth
                .SignInAnonymouslyAsync()
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
        catch (FirebaseException exception)
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
        _auth.SignOut();
    }

    private static AuthenticationResult MapException(FirebaseException exception)
    {
        AuthError error = (AuthError)exception.ErrorCode;

        switch (error)
        {
            case AuthError.NetworkRequestFailed:
                return AuthenticationResult.NetworkError;

            case AuthError.TooManyRequests:
                return AuthenticationResult.TooManyRequests;

            default:
                Debug.LogWarning(exception);
                return AuthenticationResult.UnknownError;
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
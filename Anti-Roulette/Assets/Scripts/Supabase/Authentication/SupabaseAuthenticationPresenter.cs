using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class SupabaseAuthenticationPresenter : IAuthenticationProvider, IAuthenticationInfoProvider, IAuthenticationEventsProvider
{
    private readonly SupabaseAuthenticationModel _model;

    public SupabaseAuthenticationPresenter(SupabaseAuthenticationModel model)
    {
        _model = model;
    }

    public bool IsAuthorized => _model.IsAuthorized;
    public string UserId => _model.UserId;

    public UniTask<AuthenticationResult> EnsureSignedInAsync() => _model.EnsureSignedInAsync();

    public void SignOut() => _model.SignOut();

    public event Action<AuthenticationResult> OnAuthenticationResult
    {
        add => _model.OnAuthenticationResult += value;
        remove => _model.OnAuthenticationResult -= value;
    }
}

public interface IAuthenticationInfoProvider
{
    bool IsAuthorized { get; }
    string UserId { get; }
}

public interface IAuthenticationEventsProvider
{
    event Action<AuthenticationResult> OnAuthenticationResult;
}

public interface IAuthenticationProvider
{
    UniTask<AuthenticationResult> EnsureSignedInAsync();
    void SignOut();
}
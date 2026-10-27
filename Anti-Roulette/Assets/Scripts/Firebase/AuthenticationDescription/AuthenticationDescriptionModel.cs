using System;
using System.Collections.Generic;

public sealed class AuthenticationDescriptionModel
{
    private readonly Dictionary<AuthenticationResult, string> _descriptions = new()
    {
        [AuthenticationResult.Success] =
            string.Empty,

        [AuthenticationResult.Cancelled] =
            string.Empty,

        [AuthenticationResult.NotAuthorized] =
            "You are not authorized.",

        [AuthenticationResult.NetworkError] =
            "No internet connection. Please try again.",

        [AuthenticationResult.Timeout] =
            "The connection is taking too long. Please check your internet connection and try again.",

        [AuthenticationResult.TooManyRequests] =
            "Too many attempts. Please wait a moment and try again.",

        [AuthenticationResult.UnknownError] =
            "Something went wrong or no internet connection. Please try again."
    };

    private readonly IAuthenticationEventsProvider _authenticationEventsProvider;

    private readonly ISoundProvider _soundProvider;

    public AuthenticationDescriptionModel(IAuthenticationEventsProvider authenticationEventsProvider, ISoundProvider soundProvider)
    {
        _authenticationEventsProvider = authenticationEventsProvider;
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        _authenticationEventsProvider.OnAuthenticationResult += SetDescription;
    }

    public void Dispose()
    {
        _authenticationEventsProvider.OnAuthenticationResult -= SetDescription;
    }

    private void SetDescription(AuthenticationResult result)
    {
        if (result == AuthenticationResult.Success)
            _soundProvider.PlayOneShot("Registrate_Success");
        else
            _soundProvider.PlayOneShot("Registrate_Fail");

        if (!_descriptions.TryGetValue(result, out string description))
            return;

        OnSetDescription?.Invoke(description);
    }

    #region Output

    public event Action<string> OnSetDescription;

    #endregion
}

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RegistrationState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;

    private readonly IPlayerProfileInfoProvider _profileInfo;
    private readonly IAuthenticationProvider _authentication;
    private readonly IPlayerDatabaseProvider _database;
    private readonly IMoneyProvider _moneyProvider;
    private readonly IAuthLoadStatusProvider _authLoadStatusProvider;

    private readonly UIRoot_Menu _sceneRoot;

    public RegistrationState_Game(
        IStateProvider stateProvider,
        IPlayerProfileInfoProvider profileInfo,
        IAuthenticationProvider authentication,
        IPlayerDatabaseProvider database,
        UIRoot_Menu sceneRoot,
        IMoneyProvider moneyProvider,
        IAuthLoadStatusProvider authLoadStatusProvider)
    {
        _stateProvider = stateProvider;
        _profileInfo = profileInfo;
        _authentication = authentication;
        _database = database;
        _sceneRoot = sceneRoot;
        _moneyProvider = moneyProvider;
        _authLoadStatusProvider = authLoadStatusProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _authLoadStatusProvider.Load();

        AuthenticationResult authResult = await _authentication.EnsureSignedInAsync();

        if (authResult != AuthenticationResult.Success)
        {
            ChangeStateToProfileInput();
            _authLoadStatusProvider.Show();
            return;
        }

        _moneyProvider.SetZero();

        PlayerData playerData = new(_profileInfo.Profile.Nickname, 0);

        DatabaseResult dbResult = await _database.CreateOrUpdatePlayer(playerData);

        if (dbResult != DatabaseResult.Success)
            Debug.LogWarning($"CreateOrUpdatePlayer failed: {dbResult}");

        _sceneRoot.HideRegistrationGirlPanel();
        _sceneRoot.HideRegistrationPanel();

        ChangeStateToStartMain();
    }

    private void ChangeStateToProfileInput()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_NicknameProfileInputState_Menu>());
    }

    private void ChangeStateToStartMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_StartMainState_Menu>());
    }
}

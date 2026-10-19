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

    private readonly UIRoot_Menu _sceneRoot;

    public RegistrationState_Game(
        IStateProvider stateProvider,
        IPlayerProfileInfoProvider profileInfo,
        IAuthenticationProvider authentication,
        IPlayerDatabaseProvider database,
        UIRoot_Menu sceneRoot,
        IMoneyProvider moneyProvider)
    {
        _stateProvider = stateProvider;
        _profileInfo = profileInfo;
        _authentication = authentication;
        _database = database;
        _sceneRoot = sceneRoot;
        _moneyProvider = moneyProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        //_sceneRoot.ShowRegistrationLoadingPanel();

        AuthenticationResult authResult = await _authentication.EnsureSignedInAsync();

        if (authResult != AuthenticationResult.Success)
        {
            ChangeStateToProfileInput();
            return;
        }

        _moneyProvider.SetZero();

        PlayerData playerData = new(_profileInfo.Profile.Nickname, 0);

        DatabaseResult dbResult = await _database.CreateOrUpdatePlayer(playerData);

        if (dbResult != DatabaseResult.Success)
            Debug.LogWarning($"CreateOrUpdatePlayer failed: {dbResult}");

        ChangeStateToStartMain();
    }

    public override void Exit()
    {
        base.Exit();

        //_sceneRoot.HideRegistrationLoadingPanel();
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

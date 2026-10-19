using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Registration_StartMainState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IPlayerDatabaseProvider _playerDatabaseProvider;
    private readonly IAuthenticationInfoProvider _authenticationInfoProvider;
    private readonly IPlayerProfileInfoProvider _profileInfoProvider;
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IMoneyInfoProvider _moneyInfoProvider;

    public Registration_StartMainState_Menu(IStateProvider stateProvider, IPlayerDatabaseProvider playerDatabaseProvider, IAuthenticationInfoProvider authenticationInfoProvider, IPlayerProfileInfoProvider playerProfileInfoProvider, IDatabaseProvider databaseProvider, IMoneyInfoProvider moneyInfoProvider)
    {
        _stateProvider = stateProvider;
        _playerDatabaseProvider = playerDatabaseProvider;
        _authenticationInfoProvider = authenticationInfoProvider;
        _profileInfoProvider = playerProfileInfoProvider;
        _databaseProvider = databaseProvider;
        _moneyInfoProvider = moneyInfoProvider;
    }

    public void Enter()
    {
        if (_authenticationInfoProvider.IsAuthorized)
        {
            _playerDatabaseProvider.CreateOrUpdatePlayer(new PlayerData(_profileInfoProvider.Profile.Nickname, _moneyInfoProvider.Money)).Forget();
            _databaseProvider.GetTopPlayers(10).Forget();
        }

        ChangeStateToMain();
    }

    public void Exit()
    {

    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}

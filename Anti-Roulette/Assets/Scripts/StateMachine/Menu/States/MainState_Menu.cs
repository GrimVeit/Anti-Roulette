public class MainState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;
    private readonly ISceneService _sceneService;

    public MainState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot, ISceneService sceneService)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _sceneService = sceneService;
    }

    public void Enter()
    {
        _sceneRoot.OnClickShop_MainRightUp += ChangeStateToShop;
        _sceneRoot.OnClickLeaders_MainRightUp += ChangeStateToLeaderboard;
        _sceneRoot.OnClickSettings_MainLeftUp += ChangeStateToSettings;

        _sceneRoot.ShowMainLeftUpPanel();
        _sceneRoot.ShowMainRightUpPanel();
        _sceneRoot.ShowMainSpinPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickShop_MainRightUp -= ChangeStateToShop;
        _sceneRoot.OnClickLeaders_MainRightUp -= ChangeStateToLeaderboard;
        _sceneRoot.OnClickSettings_MainLeftUp -= ChangeStateToSettings;

        _sceneRoot.HideMainLeftUpPanel();
        _sceneRoot.HideMainRightUpPanel();
        _sceneRoot.HideMainSpinPanel();
    }

    private void ChangeStateToSettings()
    {
        _stateProvider.SetState(_stateProvider.GetState<SettingsState_Menu>());
    }

    private void ChangeStateToLeaderboard()
    {
        _stateProvider.SetState(_stateProvider.GetState<LeaderboardState_Menu>());
    }

    private void ChangeStateToShop()
    {
        _stateProvider.SetState(_stateProvider.GetState<ShopState_Menu>());
    }
}

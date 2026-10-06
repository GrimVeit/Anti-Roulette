public class MainState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISceneService _sceneService;
    private readonly UIRoot_Game _sceneRoot;

    public MainState_Game(IStateProvider stateProvider, ISceneService sceneService, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneService = sceneService;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    #region OUTPUT

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu,LoadingType.Default));
    }

    #endregion
}

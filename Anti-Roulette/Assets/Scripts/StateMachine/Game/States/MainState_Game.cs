using UnityEngine;

public class MainState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISceneService _sceneService;
    private readonly UIRoot_Game _sceneRoot;

    private readonly IChipSpawnerProvider _chipSpawnerProvider;
    private readonly IChipArrowInfo _chipArrowInfo;
    private readonly IChipSliderListen _chipSliderListen;

    private readonly IChipArrowActivator _chipArrowActivator;
    private readonly IChipSliderActivator _chipSliderActivator;
    private readonly IChipPseudoProvider _chipPseudoProvider;

    public MainState_Game(IStateProvider stateProvider, ISceneService sceneService, UIRoot_Game sceneRoot, IChipSpawnerProvider chipSpawnerProvider, IChipArrowInfo chipArrowInfo, IChipSliderListen chipSliderListen, IChipSliderActivator chipSliderActivator, IChipArrowActivator chipArrowActivator, IChipPseudoProvider chipPseudoProvider)
    {
        _stateProvider = stateProvider;
        _sceneService = sceneService;
        _sceneRoot = sceneRoot;
        _chipSpawnerProvider = chipSpawnerProvider;
        _chipArrowInfo = chipArrowInfo;
        _chipSliderListen = chipSliderListen;
        _chipSliderActivator = chipSliderActivator;
        _chipArrowActivator = chipArrowActivator;
        _chipPseudoProvider = chipPseudoProvider;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;

        _chipSliderListen.OnShotPower += Spawn;

        _chipArrowActivator.Activate();
        _chipSliderActivator.Activate();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_MainHeader -= ChangeSceneToMenu;

        _chipSliderListen.OnShotPower -= Spawn;

        _chipArrowActivator.Deactivate();
        _chipSliderActivator.Deactivate();
    }

    private void Spawn(float power)
    {
        _chipPseudoProvider.Delete();
        _chipArrowActivator.Deactivate();
        _chipSliderActivator.Deactivate();

        _chipSpawnerProvider.Spawn(_chipArrowInfo.Direction, power);
    }

    #region OUTPUT

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Black));
    }

    #endregion
}

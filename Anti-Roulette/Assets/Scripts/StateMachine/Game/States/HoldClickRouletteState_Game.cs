using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldClickRouletteState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly IRouletteHighlightActivator _highlightActivator;
    private readonly ISceneService _sceneService;

    public HoldClickRouletteState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IRouletteHighlightActivator highlightActivator, ISceneService sceneService)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _highlightActivator = highlightActivator;
        _sceneService = sceneService;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;

        _sceneRoot.OnClickRoulette_MainHeader += ChangeStateToRoulette;

        _highlightActivator.Activate();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_MainHeader -= ChangeSceneToMenu;

        _sceneRoot.OnClickRoulette_MainHeader -= ChangeStateToRoulette;

        _highlightActivator.Deactivate();

        _sceneRoot.HideMainPanel();
        _sceneRoot.HideMainHeaderPanel();
    }

    private void ChangeStateToRoulette()
    {
        _stateProvider.SetState(_stateProvider.GetState<RouletteSpinState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }
}

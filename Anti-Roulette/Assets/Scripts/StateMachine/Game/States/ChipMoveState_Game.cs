using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMoveState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IChipSpawnerListener _chipSpawnerListener;
    private readonly INumberSectorTriggerListener _numberSectorTriggerListener;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ISceneService _sceneService;

    public ChipMoveState_Game(IStateProvider stateProvider, IChipSpawnerListener chipSpawnerListener, INumberSectorTriggerListener numberSectorTriggerListener, UIRoot_Game sceneRoot, ISceneService sceneService)
    {
        _stateProvider = stateProvider;
        _chipSpawnerListener = chipSpawnerListener;
        _numberSectorTriggerListener = numberSectorTriggerListener;
        _sceneRoot = sceneRoot;
        _sceneService = sceneService;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;
        _numberSectorTriggerListener.OnSuccess += ChangeStateToWin;
        _numberSectorTriggerListener.OnFailed += ChangeStateToLose;
        _chipSpawnerListener.OnFailed += ChangeStateToLose;
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_MainHeader -= ChangeSceneToMenu;
        _numberSectorTriggerListener.OnSuccess -= ChangeStateToWin;
        _numberSectorTriggerListener.OnFailed -= ChangeStateToLose;
        _chipSpawnerListener.OnFailed -= ChangeStateToLose;
    }

    private void ChangeStateToLose()
    {
        _stateProvider.SetState(_stateProvider.GetState<LoseChipState_Game>());
    }

    private void ChangeStateToWin()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinChipState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }
}

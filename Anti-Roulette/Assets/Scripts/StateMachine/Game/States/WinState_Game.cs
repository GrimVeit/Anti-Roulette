using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WinState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ISceneService _sceneService;

    private readonly IFinishProvider _finishProvider;
    private readonly IChipPseudoProvider _chipPseudoProvider;
    private readonly IGameIncomeProvider _gameIncomeProvider;
    private readonly IChipSpawnerProvider _chipSpawnerProvider;

    public WinState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ISceneService sceneService, IFinishProvider finishProvider, IChipPseudoProvider chipPseudoProvider, IGameIncomeProvider gameIncomeProvider, IChipSpawnerProvider chipSpawnerProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _sceneService = sceneService;
        _finishProvider = finishProvider;
        _chipPseudoProvider = chipPseudoProvider;
        _gameIncomeProvider = gameIncomeProvider;
        _chipSpawnerProvider = chipSpawnerProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.OnClickExit_FinishButtons += ChangeSceneToMenu;
        _sceneRoot.OnClickRestart_FinishButtons += ChangeStateToRoulette;

        _sceneRoot.HideMainHeaderPanel();
        _sceneRoot.HideMainPanel();

        _sceneRoot.ShowFinishBackgroundPanel();

        await UniTask.WaitForSeconds(0.2f);

        _sceneRoot.ShowFinishLinePanel();

        await UniTask.WaitForSeconds(0.1f);

        _sceneRoot.ShowFinishButtonsPanel();

        await UniTask.WaitForSeconds(0.1f);

        _sceneRoot.ShowFinishGirlPanel();
    }

    public override void Exit()
    {
        _sceneRoot.OnClickExit_FinishButtons -= ChangeSceneToMenu;
        _sceneRoot.OnClickRestart_FinishButtons -= ChangeStateToRoulette;

        _sceneRoot.HideFinishBackgroundPanel();
        _sceneRoot.HideFinishLinePanel();
        _sceneRoot.HideFinishButtonsPanel();
        _sceneRoot.HideFinishGirlPanel();

        base.Exit();
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }

    private void ChangeStateToRoulette()
    {
        _chipPseudoProvider.Reload();
        _finishProvider.Reset();
        _gameIncomeProvider.Reset();
        _chipSpawnerProvider.Clear();

        _stateProvider.SetState(_stateProvider.GetState<RouletteSpinState_Game>());
    }
}

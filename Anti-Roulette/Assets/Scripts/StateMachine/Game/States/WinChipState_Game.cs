using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WinChipState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly IGameIncomeProvider _gameIncomeProvider;
    private readonly ISceneService _sceneService;
    private readonly IFinishProvider _finishProvider;
    private readonly ISoundProvider _soundProvider;
    private readonly ISound _soundBackground;

    public WinChipState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IGameIncomeProvider gameIncomeProvider, ISceneService sceneService, IFinishProvider finishProvider, ISoundProvider soundProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _gameIncomeProvider = gameIncomeProvider;
        _sceneService = sceneService;
        _finishProvider = finishProvider;
        _soundProvider = soundProvider;
        _soundBackground = _soundProvider.GetSound("Background");
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.OnClickExit_MainHeader += ChangeSceneToMenu;

        _soundBackground.SetVolumeEnd(0.3f, 0.2f, () => { _soundProvider.PlayOneShot("Chip_Win"); });

        _gameIncomeProvider.Add(10);

        _sceneRoot.ShowHitPanel();
        _sceneRoot.HideMainPanel();

        await UniTask.WaitForSeconds(4f);

        _sceneRoot.HideHitPanel();
        _sceneRoot.ShowMainPanel();

        _finishProvider.AddWin();

        _soundBackground.SetVolumeEnd(1, 0.2f);

        ChangeStateToRouletteClick();

        await UniTask.CompletedTask;
    }

    public override void Exit()
    {
        base.Exit();

        _sceneRoot.OnClickExit_MainHeader -= ChangeSceneToMenu;
    }

    private void ChangeStateToRouletteClick()
    {
        _stateProvider.SetState(_stateProvider.GetState<CheckFinishState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }
}

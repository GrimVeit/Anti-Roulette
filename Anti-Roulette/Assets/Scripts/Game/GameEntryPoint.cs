using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Game uIRoot;

    private UIRoot_Game _sceneRoot;
    private ViewContainer _viewContainer;

    private MoneyVisualPresenter _moneyVisualPresenter;
    private BackgroundVisualPresenter _backgroundVisualPresenter;
    private ProfileAvatarVisualPresenter _profileAvatarVisualPresenter;

    private StateMachine_Game _stateMachine;

    #region ENTRY

    public override async UniTask Initialize(DIContainer container)
    {
        _sceneRoot = Instantiate(uIRoot);
        container.RegisterInstance(_sceneRoot);

        var uiRootView = container.Resolve<UIRootView>();
        uiRootView.AttachSceneUI(_sceneRoot.gameObject, Camera.main);

        _viewContainer = _sceneRoot.GetComponent<ViewContainer>();
        _viewContainer.Initialize();
        container.RegisterInstance(_viewContainer);

        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    public override UniTask BeforeShutdown()
    {
        base.BeforeShutdown();

        _sceneRoot.Dispose();

        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();
        await base.ShutDown();

        _sceneRoot?.Dispose();
        _moneyVisualPresenter?.Dispose();

        _backgroundVisualPresenter?.Dispose();

        _profileAvatarVisualPresenter?.Dispose();

        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _sceneRoot.SetSoundProvider(_soundPresenter);

        _moneyVisualPresenter = new MoneyVisualPresenter(new MoneyVisualModel(_storeMoneyPresenter, _storeMoneyPresenter), _viewContainer.GetView<MoneyVisualView>());

        _backgroundVisualPresenter = new BackgroundVisualPresenter(_storeBackgroundPresenter, _viewContainer.GetView<BackgroundVisualView>());

        _profileAvatarVisualPresenter = new ProfileAvatarVisualPresenter(new ProfileAvatarVisualModel(_storePlayerProfilePresenter, _storePlayerProfilePresenter), _viewContainer.GetView<ProfileAvatarVisualView>());

        _stateMachine = new StateMachine_Game(container);

        _sceneRoot.Initialize();
        _moneyVisualPresenter.Initialize();

        _backgroundVisualPresenter.Initialize();

        _profileAvatarVisualPresenter.Initialize();
        
        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}

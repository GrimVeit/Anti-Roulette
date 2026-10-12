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
    private WheelVisualPresenter _wheelVisualPresenter;
    private ProfileAvatarVisualPresenter _profileAvatarVisualPresenter;

    private ChipPseudoSpawnerPresenter _chipPseudoSpawnerPresenter;
    private ChipCounterPresenter _chipCounterPresenter;

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

        _sceneRoot.Dispose();
        _moneyVisualPresenter?.Dispose();

        _profileAvatarVisualPresenter?.Dispose();

        _chipPseudoSpawnerPresenter?.Dispose();
        _chipCounterPresenter?.Dispose();

        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _sceneRoot.SetSoundProvider(_soundPresenter);

        _moneyVisualPresenter = new MoneyVisualPresenter(new MoneyVisualModel(_storeMoneyPresenter, _storeMoneyPresenter), _viewContainer.GetView<MoneyVisualView>());

        _backgroundVisualPresenter = new BackgroundVisualPresenter(_storeBackgroundPresenter, _viewContainer.GetView<BackgroundVisualView>());
        _wheelVisualPresenter = new WheelVisualPresenter(_storeWheelPresenter, _viewContainer.GetView<WheelVisualView>());

        _profileAvatarVisualPresenter = new ProfileAvatarVisualPresenter(new ProfileAvatarVisualModel(_storePlayerProfilePresenter, _storePlayerProfilePresenter), _viewContainer.GetView<ProfileAvatarVisualView>());

        _chipPseudoSpawnerPresenter = new ChipPseudoSpawnerPresenter(new ChipPseudoSpawnerModel(_storeChipPresenter, 5), _viewContainer.GetView<ChipPseudoSpawnerView>());
        _chipCounterPresenter = new ChipCounterPresenter(new ChipCounterModel(_chipPseudoSpawnerPresenter, 5), _viewContainer.GetView<ChipCounterView>());

        _stateMachine = new StateMachine_Game(container);

        _sceneRoot.Initialize();
        _moneyVisualPresenter.Initialize();

        _backgroundVisualPresenter.Initialize();
        _wheelVisualPresenter.Initialize();

        _profileAvatarVisualPresenter.Initialize();

        _chipPseudoSpawnerPresenter.Initialize();
        _chipCounterPresenter.Initialize();
        
        return UniTask.CompletedTask;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _chipPseudoSpawnerPresenter.MoveToStart();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _chipPseudoSpawnerPresenter.Delete();
        }

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            _chipPseudoSpawnerPresenter.Reload();
        }
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}

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

    private FinishPresenter _finishPresenter;

    private StoreNumberSectorPresenter _storeNumberSectorPresenter;
    private NumberSectorVisualPresenter _numberSectorVisualPresenter;
    private NumberSectorHighlightPresenter _numberSectorHighlightPresenter;
    private NumberSectorTriggerPresenter _numberSectorTriggerPresenter;

    private RouletteHighlightPresenter _rouletteHighlightPresenter;
    private RouletteSpinPresenter _rouletteSpinPresenter;

    private ChipPseudoSpawnerPresenter _chipPseudoSpawnerPresenter;
    private ChipCounterPresenter _chipCounterPresenter;
    private ChipArrowPresenter _chipArrowPresenter;
    private ChipSliderPresenter _chipSliderPresenter;

    private ChipSpawnerPresenter _chipSpawnerPresenter;

    private GameIncomePresenter _gameIncomePresenter;

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

        _finishPresenter?.Dispose();

        _numberSectorVisualPresenter?.Dispose();
        _numberSectorHighlightPresenter?.Dispose();
        _numberSectorTriggerPresenter?.Dispose();

        _rouletteSpinPresenter?.Dispose();

        _chipPseudoSpawnerPresenter?.Dispose();
        _chipCounterPresenter?.Dispose();
        _chipArrowPresenter?.Dispose();
        _chipSliderPresenter?.Dispose();

        _chipSpawnerPresenter?.Dispose();

        _gameIncomePresenter?.Dispose();

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

        _finishPresenter = new FinishPresenter(new FinishModel(), _viewContainer.GetView<FinishView>());
        container.RegisterInstance<IFinishInfo>(_finishPresenter);
        container.RegisterInstance<IFinishProvider>(_finishPresenter);

        _storeNumberSectorPresenter = new StoreNumberSectorPresenter(new StoreNumberSectorModel());
        container.RegisterInstance<IStoreNumberSectorListener>(_storeNumberSectorPresenter);
        container.RegisterInstance<IStoreNumberSectorProvider>(_storeNumberSectorPresenter);

        _numberSectorVisualPresenter = new NumberSectorVisualPresenter(new NumberSectorVisualModel(_storeNumberSectorPresenter), _viewContainer.GetView<NumberSectorVisualView>());

        _numberSectorHighlightPresenter = new NumberSectorHighlightPresenter(new NumberSectorHighlightModel(_storeNumberSectorPresenter), _viewContainer.GetView<NumberSectorHighlightView>());
        container.RegisterInstance<INumberSectorHighlightProvider>(_numberSectorHighlightPresenter);

        _numberSectorTriggerPresenter = new NumberSectorTriggerPresenter(new NumberSectorTriggerModel(_storeNumberSectorPresenter), _viewContainer.GetView<NumberSectorTriggerView>());
        container.RegisterInstance<INumberSectorTriggerListener>(_numberSectorTriggerPresenter);


        _rouletteHighlightPresenter = new RouletteHighlightPresenter(_viewContainer.GetView<RouletteHighlightView>());
        container.RegisterInstance<IRouletteHighlightActivator>(_rouletteHighlightPresenter);

        _rouletteSpinPresenter = new RouletteSpinPresenter(new RouletteSpinModel(_soundPresenter), _viewContainer.GetView<RouletteSpinView>());
        container.RegisterInstance<IRouletteSpinProvider>(_rouletteSpinPresenter);


        _chipPseudoSpawnerPresenter = new ChipPseudoSpawnerPresenter(new ChipPseudoSpawnerModel(_storeChipPresenter, 2, _soundPresenter), _viewContainer.GetView<ChipPseudoSpawnerView>());
        container.RegisterInstance<IChipPseudoProvider>(_chipPseudoSpawnerPresenter);
        container.RegisterInstance<IChipPseudoListener>(_chipPseudoSpawnerPresenter);

        _chipCounterPresenter = new ChipCounterPresenter(new ChipCounterModel(_chipPseudoSpawnerPresenter, 2), _viewContainer.GetView<ChipCounterView>());
        container.RegisterInstance<IChipCounterInfo>(_chipCounterPresenter);

        _chipArrowPresenter = new ChipArrowPresenter(new ChipArrowModel(), _viewContainer.GetView<ChipArrowView>());
        container.RegisterInstance<IChipArrowInfo>(_chipArrowPresenter);
        container.RegisterInstance<IChipArrowActivator>(_chipArrowPresenter);

        _chipSliderPresenter = new ChipSliderPresenter(new ChipSliderModel(_soundPresenter), _viewContainer.GetView<ChipSliderView>());
        container.RegisterInstance<IChipSliderListen>(_chipSliderPresenter);
        container.RegisterInstance<IChipSliderActivator>(_chipSliderPresenter);

        _chipSpawnerPresenter = new ChipSpawnerPresenter(new ChipSpawnerModel(_storeChipPresenter), _viewContainer.GetView<ChipSpawnerView>());
        container.RegisterInstance<IChipSpawnerProvider>(_chipSpawnerPresenter);
        container.RegisterInstance<IChipSpawnerListener>(_chipSpawnerPresenter);

        _gameIncomePresenter = new GameIncomePresenter(new GameIncomeModel(_storeMoneyPresenter), _viewContainer.GetView<GameIncomeView>());
        container.RegisterInstance<IGameIncomeProvider>(_gameIncomePresenter);

        _stateMachine = new StateMachine_Game(container);

        _sceneRoot.Initialize();
        _moneyVisualPresenter.Initialize();

        _backgroundVisualPresenter.Initialize();
        _wheelVisualPresenter.Initialize();

        _profileAvatarVisualPresenter.Initialize();

        _finishPresenter.Initialize();

        _numberSectorVisualPresenter.Initialize();
        _numberSectorHighlightPresenter.Initialize();
        _numberSectorTriggerPresenter.Initialze();

        _rouletteSpinPresenter.Initialize();

        _chipPseudoSpawnerPresenter.Initialize();
        _chipCounterPresenter.Initialize();
        _chipArrowPresenter.Initialize();
        _chipSliderPresenter.Initialize();

        _chipSpawnerPresenter.Initialize();

        _gameIncomePresenter.Initialize();
        
        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}

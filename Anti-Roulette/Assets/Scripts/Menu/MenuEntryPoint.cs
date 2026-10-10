using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Menu uIRoot;

    private UIRoot_Menu _uIRoot;
    private ViewContainer _viewContainer;
    private VolumeSettingsPresenter _volumeSettingsPresenter;
    private MoneyVisualPresenter _moneyVisualPresenter;

    private BackgroundShopVisualPresenter _backgroundShopVisualPresenter;
    private WheelShopVisualPresenter _wheelShopVisualPresenter;
    private ChipShopVisualPresenter _chipShopVisualPresenter;
    private ShopTypeVisualPresenter _shopTypeVisualPresenter;

    private BackgroundSelectVisualPresenter _backgroundSelectVisualPresenter;
    private WheelSelectVisualPresenter _wheelSelectVisualPresenter;
    private ChipSelectVisualPresenter _chipSelectVisualPresenter;

    private ProfileAvatarSelectPresenter _profileAvatarSelectPresenter;
    private ProfileAvatarVisualPresenter _profileAvatarVisualPresenter;

    private VideoPresenter _videoPresenter;
    private StateMachine_Menu _stateMachine;

    #region ENTRY

    public override async UniTask Initialize(DIContainer container)
    {
        _uIRoot = Instantiate(uIRoot);
        container.RegisterInstance(_uIRoot);

        var uiRootView = container.Resolve<UIRootView>();
        uiRootView.AttachSceneUI(
            _uIRoot.gameObject,
            Camera.main
        );

        _viewContainer = _uIRoot.GetComponent<ViewContainer>();
        _viewContainer.Initialize();
        container.RegisterInstance(_viewContainer);

        _videoPresenter = new VideoPresenter(new VideoModel(), _viewContainer.GetView<VideoView>());
        container.RegisterInstance<IVideoProvider>(_videoPresenter);
        await _videoPresenter.Initialize();

        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    public override UniTask BeforeShutdown()
    {
        base.BeforeShutdown();

        _uIRoot.Dispose();

        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();
        await base.ShutDown();

        _volumeSettingsPresenter?.Dispose();
        _moneyVisualPresenter?.Dispose();

        _backgroundShopVisualPresenter?.Dispose();
        _wheelShopVisualPresenter?.Dispose();
        _chipShopVisualPresenter?.Dispose();
        _shopTypeVisualPresenter?.Dispose();

        _backgroundSelectVisualPresenter?.Dispose();
        _wheelSelectVisualPresenter?.Dispose();
        _chipSelectVisualPresenter?.Dispose();

        _profileAvatarSelectPresenter?.Dispose();
        _profileAvatarVisualPresenter?.Dispose();

        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _uIRoot.SetSoundProvider(_soundPresenter);

        _volumeSettingsPresenter = new VolumeSettingsPresenter(new VolumeSettingsModel(_storeSoundSettingsPresenter,_storeSoundSettingsPresenter,_storeSoundSettingsPresenter), _viewContainer.GetView<VolumeSettingsView>());
        _moneyVisualPresenter = new MoneyVisualPresenter(new MoneyVisualModel(_storeMoneyPresenter,_storeMoneyPresenter),_viewContainer.GetView<MoneyVisualView>());

        _backgroundShopVisualPresenter = new BackgroundShopVisualPresenter(new BackgroundShopVisualModel(_storeBackgroundPresenter, _storeBackgroundPresenter, _storeBackgroundPresenter, _storeMoneyPresenter), _viewContainer.GetView<BackgroundShopVisualView>());
        _wheelShopVisualPresenter = new WheelShopVisualPresenter(new WheelShopVisualModel(_storeWheelPresenter, _storeWheelPresenter, _storeWheelPresenter, _storeMoneyPresenter), _viewContainer.GetView<WheelShopVisualView>());
        _chipShopVisualPresenter = new ChipShopVisualPresenter(new ChipShopVisualModel(_storeChipPresenter, _storeChipPresenter, _storeChipPresenter, _storeMoneyPresenter), _viewContainer.GetView<ChipShopVisualView>());
        _shopTypeVisualPresenter = new ShopTypeVisualPresenter(new ShopTypeVisualModel(), _viewContainer.GetView<ShopTypeVisualView>());

        _backgroundSelectVisualPresenter = new BackgroundSelectVisualPresenter(new BackgroundSelectVisualModel(_storeBackgroundPresenter, _storeBackgroundPresenter, _storeBackgroundPresenter), _viewContainer.GetView<BackgroundSelectVisualView>());
        _wheelSelectVisualPresenter = new WheelSelectVisualPresenter(new WheelSelectVisualModel(_storeWheelPresenter, _storeWheelPresenter, _storeWheelPresenter), _viewContainer.GetView<WheelSelectVisualView>());
        _chipSelectVisualPresenter = new ChipSelectVisualPresenter(new ChipSelectVisualModel(_storeChipPresenter, _storeChipPresenter, _storeChipPresenter), _viewContainer.GetView<ChipSelectVisualView>());

        _profileAvatarSelectPresenter = new ProfileAvatarSelectPresenter(new ProfileAvatarSelectModel(_storePlayerProfilePresenter, _storePlayerProfilePresenter, _storePlayerProfilePresenter), _viewContainer.GetView<ProfileAvatarSelectView>());
        _profileAvatarVisualPresenter = new ProfileAvatarVisualPresenter(new ProfileAvatarVisualModel(_storePlayerProfilePresenter, _storePlayerProfilePresenter), _viewContainer.GetView<ProfileAvatarVisualView>());

        _stateMachine = new StateMachine_Menu(container);

        _uIRoot.Initialize();
        Debug.Log("LOL");
        _volumeSettingsPresenter.Initialize();
        _moneyVisualPresenter.Initialize();

        _backgroundShopVisualPresenter.Initialize();
        _wheelShopVisualPresenter.Initialize();
        _chipShopVisualPresenter.Initialize();
        _shopTypeVisualPresenter.Initialize();

        _backgroundSelectVisualPresenter.Initialize();
        _wheelSelectVisualPresenter.Initialize();
        _chipSelectVisualPresenter.Initialize();

        _profileAvatarSelectPresenter.Initialize();
        _profileAvatarVisualPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}

using BaCon;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class MenuEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Menu uIRoot;

    private UIRoot_Menu _uIRoot;
    private ViewContainer _viewContainer;
    private VolumeSettingsPresenter _volumeSettingsPresenter;
    private MoneyVisualPresenter _moneyVisualPresenter;

    private FirebaseAuthenticationPresenter _firebaseAuthenticationPresenter;
    private FirebasePlayerDatabasePresenter _firebasePlayerDatabasePresenter;
    private FirebaseDatabasePresenter _firebaseDatabasePresenter;
    private AuthenticationDescriptionPresenter _authenticationDescriptionPresenter;
    private ProfileNicknameInputPresenter _profileNicknameInputPresenter;
    private LeaderboardPresenter _leaderboardPresenter;

    private AuthenticationLoadStatusPresenter _authenticationLoadStatusPresenter;

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

        _firebaseDatabasePresenter?.Cancel();
        _firebasePlayerDatabasePresenter?.Cancel();

        _authenticationDescriptionPresenter?.Dispose();
        _profileNicknameInputPresenter?.Dispose();
        _authenticationLoadStatusPresenter?.Dispose();
        _leaderboardPresenter?.Dispose();

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
        //-----------------------FIREBASE---------------------//

        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;
        FirebaseDatabase database = FirebaseDatabase.DefaultInstance;

        _firebaseAuthenticationPresenter = new FirebaseAuthenticationPresenter(new FirebaseAuthenticationModel(firebaseAuth));
        container.RegisterInstance<IAuthenticationInfoProvider>(_firebaseAuthenticationPresenter);
        container.RegisterInstance<IAuthenticationProvider>(_firebaseAuthenticationPresenter);

        _firebasePlayerDatabasePresenter = new FirebasePlayerDatabasePresenter(new FirebasePlayerDatabaseModel(database, _firebaseAuthenticationPresenter));
        container.RegisterInstance<IPlayerDatabaseProvider>(_firebasePlayerDatabasePresenter);

        _firebaseDatabasePresenter = new FirebaseDatabasePresenter(new FirebaseDatabaseModel(database));
        container.RegisterInstance<IDatabaseProvider>(_firebaseDatabasePresenter);

        _leaderboardPresenter = new LeaderboardPresenter(new LeaderboardModel(_firebaseDatabasePresenter), _viewContainer.GetView<LeaderboardView>());

        _authenticationDescriptionPresenter = new AuthenticationDescriptionPresenter(new AuthenticationDescriptionModel(_firebaseAuthenticationPresenter, _soundPresenter), _viewContainer.GetView<AuthenticationDescriptionView>());

        _profileNicknameInputPresenter = new ProfileNicknameInputPresenter(new ProfileNicknameInputModel(_storePlayerProfilePresenter), _viewContainer.GetView<ProfileNicknameInputView>());

        _authenticationLoadStatusPresenter = new AuthenticationLoadStatusPresenter(new AuthenticationLoadStatusModel(), _viewContainer.GetView<AuthenticationLoadStatusView>());
        container.RegisterInstance<IAuthLoadStatusProvider>(_authenticationLoadStatusPresenter);

        //-----------------------------------------------------//

        _uIRoot.SetSoundProvider(_soundPresenter);

        _volumeSettingsPresenter = new VolumeSettingsPresenter(new VolumeSettingsModel(_storeSoundSettingsPresenter,_storeSoundSettingsPresenter,_storeSoundSettingsPresenter), _viewContainer.GetView<VolumeSettingsView>());
        _moneyVisualPresenter = new MoneyVisualPresenter(new MoneyVisualModel(_storeMoneyPresenter,_storeMoneyPresenter),_viewContainer.GetView<MoneyVisualView>());

        _backgroundShopVisualPresenter = new BackgroundShopVisualPresenter(new BackgroundShopVisualModel(_storeBackgroundPresenter, _storeBackgroundPresenter, _storeBackgroundPresenter, _storeMoneyPresenter, _soundPresenter), _viewContainer.GetView<BackgroundShopVisualView>());
        _wheelShopVisualPresenter = new WheelShopVisualPresenter(new WheelShopVisualModel(_storeWheelPresenter, _storeWheelPresenter, _storeWheelPresenter, _storeMoneyPresenter, _soundPresenter), _viewContainer.GetView<WheelShopVisualView>());
        _chipShopVisualPresenter = new ChipShopVisualPresenter(new ChipShopVisualModel(_storeChipPresenter, _storeChipPresenter, _storeChipPresenter, _storeMoneyPresenter, _soundPresenter), _viewContainer.GetView<ChipShopVisualView>());
        _shopTypeVisualPresenter = new ShopTypeVisualPresenter(new ShopTypeVisualModel(_soundPresenter), _viewContainer.GetView<ShopTypeVisualView>());

        _backgroundSelectVisualPresenter = new BackgroundSelectVisualPresenter(new BackgroundSelectVisualModel(_storeBackgroundPresenter, _storeBackgroundPresenter, _storeBackgroundPresenter, _soundPresenter), _viewContainer.GetView<BackgroundSelectVisualView>());
        _wheelSelectVisualPresenter = new WheelSelectVisualPresenter(new WheelSelectVisualModel(_storeWheelPresenter, _storeWheelPresenter, _storeWheelPresenter, _soundPresenter), _viewContainer.GetView<WheelSelectVisualView>());
        _chipSelectVisualPresenter = new ChipSelectVisualPresenter(new ChipSelectVisualModel(_storeChipPresenter, _storeChipPresenter, _storeChipPresenter, _soundPresenter), _viewContainer.GetView<ChipSelectVisualView>());

        _profileAvatarSelectPresenter = new ProfileAvatarSelectPresenter(new ProfileAvatarSelectModel(_storePlayerProfilePresenter, _storePlayerProfilePresenter, _storePlayerProfilePresenter, _soundPresenter), _viewContainer.GetView<ProfileAvatarSelectView>());
        _profileAvatarVisualPresenter = new ProfileAvatarVisualPresenter(new ProfileAvatarVisualModel(_storePlayerProfilePresenter, _storePlayerProfilePresenter), _viewContainer.GetView<ProfileAvatarVisualView>());

        _stateMachine = new StateMachine_Menu(container);

        _uIRoot.Initialize();
        Debug.Log("LOL");
        _volumeSettingsPresenter.Initialize();
        _moneyVisualPresenter.Initialize();

        _authenticationDescriptionPresenter.Initialize();
        _profileNicknameInputPresenter.Initialize();
        _authenticationLoadStatusPresenter.Initialize();
        _leaderboardPresenter.Initialize();

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

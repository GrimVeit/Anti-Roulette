using System;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StartPrepareEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Prepare uIRoot;

    private ViewContainer _viewContainer;
    private UIRoot_Prepare _uIRoot;
    private ISceneService _sceneService;
    private VideoPresenter _videoPresenter;
    private SupabaseDatabasePresenter _supabaseDatabasePresenter;
    private GeoLocationPresenter _geoLocationPresenter;
    private URLResolverPresenter _urlResolverPresenter;
    private InternetPresenter _internetPresenter;
    private WebViewPresenter _webViewPresenter;

    private readonly string Nickname = "Tester902";

    #region ENTRY

    public override async UniTask Initialize(DIContainer container)
    {
        _sceneService = container.Resolve<ISceneService>();

        _uIRoot = Instantiate(uIRoot);
        container.RegisterInstance(_uIRoot);
        _uIRoot.Initialize();

        var uiRootView = container.Resolve<UIRootView>();
        uiRootView.AttachSceneUI(
            _uIRoot.gameObject,
            Camera.main
        );

        _viewContainer = _uIRoot.GetComponent<ViewContainer>();
        _viewContainer.Initialize();
        container.RegisterInstance(_viewContainer);

        _uIRoot.ShowPreparePanel();

        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    public override UniTask BeforeShutdown()
    {
        base.BeforeShutdown();

        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();

        _webViewPresenter.Dispose();

        await base.ShutDown();
    }

    #endregion

    protected override async UniTask OnBaseInitialized(DIContainer container)
    {

        //-----------------------FIREBASE---------------------//

        _supabaseDatabasePresenter = new SupabaseDatabasePresenter(new SupabaseDatabaseModel(container.Resolve<ISupabaseProvider>().Client));
        container.RegisterInstance<IDatabaseProvider>(_supabaseDatabasePresenter);

        //-----------------------------------------------------//

        _geoLocationPresenter = new GeoLocationPresenter(new GeoLocationModel());

        _urlResolverPresenter = new URLResolverPresenter(new UrlResolverModel());

        _internetPresenter = new InternetPresenter(new InternetModel());

        _webViewPresenter = new WebViewPresenter(new WebViewModel(), _viewContainer.GetView<WebViewView>());

        _videoPresenter = new VideoPresenter(new VideoModel(), _viewContainer.GetView<VideoView>());

        _webViewPresenter.Initialize();

        await _videoPresenter.Initialize();

        await UniTask.WaitForSeconds(0.1f);

        _videoPresenter.Play("Prepare");

        await UniTask.CompletedTask;
    }

    protected override async UniTask OnSceneInitialized(DIContainer container)
    {
        // Дедлайн 9 секунд — только для "нет интернета" и "запрос за первым местом"
        var deadline = Time.realtimeSinceStartup + 9f;

        Debug.LogWarning("Проверка интернета");

        // NETWORK
        if (!_internetPresenter.HasNetwork)
        {
            Debug.LogError("НЕТ ИНТЕРНЕТА");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None), deadline).Forget();
            return;
        }

        // FIRST PERSON
        Debug.LogWarning("Запрос за первым игроком");

        (DatabaseResult resultPlayerByPlace, PlayerData playerData) = await _supabaseDatabasePresenter.GetPlayerByPlace(1);

        if (resultPlayerByPlace != DatabaseResult.Success || playerData == null)
        {
            Debug.LogError("НЕУДАЧНЫЙ ЗАПРОС ЗА ПЕРВЫМ ЧЕЛОВЕКОМ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None), deadline).Forget();
            return;
        }

        if (string.IsNullOrEmpty(playerData.Nickname) || playerData.Nickname != Nickname)
        {
            Debug.LogError("НЕ ТОТ ЧЕЛОВЕК НА ПЕРВОМ МЕСТЕ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None), deadline).Forget();
            return;
        }

        // ===== Дальше таймер 9 секунд не нужен, порядок как есть =====

        Debug.LogWarning("Запрос за гео игрока");

        (GeoLocationResult resultPlayerIp, IPInfo ipInfo) = await _geoLocationPresenter.GetUserLocation();

        if (resultPlayerIp != GeoLocationResult.Success || ipInfo == null)
        {
            Debug.LogError("НЕУДАЧНЫЙ ЗАПРОС ЗА ГЕО ИГРОКА");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if (string.IsNullOrEmpty(ipInfo.country))
        {
            Debug.LogError("НЕТ СТРАНЫ В ДАННЫХ ГЕО");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        // GEO_LINK_NICKNAME
        Debug.LogWarning("Запрос за данными серыми");

        (DatabaseResult resultLinkGeo, LinkGeoData linkGeoData) = await _supabaseDatabasePresenter.GetLinkGeoData();

        if (resultLinkGeo != DatabaseResult.Success || linkGeoData == null)
        {
            Debug.LogError("НЕУДАЧНЫЙ ЗАПРОС ЗА СЕРЫМИ ДАННЫМИ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if (string.IsNullOrEmpty(linkGeoData.Link) || linkGeoData.Geo == null)
        {
            Debug.LogError("НЕТ В ДАННЫХ ИЛИ ССЫЛКИ ИЛИ ГЕО");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if (!linkGeoData.Geo.Contains(ipInfo.country))
        {
            Debug.LogError("НЕ ТО ГЕО");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        // UniWebView
        Debug.Log(linkGeoData.Link.ToString());
        (UrlResolverResult resultUrlResorve, string Url) =
            await _urlResolverPresenter.ResolveFromTitle(linkGeoData.Link);

        if (resultUrlResorve != UrlResolverResult.Success || string.IsNullOrEmpty(Url))
        {
            Debug.LogError("НЕУДАЧНЫЙ ЗАПРОС ЗА ССЫЛКОЙ В ТАЙТЛ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        _webViewPresenter.SetURL("https://google.com");
        _webViewPresenter.Load();
        _webViewPresenter.Show();
    }

    /// <summary>
    /// Для первых двух проверок (нет интернета / первое место) — ждём до абсолютного дедлайна (9 сек от старта).
    /// Для остальных веток — стандартные 50 мс (time по умолчанию).
    /// </summary>
    private async UniTask StartAutoTransition(SceneTransition transition, float? deadline = null)
    {
        float delayMs = 50f;

        if (deadline.HasValue)
        {
            var remainingSec = deadline.Value - Time.realtimeSinceStartup;
            delayMs = remainingSec > 0f ? remainingSec * 1000f : 0f;
        }

        Debug.Log($"StartAutoTransition delay: {delayMs:F0} ms");

        if (delayMs > 0f)
            await UniTask.Delay(TimeSpan.FromMilliseconds(delayMs), ignoreTimeScale: true);

        _uIRoot.HidePreparePanel();

        await UniTask.WaitForSeconds(0.12f);

        await _sceneService.ChangeSceneAsync(transition);
    }
}

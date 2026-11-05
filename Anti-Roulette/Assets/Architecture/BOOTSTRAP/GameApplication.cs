using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class GameApplication : IDisposable
{
    public DIContainer GlobalContainer { get; }

    private AppServices services;
    private UIRootView uiRoot;

    public GameApplication()
    {
        GlobalContainer = new DIContainer();

        services = new AppServices(GlobalContainer);

        RegisterServices();
        RegisterGlobalObjects();
    }


    public async UniTask Start()
    {
        await UniTask.Yield(PlayerLoopTiming.Update);

        await services.InitializeAsync();

        await uiRoot.Initialize();

        var sceneService = GlobalContainer.Resolve<ISceneService>();
        await sceneService.ChangeSceneAsync(new SceneTransition(Scenes.StartPrepare, LoadingType.None));
    }


    private void RegisterServices()
    {
        var sceneService = new SceneService(GlobalContainer);
        GlobalContainer.RegisterInstance<ISceneService>(sceneService);

        var storeSessionPresenter = new StoreSessionPresenter(new StoreSessionModel());
        GlobalContainer.RegisterInstance<ISessionInfoProvider>(storeSessionPresenter);
        GlobalContainer.RegisterInstance<ISessionProvider>(storeSessionPresenter);
    }


    private void RegisterGlobalObjects()
    {
        var uiRootPrefab = Resources.Load<UIRootView>("UIRootView");

        if (uiRootPrefab == null)
        {
            throw new InvalidOperationException("UIRootView prefab was not found in Assets/Resources.");
        }

        uiRoot = Object.Instantiate(uiRootPrefab);
        Object.DontDestroyOnLoad(uiRoot.gameObject);
        GlobalContainer.RegisterInstance(uiRoot);
    }


    public void Dispose()
    {
        if (uiRoot != null)
        {
            Object.Destroy(uiRoot.gameObject);
            uiRoot = null;
        }
    }
}

public sealed class AppServices : IAsyncDisposable
{
    private readonly List<IAppService> _orderedServices = new();
    public SupabaseProvider Supabase { get; }

    public AppServices(DIContainer container)
    {
        Supabase = new SupabaseProvider("https://xrnztbsbvqrurvxtvcqi.supabase.co", "sb_publishable_WzMp-gGcp9TEyXX4cipYrQ_3k0vyQqh");

        _orderedServices.Add(Supabase);

        container.RegisterInstance<ISupabaseProvider>(Supabase);
    }

    public async UniTask InitializeAsync()
    {
        foreach (var service in _orderedServices)
        {
            await service.InitializeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        // в обратном порядке — чтобы зависимости умирали после тех, кто их использует
        for (int i = _orderedServices.Count - 1; i >= 0; i--)
        {
            if (_orderedServices[i] is IAsyncDisposable d)
                await d.DisposeAsync();
        }
        _orderedServices.Clear();
    }
}

public interface IAppService
{
    UniTask InitializeAsync();
}
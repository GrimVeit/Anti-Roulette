using System;
using System.Collections.Generic;

public class BackgroundSelectVisualModel
{
    private readonly IBackgroundInfoProvider _backgroundInfoProvider;
    private readonly IBackgroundListener _backgroundListener;
    private readonly IBackgroundProvider _backgroundProvider;

    public BackgroundSelectVisualModel(IBackgroundInfoProvider backgroundInfoProvider, IBackgroundListener backgroundListener, IBackgroundProvider backgroundProvider)
    {
        _backgroundInfoProvider = backgroundInfoProvider;
        _backgroundListener = backgroundListener;
        _backgroundProvider = backgroundProvider;
    }

    public void Initialize()
    {
        _backgroundListener.OnOpenBackground += OpenBackground;
        _backgroundListener.OnSelectBackground += SelectBackground;

        OnSetBackgrounds?.Invoke(_backgroundInfoProvider.GetBackgrounds());
        OnSelectBackground?.Invoke(_backgroundInfoProvider.CurrentBackgroundIndex);
    }

    public void Dispose()
    {
        _backgroundListener.OnOpenBackground -= OpenBackground;
        _backgroundListener.OnSelectBackground -= SelectBackground;
    }

    public void Select(int index)
    {
        if(_backgroundInfoProvider.CurrentBackgroundIndex == index) return;

        if(!_backgroundInfoProvider.GetBackground(index).IsOpened) return;

        OnDeselectBackground?.Invoke(_backgroundInfoProvider.CurrentBackgroundIndex);

        _backgroundProvider.SelectBackground(index);
    }

    private void OpenBackground(Background background)
    {
        OnOpenBackground?.Invoke(background.Index);
    }

    private void SelectBackground(Background background)
    {
        OnSelectBackground?.Invoke(background.Index);
    }

    #region Output

    public event Action<IReadOnlyList<Background>> OnSetBackgrounds;
    public event Action<int> OnOpenBackground;
    public event Action<int> OnSelectBackground;
    public event Action<int> OnDeselectBackground;

    #endregion
}

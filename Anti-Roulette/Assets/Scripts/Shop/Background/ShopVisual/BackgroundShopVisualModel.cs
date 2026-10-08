using System.Collections.Generic;
using System;

public class BackgroundShopVisualModel
{
    private readonly IBackgroundInfoProvider _backgroundInfoProvider;
    private readonly IBackgroundListener _backgroundListener;
    private readonly IBackgroundProvider _backgroundProvider;
    private readonly IMoneyProvider _moneyProvider;

    public BackgroundShopVisualModel(IBackgroundInfoProvider backgroundInfoProvider, IBackgroundListener backgroundListener, IBackgroundProvider backgroundProvider, IMoneyProvider moneyProvider)
    {
        _backgroundInfoProvider = backgroundInfoProvider;
        _backgroundListener = backgroundListener;
        _backgroundProvider = backgroundProvider;
        _moneyProvider = moneyProvider;
    }

    public void Initialize()
    {
        _backgroundListener.OnOpenBackground += OpenBackground;

        OnSetBackgrounds?.Invoke(_backgroundInfoProvider.GetBackgrounds());
    }

    public void Dispose()
    {
        _backgroundListener.OnOpenBackground -= OpenBackground;
    }

    private void OpenBackground(Background background)
    {
        OnOpenBackground?.Invoke(background.Index);
    }

    public void Buy(int index)
    {
        if (_backgroundInfoProvider.IsBackgroundOpened(index)) return;

        var back = _backgroundInfoProvider.GetBackground(index);

        if (!_moneyProvider.CanAfford(back.Price))
        {
            //NO MONEY
            return;
        }

        _moneyProvider.ChangeMoney(-back.Price);

        _backgroundProvider.OpenBackground(back.Index);
    }

    #region Output

    public event Action<IReadOnlyList<Background>> OnSetBackgrounds;
    public event Action<int> OnOpenBackground;

    #endregion
}

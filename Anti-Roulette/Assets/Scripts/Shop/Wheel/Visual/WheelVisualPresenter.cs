using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisualPresenter
{
    private readonly IWheelInfoProvider _wheelInfoProvider;
    private readonly WheelVisualView _view;

    public WheelVisualPresenter(IWheelInfoProvider wheelInfoProvider, WheelVisualView view)
    {
        _wheelInfoProvider = wheelInfoProvider;
        _view = view;
    }

    public void Initialize()
    {
        _view.SetDesign(_wheelInfoProvider.GetCurrentWheel());
    }
}

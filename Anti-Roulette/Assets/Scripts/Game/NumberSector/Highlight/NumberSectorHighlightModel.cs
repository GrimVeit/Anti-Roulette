using System;

public class NumberSectorHighlightModel
{
    private readonly IStoreNumberSectorListener _storeNumberSectorListener;

    private int _currentNumber = -1;
    private bool _isVisible = false;

    public NumberSectorHighlightModel(IStoreNumberSectorListener storeNumberSectorListener)
    {
        _storeNumberSectorListener = storeNumberSectorListener;
    }

    public void Initialize()
    {
        _storeNumberSectorListener.OnChooseNumberColor += SetNumberColor;
    }

    public void Dispose()
    {
        _storeNumberSectorListener.OnChooseNumberColor -= SetNumberColor;
    }

    private void SetNumberColor(NumberColor numberColor)
    {
        if (_currentNumber != -1 && _isVisible)
        {
            OnHideHighlight?.Invoke(_currentNumber);
        }

        _currentNumber = numberColor.Number;
        _isVisible = false;
    }

    public void Show()
    {
        if (_currentNumber == -1) return;
        if (_isVisible) return;

        _isVisible = true;
        OnShowHighlight?.Invoke(_currentNumber);
    }

    public void Hide()
    {
        if (_currentNumber == -1) return;
        if (!_isVisible) return;

        _isVisible = false;
        OnHideHighlight?.Invoke(_currentNumber);
    }

    #region Output

    public event Action<int> OnHideHighlight;
    public event Action<int> OnShowHighlight;

    #endregion
}

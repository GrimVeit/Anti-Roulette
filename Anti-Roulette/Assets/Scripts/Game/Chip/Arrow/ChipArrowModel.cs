using System;

public class ChipArrowModel
{
    public void Activate()
    {
        OnActivate?.Invoke();
    }

    public void Deactivate()
    {
        OnDeactivate?.Invoke();
    }

    #region Input

    public event Action OnActivate;
    public event Action OnDeactivate;

    #endregion
}

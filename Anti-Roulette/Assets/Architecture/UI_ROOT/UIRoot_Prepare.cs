using UnityEngine;

public class UIRoot_Prepare : UIRoot
{
    [SerializeField] private FadePanel _fadePanel;

    public override void Initialize()
    {
        base.Initialize();

        _fadePanel.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        _fadePanel.Dispose();
    }

    #region Input

    public void ShowPreparePanel()
    {
        ShowPanel(_fadePanel);
    }

    public void HidePreparePanel()
    {
        HidePanel(_fadePanel);
    }

    #endregion
}

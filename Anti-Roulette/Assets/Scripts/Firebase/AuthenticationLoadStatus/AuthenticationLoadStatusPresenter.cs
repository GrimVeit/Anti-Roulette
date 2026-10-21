using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationLoadStatusPresenter : IAuthLoadStatusProvider
{
    private readonly AuthenticationLoadStatusModel _model;
    private readonly AuthenticationLoadStatusView _view;

    public AuthenticationLoadStatusPresenter(AuthenticationLoadStatusModel model, AuthenticationLoadStatusView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnShow += _view.Show;
        _model.OnLoad += _view.Load;
    }

    private void DeactivateEvents()
    {
        _model.OnShow -= _view.Show;
        _model.OnLoad -= _view.Load;
    }

    #region Input

    public void Load() => _model.Load();
    public void Show() => _model.Show();

    #endregion
}

public interface IAuthLoadStatusProvider
{
    public void Load();
    public void Show();
}

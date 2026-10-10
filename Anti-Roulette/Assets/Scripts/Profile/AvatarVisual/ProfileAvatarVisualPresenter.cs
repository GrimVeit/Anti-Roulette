using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileAvatarVisualPresenter
{
    private readonly ProfileAvatarVisualModel _model;
    private readonly ProfileAvatarVisualView _view;

    public ProfileAvatarVisualPresenter(ProfileAvatarVisualModel model, ProfileAvatarVisualView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnSetAvatar += _view.SetAvatar;
    }

    private void DeactivateEvents()
    {
        _model.OnSetAvatar -= _view.SetAvatar;
    }
}

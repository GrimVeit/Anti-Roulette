using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileAvatarVisualModel
{
    private readonly IPlayerProfileInfoProvider _profileInfoProvider;
    private readonly IPlayerProfileEventsProvider _profileEventsProvider;

    public ProfileAvatarVisualModel(IPlayerProfileInfoProvider profileInfoProvider, IPlayerProfileEventsProvider profileEventsProvider)
    {
        _profileInfoProvider = profileInfoProvider;
        _profileEventsProvider = profileEventsProvider;
    }

    public void Initialize()
    {
        _profileEventsProvider.OnAvatarChanged += SetAvatar;

        SetAvatar(_profileInfoProvider.Profile.Avatar);
    }

    public void Dispose()
    {
        _profileEventsProvider.OnAvatarChanged -= SetAvatar;
    }

    private void SetAvatar(int index)
    {
        OnSetAvatar?.Invoke(index);
    }

    #region Output

    public event Action<int> OnSetAvatar;

    #endregion
}

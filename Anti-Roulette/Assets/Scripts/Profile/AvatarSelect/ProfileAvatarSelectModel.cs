using System;

public class ProfileAvatarSelectModel
{
    private readonly IPlayerProfileEventsProvider _playerProfileEventsProvider;
    private readonly IPlayerProfileInfoProvider _playerProfileInfoProvider;
    private readonly IPlayerProfileProvider _playerProfileProvider;

    public ProfileAvatarSelectModel(IPlayerProfileEventsProvider playerProfileEventsProvider, IPlayerProfileInfoProvider playerProfileInfoProvider, IPlayerProfileProvider playerProfileProvider)
    {
        _playerProfileEventsProvider = playerProfileEventsProvider;
        _playerProfileInfoProvider = playerProfileInfoProvider;
        _playerProfileProvider = playerProfileProvider;
    }

    public void Initialize()
    {
        _playerProfileEventsProvider.OnAvatarChanged += Select;

        OnSelectAvatar?.Invoke(_playerProfileInfoProvider.Profile.Avatar);
    }

    public void Dispose()
    {
        _playerProfileEventsProvider.OnAvatarChanged -= Select;
    }

    public void SelectAvatar(int index)
    {
        if (!_playerProfileInfoProvider.IsValidAvatar(index)) return;

        var current = _playerProfileInfoProvider.Profile.Avatar;

        if (current == index)
        {
            OnDeselectAvatar?.Invoke(current);
            _playerProfileProvider.ClearAvatar();
            return;
        }

        OnDeselectAvatar?.Invoke(current);
        _playerProfileProvider.SetAvatar(index);
    }

    private void Select(int value)
    {
        OnSelectAvatar?.Invoke(value);
    }

    #region Output

    public event Action<int> OnSelectAvatar;
    public event Action<int> OnDeselectAvatar;

    #endregion
}

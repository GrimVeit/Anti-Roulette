using System;

public sealed class StorePlayerProfilePresenter : IPlayerProfileInfoProvider, IPlayerProfileProvider, IPlayerProfileEventsProvider
{
    private readonly StorePlayerProfileModel _model;

    public StorePlayerProfilePresenter(StorePlayerProfileModel model) => _model = model;

    public void Initialize() => _model.Initialize();
    public void Dispose() => _model.Dispose();

    #region Input

    public void SetNickname(string nickname) => _model.SetNickname(nickname);
    public void SetAvatar(int avatar) => _model.SetAvatar(avatar);
    public void ClearAvatar() => _model.SetAvatar(-1);
    public bool IsValidAvatar(int index) => _model.IsValidAvatar(index);


    #endregion

    #region Output

    public IPlayerProfile Profile => _model.Profile;

    public event Action<IPlayerProfile> OnProfileChanged
    {
        add => _model.OnProfileChanged += value;
        remove => _model.OnProfileChanged -= value;
    }

    public event Action<int> OnAvatarChanged
    {
        add => _model.OnAvatarChanged += value;
        remove => _model.OnAvatarChanged -= value;
    }

    public event Action<string> OnNicknameChanged
    {
        add => _model.OnNicknameChanged += value;
        remove => _model.OnNicknameChanged -= value;
    }

    #endregion
}

public interface IPlayerProfileInfoProvider
{
    IPlayerProfile Profile { get; }
    bool IsValidAvatar(int index);
}

public interface IPlayerProfileProvider
{
    void SetNickname(string nickname);
    void SetAvatar(int avatar);
    void ClearAvatar();
}

public interface IPlayerProfileEventsProvider
{
    event Action<IPlayerProfile> OnProfileChanged;
    event Action<int> OnAvatarChanged;
    event Action<string> OnNicknameChanged;
}

using System;
using UnityEngine;

public sealed class StorePlayerProfileModel : IDisposable
{
    public IPlayerProfile Profile => _profile;
    public event Action<IPlayerProfile> OnProfileChanged;
    public event Action<int> OnAvatarChanged;
    public event Action<string> OnNicknameChanged;

    private readonly string _nicknameKey;
    private readonly string _defaultNickname = "ABCD123";

    private readonly string _avatarKey;
    private readonly int _defaultAvatar = -1;

    private const int AvatarCount = 20;

    private PlayerProfile _profile;

    public StorePlayerProfileModel(string nicknameKey, string avatarKey)
    {
        _nicknameKey = nicknameKey;
        _avatarKey = avatarKey;
    }

    public void Initialize()
    {
        string nickname = PlayerPrefs.GetString(_nicknameKey, _defaultNickname);
        int avatar = PlayerPrefs.GetInt(_avatarKey, _defaultAvatar);

        _profile = new PlayerProfile(nickname, avatar);
    }

    public void SetNickname(string nickname)
    {
        _profile.SetNickname(nickname);

        OnNicknameChanged?.Invoke(nickname);
        OnProfileChanged?.Invoke(_profile);
    }

    public void SetAvatar(int value)
    {
        if (!IsValidAvatar(value))
        {
            Debug.LogWarning("Not valid avatar");
            return;
        }

        _profile.SetAvatar(value);

        OnAvatarChanged?.Invoke(value);
        OnProfileChanged?.Invoke(_profile);
    }

    public void Save()
    {
        if (_profile == null) return;

        PlayerPrefs.SetString(_nicknameKey, _profile.Nickname);
        PlayerPrefs.SetInt(_avatarKey, _profile.Avatar);

        PlayerPrefs.Save();
    }

    public bool IsValidAvatar(int index)
    {
        return index >= -1 && index < AvatarCount;
    }

    public void Dispose()
    {
        Save();
    }
}

public sealed class PlayerProfile : IPlayerProfile
{
    public string Nickname { get; private set; }
    public int Avatar { get; private set; }

    public PlayerProfile(string nickname, int avatar)
    {
        Nickname = nickname;
        Avatar = avatar;
    }

    public void SetAvatar(int value)
    {
        Avatar = value;
    }

    public void SetNickname(string value)
    {
        Nickname = value;
    }
}

public interface IPlayerProfile
{
    string Nickname { get; }
    int Avatar { get; }
}

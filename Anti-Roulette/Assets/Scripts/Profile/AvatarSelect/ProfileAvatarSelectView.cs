using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProfileAvatarSelectView : View
{
    [SerializeField] private List<ProfileAvatarConfig> avatarConfigs = new();
    [SerializeField] private ProfileAvatarSelect profileAvatarSelect_Prefab;

    [Header("Avatar Move")]
    [SerializeField] private Transform transformAvatarPlace;
    [SerializeField] private Transform transformEmpty;
    [SerializeField] private float durationAvatarMove;

    private readonly Dictionary<int, ProfileAvatarSelect> avatars = new();

    private Tween tweenScaleAvatar;

    public void Initialize()
    {
        for (int i = 0; i < avatarConfigs.Count; i++)
        {
            var config = avatarConfigs[i];

            var avatar = Instantiate(profileAvatarSelect_Prefab, config.TransformPos);
            avatar.transform.localPosition = Vector3.zero;
            avatar.Initialize(i, config.TransformPos, config.Sprite);
            avatar.OnChoose += ChooseAvatar;

            avatars.Add(i, avatar);
        }
    }

    public void Dispose()
    {
        foreach (var avatar in avatars.Values)
        {
            avatar.OnChoose -= ChooseAvatar;
            avatar.Dispose();
        }

        avatars.Clear();
    }

    public void Select(int index)
    {
        if (!TryGetShopVisual(index, out ProfileAvatarSelect target))
            return;

        if (target.IsSelected)
            return;

        foreach (var avatar in avatars.Values)
        {
            if (avatar != target && avatar.IsSelected)
                avatar.Deselect(durationAvatarMove);
        }

        tweenScaleAvatar?.Kill();
        tweenScaleAvatar = transformEmpty.DOScale(0, durationAvatarMove);

        target.Select(transformAvatarPlace, durationAvatarMove);
    }

    public void Deselect(int index)
    {
        if (!TryGetShopVisual(index, out ProfileAvatarSelect avatar))
            return;

        if (!avatar.IsSelected)
            return;

        avatar.Deselect(durationAvatarMove);

        tweenScaleAvatar?.Kill();
        tweenScaleAvatar = transformEmpty.DOScale(1, durationAvatarMove);
    }

    private bool TryGetShopVisual(int index, out ProfileAvatarSelect visual)
    {
        if (avatars.TryGetValue(index, out visual))
            return true;

        Debug.LogWarning($"Not found ProfileAvatarSelect with Index - {index}");

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnChooseAvatar;

    private void ChooseAvatar(int index)
    {
        OnChooseAvatar?.Invoke(index);
    }

    #endregion

    [System.Serializable]
    private class ProfileAvatarConfig
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Transform _transformPos;

        public Sprite Sprite => _sprite;
        public Transform TransformPos => _transformPos;
    }
}
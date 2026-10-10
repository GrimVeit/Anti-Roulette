using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProfileAvatarSelect : MonoBehaviour
{
    public event Action<int> OnChoose;

    public int AvatarId => _avatarId;
    public bool IsSelected { get; private set; }

    [Header("References")]
    [SerializeField] private Image imageAvatar;
    [SerializeField] private Button buttonVisualBuy;

    private int _avatarId = -1;
    private Transform _defaultSlot;
    private Tween _tweenMove;

    public void Initialize(int avatarId, Transform defaultSlot, Sprite sprite)
    {
        if (_avatarId != -1)
        {
            Debug.LogWarning($"[{nameof(ProfileAvatarSelect)}] Already initialized with id {_avatarId}");
            return;
        }

        imageAvatar.sprite = sprite;
        _avatarId = avatarId;
        _defaultSlot = defaultSlot;
        buttonVisualBuy.onClick.AddListener(Choose);
    }

    public void Dispose()
    {
        buttonVisualBuy.onClick.RemoveListener(Choose);
        _tweenMove?.Kill();
    }

    public void Select(Transform slot, float duration)
    {
        IsSelected = true;

        transform.SetParent(slot);

        MoveTo(Vector3.zero, duration);
    }

    public void Deselect(float duration)
    {
        IsSelected = false;

        transform.SetParent(_defaultSlot);

        MoveTo(Vector3.zero, duration);
    }

    private void MoveTo(Vector3 vector, float duration)
    {
        _tweenMove?.Kill();
        _tweenMove = transform.DOLocalMove(vector, duration).SetEase(Ease.OutQuad);
    }

    private void Choose()
    {
        OnChoose?.Invoke(_avatarId);
    }
}

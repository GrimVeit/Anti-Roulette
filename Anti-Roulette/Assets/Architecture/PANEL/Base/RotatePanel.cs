using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatePanel : Panel
{
    [Header("Rotation")]
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Tween tween;

    protected override void OnStartShow()
    {
        tween?.Kill();

        tween = transform
            .DOLocalRotate(to, duration)
            .SetUpdate(true)
            .OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        tween?.Kill();

        tween = transform
            .DOLocalRotate(from, duration)
            .SetUpdate(true)
            .OnComplete(CompleteHide);
    }

    public override void Dispose()
    {
        tween?.Kill();
        tween = null;

        base.Dispose();
    }
}

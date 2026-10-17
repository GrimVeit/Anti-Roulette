using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rotate3PointPanel : Panel
{
    [Header("Rotations")]
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 open;
    [SerializeField] private Vector3 exit;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Tween tween;

    protected override void OnStartShow()
    {
        tween?.Kill();

        transform.localEulerAngles = from;

        tween = transform
            .DOLocalRotate(open, duration)
            .SetUpdate(true)
            .OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        tween?.Kill();

        tween = transform
            .DOLocalRotate(exit, duration)
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
